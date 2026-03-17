using AutoMapper;
using GoVibe.API.Exceptions;
using GoVibe.API.Models;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.Repositories.PlaceImages;
using GoVibe.Infrastructure.UnitOfWork;

namespace GoVibe.API.Services
{
    public class PlaceService
    {
        private readonly IPlaceQueryRepository placeQueryRepository;
        private readonly IPlaceCommandRepository placeCommandRepository;
        private readonly IPlaceImageCommandRepository placeImageCommandRepository;
        private readonly IPlaceImageQueryRepository placeImageQueryRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlaceService(
            IPlaceQueryRepository placeQueryRepository,
            IPlaceCommandRepository placeCommandRepository,
            IPlaceImageCommandRepository  placeImageCommandRepository,
            IPlaceImageQueryRepository placeImageQueryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.placeQueryRepository = placeQueryRepository;
            this.placeCommandRepository = placeCommandRepository;
            this.placeImageCommandRepository = placeImageCommandRepository;
            this.placeImageQueryRepository = placeImageQueryRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PlaceModel> Add(AddPlaceRequest request)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();
                var nameExists = await placeQueryRepository.ExistsAsync((x) => x.Name.ToLower() == request.Name.ToLower());
                if (nameExists)
                {
                    throw new ArgumentException("Place Name already exists");
                }

                Place newPlace = new()
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Country = request.Country,
                    CategoryId = Guid.Parse(request.CategoryId),
                    Phone = request.Phone,
                    Website = request.Website,
                    OpeningHours = request.OpeningHours,
                    Status = request.Status,
                    Address = request.Address,
                };
                await placeCommandRepository.AddAsync(newPlace);

                List<PlaceImage> placeImages = [];
                foreach (var image in request.Images)
                {
                    string url = ""; //
                    PlaceImage placeImage = new()
                    {
                        PlaceId = newPlace.Id,
                        ImageUrl = url,
                    };
                    placeImages.Add(placeImage);
                }
                await placeImageCommandRepository.AddRangeAsync(placeImages);
                await unitOfWork.CommitAsync();
                return mapper.Map<PlaceModel>(newPlace);
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<Pagination<PlaceModel>> GetAllPagination(int pageIndex = 0, int pageSize = 20)
        {
            pageIndex = Math.Max(pageIndex, 1);   // >= 1
            pageSize = Math.Min(pageSize, 50);    // <= 50
            (List<Place> places, int total) = await placeQueryRepository.GetAllPagination(pageIndex, pageSize);

            return new Pagination<PlaceModel>
            {
                Items = mapper.Map<List<PlaceModel>>(places),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<PlaceDetailsModel> Get(string id)
        {
            var place = await placeQueryRepository.GetByIdAsync(Guid.Parse(id), false, [x => x.Category, x => x.Images, x => x.Reviews]);
            if (place == null)
            {
                throw new NotFoundException("Place not found");
            }
            return mapper.Map<PlaceDetailsModel>(place);
        }

        public async Task<PlaceModel> Update(UpdatePlaceRequest request)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();
                var place = await placeQueryRepository.GetByIdAsync(Guid.Parse(request.Id), false, [(x) => x.Category]);
                if (place == null)
                {
                    throw new NotFoundException("Amenty not found");
                }

                place.Name = request.Name;
                place.Description = request.Description;
                place.Address = request.Address;
                place.Country = request.Country;
                place.CategoryId = Guid.Parse(request.CategoryId);
                place.Phone = request.Phone;
                place.Website = request.Website;
                place.OpeningHours = request.OpeningHours;
                place.Status = request.Status;
                await placeCommandRepository.UpdateAsync(place);

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
                await placeImageCommandRepository.AddRangeAsync(placeImages);

                foreach (var deleteImage in request.DeleteImages)
                {
                    // remove deleteImage in storage
                }
                await placeImageCommandRepository.DeleteRangeAsync(request.DeleteImages.Select(x => Guid.Parse(x)));
                
                await unitOfWork.CommitAsync();
                return mapper.Map<PlaceModel>(place);
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<PlaceModel> Delete(string id)
        {
            var amenity = await placeQueryRepository.GetByIdAsync(Guid.Parse(id));
            if (amenity == null)
            {
                throw new NotFoundException("Amenity not found");
            }

            await placeCommandRepository.DeleteAsync(amenity);
            var r = await placeCommandRepository.SaveChangesAsync();
            return mapper.Map<PlaceModel>(amenity);
        }

        public async Task DeleteMany(DeleteManyPlacesRequest request)
        {
            await placeCommandRepository.DeleteRangeAsync(request.Ids.Select(x => Guid.Parse(x)));
            var r = await placeCommandRepository.SaveChangesAsync();
        }
    }
}
