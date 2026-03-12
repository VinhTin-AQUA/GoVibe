using AutoMapper;
using AutoMapper.Configuration.Conventions;
using GoVibe.API.Exceptions;
using GoVibe.API.Models;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Repositories.Places;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Xml.Linq;

namespace GoVibe.API.Services
{
    public class PlaceService
    {
        private readonly IPlaceQueryRepository placeQueryRepository;
        private readonly IPlaceCommandRepository placeCommandRepository;
        private readonly IMapper mapper;

        public PlaceService(
            IPlaceQueryRepository placeQueryRepository,
            IPlaceCommandRepository placeCommandRepository,
            IMapper mapper)
        {
            this.placeQueryRepository = placeQueryRepository;
            this.placeCommandRepository = placeCommandRepository;
            this.mapper = mapper;
        }

        public async Task<PlaceModel> Add(AddPlaceRequest request)
        {
            var nameExists = await placeQueryRepository.ExistsAsync((x) => x.Name.ToLower() == request.Name.ToLower());
            if (nameExists)
            {
                throw new ArgumentException("Place Name already exists");
            }

            Place newPlace = new()
            {
                Name = request.Name,
                Description = request.Description,
                Street = request.Street,
                Ward = request.Ward,
                District = request.District,
                City = request.City,
                Country = request.Country,
                CategoryId = Guid.Parse(request.CategoryId),
                Phone = request.Phone,
                Website = request.Website,
                OpeningHours = request.OpeningHours,
                Status = request.Status,
            };

            await placeCommandRepository.AddAsync(newPlace);
            var r = await placeCommandRepository.SaveChangesAsync();

            return mapper.Map<PlaceModel>(newPlace);
        }

        public async Task<Pagination<PlaceModel>> GetAllPagination(int pageIndex = 0, int pageSize = 20)
        {
            (List<Place> places, int total) = await placeQueryRepository.GetAllPagination(pageIndex, pageSize);

            return new Pagination<PlaceModel>
            {
                Items = mapper.Map<List<PlaceModel>>(places),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<PlaceModel> Update(UpdatePlaceRequest request)
        {
            var place = await placeQueryRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (place == null)
            {
                throw new NotFoundException("Amenty not found");
            }

            place.Name = request.Name;
            place.Description = request.Description;
            place.Street = request.Street;
            place.Ward = request.Ward;
            place.District = request.District;
            place.City = request.City;
            place.Country = request.Country;
            place.CategoryId = Guid.Parse(request.CategoryId);
            place.Phone = request.Phone;
            place.Website = request.Website;
            place.OpeningHours = request.OpeningHours;
            place.Status = request.Status;

            await placeCommandRepository.UpdateAsync(place);
            var r = await placeCommandRepository.SaveChangesAsync();

            return mapper.Map<PlaceModel>(place);
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
