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
        private readonly IPlaceQueryRepository _placeQueryRepository;
        private readonly IPlaceCommandRepository _placeCommandRepository;
        private readonly IPlaceImageCommandRepository _placeImageCommandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaceService(
            IPlaceQueryRepository placeQueryRepository,
            IPlaceCommandRepository placeCommandRepository,
            IPlaceImageCommandRepository  placeImageCommandRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _placeQueryRepository = placeQueryRepository;
            _placeCommandRepository = placeCommandRepository;
            _placeImageCommandRepository = placeImageCommandRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PlaceModel> Add(AddPlaceRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var nameExists = await _placeQueryRepository.ExistsAsync((x) => x.Name.ToLower() == request.Name.ToLower());
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
                await _placeCommandRepository.AddAsync(newPlace);

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
                await _placeImageCommandRepository.AddRangeAsync(placeImages);
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
            var place = await _placeQueryRepository.GetByIdAsync(Guid.Parse(id), false, [x => x.Category, x => x.Images, x => x.Reviews]);
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
                var place = await _placeQueryRepository.GetByIdAsync(Guid.Parse(request.Id), false, [(x) => x.Category]);
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

                foreach (var deleteImage in request.DeleteImages)
                {
                    // remove deleteImage in storage
                }
                await _placeImageCommandRepository.DeleteRangeAsync(request.DeleteImages.Select(x => Guid.Parse(x)));
                
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
            var amenity = await _placeQueryRepository.GetByIdAsync(Guid.Parse(id));
            if (amenity == null)
            {
                throw new NotFoundException("Amenity not found");
            }

            await _placeCommandRepository.DeleteAsync(amenity);
            var r = await _placeCommandRepository.SaveChangesAsync();
            return _mapper.Map<PlaceModel>(amenity);
        }

        public async Task DeleteMany(DeleteManyPlacesRequest request)
        {
            await _placeCommandRepository.DeleteRangeAsync(request.Ids.Select(x => Guid.Parse(x)));
            var r = await _placeCommandRepository.SaveChangesAsync();
        }
    }
}
