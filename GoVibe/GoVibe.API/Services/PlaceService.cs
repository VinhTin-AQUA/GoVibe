using AutoMapper;
using GoVibe.API.Configurations;
using GoVibe.API.Exceptions;
using GoVibe.API.Models;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.Repositories.PlaceImages;
using GoVibe.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace GoVibe.API.Services
{
    public class PlaceService
    {
        private readonly IPlaceQueryRepository _placeQueryRepository;
        private readonly IPlaceCommandRepository _placeCommandRepository;
        private readonly IPlaceImageCommandRepository _placeImageCommandRepository;
        private readonly IPlaceImageQueryRepository _placeImageQueryRepository;
        private readonly IPlaceCategoryQueryRepository _placeCategoryQueryRepository;
        private readonly IPlaceCategoryCommandRepository _placeCategoryCommandRepository;
        private readonly GarageService _garageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaceService(
            IPlaceQueryRepository placeQueryRepository,
            IPlaceCommandRepository placeCommandRepository,
            IPlaceImageCommandRepository  placeImageCommandRepository,
            IPlaceImageQueryRepository  placeImageQueryRepository,
            IPlaceCategoryQueryRepository placeCategoryQueryRepository,
            IPlaceCategoryCommandRepository placeCategoryCommandRepository,
            GarageService garageService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _placeQueryRepository = placeQueryRepository;
            _placeCommandRepository = placeCommandRepository;
            _placeImageCommandRepository = placeImageCommandRepository;
            _placeImageQueryRepository = placeImageQueryRepository;
            _placeCategoryQueryRepository = placeCategoryQueryRepository;
            _placeCategoryCommandRepository = placeCategoryCommandRepository;
            _garageService = garageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PlaceModel> Add(AddPlaceRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Place newPlace = new()
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Country = request.Country,
                    Phone = request.Phone,
                    Website = request.Website,
                    OpeningHours = request.OpeningHours,
                    Status = request.Status,
                    Address = request.Address,
                    Tags = request.Tags,
                };

                List<PlaceImage> placeImages = [];
                foreach (var image in request.Images)
                {
                    string url = await _garageService.Upload(BucketPrefixKeyNames.PlaceImages, image);
                    PlaceImage placeImage = new()
                    {
                        PlaceId = newPlace.Id,
                        ImageUrl = url,
                    };
                    placeImages.Add(placeImage);
                }

                newPlace.Thumbnail = placeImages[0].ImageUrl;
                await _placeCommandRepository.AddAsync(newPlace);
                await _placeImageCommandRepository.AddRangeAsync(placeImages);
                
                List<PlaceCategory> placeCategories = [];
                foreach (var categoryId in request.CategoryIds)
                {
                    var placeCategory = new PlaceCategory
                    {
                        CategoryId = categoryId,
                        PlaceId = newPlace.Id
                    };
                    placeCategories.Add(placeCategory);
                }
                await _placeCategoryCommandRepository.AddRangeAsync(placeCategories);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<PlaceModel>(newPlace);
            }
            catch
            {
                try
                {
                    await _unitOfWork.RollbackAsync();
                }
                catch
                {
                    // ignore rollback exception
                }

                throw; // giữ nguyên exception gốc
            }
        }

        public async Task<Pagination<PlaceModel>> GetAllPagination(string searchString = "", int pageIndex = 0, int pageSize = 20)
        {
            pageIndex = Math.Max(pageIndex, 1);   // >= 1
            pageSize = Math.Min(pageSize, 50);    // <= 50

            (List<Place> places, int total) = await _placeQueryRepository.GetAllPagination(searchString, pageIndex, pageSize);
            return new Pagination<PlaceModel>
            {
                Items = _mapper.Map<List<PlaceModel>>(places),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                TotalPage = total / pageSize + 1
            };
        }

        public async Task<PlaceDetailsModel> Get(string id)
        {
            var place = await _placeQueryRepository.GetByIdAsync(Guid.Parse(id), false, [
                    x => x.Include(p => p.PlaceCategories).ThenInclude(pc => pc.Category),  
                    q => q.Include(x => x.Images), 
                    q => q.Include(x => x.Reviews)
                ]);

            if (place == null)
            {
                throw new NotFoundException("Place not found");
            }
            return _mapper.Map<PlaceDetailsModel>(place);
        }

        public async Task<PlaceModel> Update(UpdatePlaceRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var place = await _placeQueryRepository.GetByIdAsync(Guid.Parse(request.Id), false, x => x.Include(x => x.PlaceCategories));

                if (place == null)
                {
                    throw new NotFoundException("Amenty not found");
                }

                place.Name = request.Name;
                place.Description = request.Description;
                place.Address = request.Address;
                place.Country = request.Country;
                place.Phone = request.Phone;
                place.Website = request.Website;
                place.OpeningHours = request.OpeningHours;
                place.Status = request.Status;
                place.Tags = request.Tags;
                await _placeCommandRepository.UpdateAsync(place);

                List<PlaceImage> placeImages = [];
                foreach (var image in request.Images)
                {
                    string url = ""; //
                    PlaceImage placeImage = new()
                    {
                        PlaceId = place.Id,
                        ImageUrl = url,
                    };
                    placeImages.Add(placeImage);
                }
                await _placeImageCommandRepository.AddRangeAsync(placeImages);

                // remove deleteImage in storage
                await _garageService.DeleteManyAsync(request.DeleteImages);              
                await _placeImageCommandRepository.DeleteRangeAsync(request.DeleteImages.Select(x => Guid.Parse(x)));

                // existing category
                var existingCategoryIds = place.PlaceCategories
                    .Select(x => x.CategoryId)
                    .ToList();

                // remove old
                var toRemove = place.PlaceCategories
                    .Where(x => !request.CategoryIds.Contains(x.CategoryId))
                    .ToList();

                // new category
                var toAdd = request.CategoryIds
                    .Where(id => !existingCategoryIds.Contains(id))
                    .Select(categoryId => new PlaceCategory
                    {
                        PlaceId = place.Id,
                        CategoryId = categoryId
                    })
                    .ToList();

                await _placeCategoryCommandRepository.DeleteRangeAsync(toRemove);
                await _placeCategoryCommandRepository.AddRangeAsync(toAdd);

                await _unitOfWork.CommitAsync();
                return _mapper.Map<PlaceModel>(place);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<PlaceModel> Delete(string id)
        {
            var place = await _placeQueryRepository.GetByIdAsync(Guid.Parse(id));
            if (place == null)
            {
                throw new NotFoundException("Place not found");
            }

            var images = await _placeImageQueryRepository.GetListByPlaceId(place.Id);
            await _garageService.DeleteManyAsync(images.Select(x => x.ImageUrl).ToList());
            
            await _placeCommandRepository.DeleteAsync(place);
            var r = await _placeCommandRepository.SaveChangesAsync();
            return _mapper.Map<PlaceModel>(place);
        }

        public async Task DeleteMany(DeleteManyPlacesRequest request)
        {
            await _placeCommandRepository.DeleteRangeAsync(request.Ids.Select(x => Guid.Parse(x)));
            var r = await _placeCommandRepository.SaveChangesAsync();
        }
    }
}
