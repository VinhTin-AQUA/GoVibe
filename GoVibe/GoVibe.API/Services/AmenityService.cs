using AutoMapper;
using GoVibe.API.Exceptions;
using GoVibe.API.Models;
using GoVibe.API.Models.Amenities;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Repositories.Amenities;

namespace GoVibe.API.Services
{
    public class AmenityService
    {
        private readonly AmenityQueryRepository amenityQueryRepository;
        private readonly AmenityCommandRepository amenityCommandRepository;
        private readonly IMapper mapper;

        public AmenityService(
            AmenityQueryRepository amenityQueryRepository,
            AmenityCommandRepository amenityCommandRepository,
            IMapper mapper)
        {
            this.amenityQueryRepository = amenityQueryRepository;
            this.amenityCommandRepository = amenityCommandRepository;
            this.mapper = mapper;
        }

        public async Task<AmenityModel> Add(AddAmenityRequest request)
        {
            var nameExists = await amenityQueryRepository.ExistsAsync((x) => x.Name.ToLower() == request.Name.ToLower());
            if (nameExists)
            {
                throw new ArgumentException("Amenity Name already exists");
            }

            Amenity newAmenity = new() { Name = request.Name, Status = request.Status };
            await amenityCommandRepository.AddAsync(newAmenity);
            var r = await amenityCommandRepository.SaveChangesAsync();

            return mapper.Map<AmenityModel>(newAmenity);
        }

        public async Task<Pagination<AmenityModel>> GetAllPagination(int pageIndex = 0, int pageSize = 20)
        {
            (List<Amenity> amenities, int total) = await amenityQueryRepository.GetAllPagination(pageIndex, pageSize);

            return new Pagination<AmenityModel>
            {
                Items = mapper.Map<List<AmenityModel>>(amenities),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<AmenityModel> Update(UpdateAmenityRequest request)
        {
            var amenity = await amenityQueryRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (amenity == null)
            {
                throw new NotFoundException("Amenty not found");
            }

            amenity.Name = request.Name;
            amenity.Status = request.Status;
            await amenityCommandRepository.UpdateAsync(amenity);
            var r = await amenityCommandRepository.SaveChangesAsync();

            return mapper.Map<AmenityModel>(amenity);
        }

        public async Task<AmenityModel> Delete(string id)
        {
            var amenity = await amenityQueryRepository.GetByIdAsync(Guid.Parse(id));
            if (amenity == null)
            {
                throw new NotFoundException("Amenity not found");
            }

            await amenityCommandRepository.DeleteAsync(amenity);
            var r = await amenityCommandRepository.SaveChangesAsync();
            return mapper.Map<AmenityModel>(amenity);
        }

        public async Task DeleteMany(DeleteManyAmenitiesRequest request)
        {
            await amenityCommandRepository.DeleteRangeAsync(request.Ids.Select(x => Guid.Parse(x)));
            var r = await amenityCommandRepository.SaveChangesAsync();
        }
    }
}
