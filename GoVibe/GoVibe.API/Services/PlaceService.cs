using AutoMapper;
using GoVibe.API.Configurations;
using GoVibe.API.Exceptions;
using GoVibe.API.Models;
using GoVibe.API.Models.Categories;
using GoVibe.API.Models.Places;
using GoVibe.Domain.Entities;
using GoVibe.Domain.Enums;
using GoVibe.Infrastructure.Repositories.PlaceImages;
using GoVibe.Infrastructure.Repositories.Places;
using GoVibe.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GoVibe.Infrastructure.Repositories.PlaceCategories;

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

                newPlace.Thumbnail = placeImages.Count> 0 ? placeImages[0].ImageUrl : "https://upload.wikimedia.org/wikipedia/commons/3/3f/JPEG_example_flower.jpg";
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
            catch(Exception ex)
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

        public async Task<Pagination<PlaceModel>> GetAllPagination(string searchString = "", int pageIndex = 1, int pageSize = 20)
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

        public async Task<(
            List<PlaceModel> topRated, 
            List<PlaceModel> mostViewed,
            List<PlaceModel> recent,
            List<PlaceModel> explore,
            int totalPlaces,
            double averageRating,
            int totalReviews,
            int totalViews
        )> GetHome()
        {
            var query = await _placeQueryRepository.GetAllAsync(false);

            var topRated = query
                .Where(p => p.TotalReviews > 0)
                .OrderByDescending(p => p.TotalRating / p.TotalReviews)
                .ThenByDescending(p => p.TotalReviews)
                .Take(4)
                .ToList();

            var mostViewed = query
                .OrderByDescending(p => p.TotalViews)
                .Take(4)
                .ToList();

            var recent = query
                .OrderByDescending(p => p.UpdatedAt)
                .Take(4)
                .ToList();

            var explore = query
                .Where(p => p.TotalReviews > 0 &&
                            (p.TotalRating / p.TotalReviews) >= 3)
                .OrderBy(p => Guid.NewGuid())
                .Take(4)
                .ToList();

            int totalPlaces = await _placeCategoryQueryRepository.CountAsync(x => true);
            double averageRating = query.Where(p => p.TotalReviews > 0).Select(x => x.TotalRating).Average();
            int totalReviews = query.Where(p => p.TotalReviews > 0).Select(x => x.TotalReviews).Sum();
            int totalViews = query.Where(p => p.TotalReviews > 0).Select(x => x.TotalViews).Sum();

            return (
                _mapper.Map<List<PlaceModel>>(topRated),
                _mapper.Map<List<PlaceModel>>(mostViewed),
                _mapper.Map<List<PlaceModel>>(recent),
                _mapper.Map<List<PlaceModel>>(explore),
                totalPlaces,
                averageRating,
                totalReviews,
                totalViews
            );
        }

        public async Task<Pagination<PlaceModel>> Search([FromBody] PlaceSearchRequest request)
        {
            var queries = new List<Func<IQueryable<Place>, IQueryable<Place>>>();

            // Default filter
            //queries.Add(q => q.Where(p => p.Status == EPlaceStatus.Active));

            // Keyword
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.ToLower();
                queries.Add(q => q.Where(p =>
                    p.Name.ToLower().Contains(keyword) ||
                    p.Address.ToLower().Contains(keyword)));
            }
            
            // Country
            if (!string.IsNullOrWhiteSpace(request.Country))
            {
                queries.Add(q => q.Where(p => p.Country == request.Country));
            }

            // Category
            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                queries.Add(q => q.Where(p =>
                    p.PlaceCategories.Any(pc =>
                        request.CategoryIds.Contains(pc.CategoryId))));
            }

            // Rating
            if (request.MinRating.HasValue)
            {
                queries.Add(q => q.Where(p =>
                    p.TotalReviews > 0 &&
                    (p.TotalRating / p.TotalReviews) >= request.MinRating.Value));
            }

            if (request.MaxRating.HasValue)
            {
                queries.Add(q => q.Where(p =>
                    p.TotalReviews > 0 &&
                    (p.TotalRating / p.TotalReviews) <= request.MaxRating.Value));
            }

            // Views
            if (request.MinViews.HasValue)
            {
                queries.Add(q => q.Where(p => p.TotalViews >= request.MinViews));
            }

            if (request.MaxViews.HasValue)
            {
                queries.Add(q => q.Where(p => p.TotalViews <= request.MaxViews));
            }

            // Tags
            if (request.Tags != null && request.Tags.Any())
            {
                queries.Add(q => q.Where(p =>
                    p.Tags.Any(tag => request.Tags.Contains(tag))));
            }

            // Sorting
            queries.Add(request.SortBy?.ToLower() switch
            {
                "rating" => request.SortDesc
                    ? (Func<IQueryable<Place>, IQueryable<Place>>)(q =>
                        q.OrderByDescending(p => p.TotalReviews == 0
                            ? 0
                            : p.TotalRating / p.TotalReviews))
                    : q => q.OrderBy(p => p.TotalReviews == 0
                            ? 0
                            : p.TotalRating / p.TotalReviews),

                "views" => request.SortDesc
                    ? q => q.OrderByDescending(p => p.TotalViews)
                    : q => q.OrderBy(p => p.TotalViews),

                "newest" => request.SortDesc
                    ? q => q.OrderByDescending(p => p.CreatedAt)
                    : q => q.OrderBy(p => p.CreatedAt),

                _ => q => q.OrderByDescending(p => p.UpdatedAt)
            });

            // 10. Query DB
            var places = await _placeQueryRepository.GetPagedAsync(
                request.PageIndex,
                request.PageSize,
                queries.ToArray());
            var total = await _placeQueryRepository.CountAsync(x => true, queries.ToArray());

            // 11. Map sang DTO
            var result = places.Select(p => new PlaceModel
            {
                Id = p.Id,
                Name = p.Name,
                TotalViews = p.TotalViews,
                TotalReviews = p.TotalReviews,
                TotalRating = (int)p.TotalRating,
                AverageRating = p.TotalReviews == 0
                    ? 0
                    : p.TotalRating / p.TotalReviews,
                Thumbnail = p.Thumbnail,
                Status = p.Status,
                UpdatedAt = p.UpdatedAt,
                Categories = p.PlaceCategories
                    .Select(pc => new CategoryModel
                    {
                        Id = pc.Category!.Id,
                        Name = pc.Category.Name
                    })
                    .ToList()
            }).ToList();

            return new Pagination<PlaceModel>
            {
                Items = _mapper.Map<List<PlaceModel>>(places),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = total,
                TotalPage = total / request.PageSize + 1
            };
        }
    }
}
