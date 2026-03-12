using AutoMapper;
using GoVibe.API.Exceptions;
using GoVibe.API.Models;
using GoVibe.API.Models.Categories;
using GoVibe.Domain.Entities;
using GoVibe.Infrastructure.Repositories.Categories;

namespace GoVibe.API.Services
{
    public class CategoryService
    {
        private readonly ICategoryCommandRepository categoryCommandRepository;
        private readonly ICategoryQueryRepository categoryQueryRepository;
        private readonly IMapper mapper;

        public CategoryService(ICategoryCommandRepository categoryCommandRepository,
            ICategoryQueryRepository categoryQueryRepository,
            IMapper mapper)
        {
            this.categoryCommandRepository = categoryCommandRepository;
            this.categoryQueryRepository = categoryQueryRepository;
            this.mapper = mapper;
        }

        public async Task<CategoryModel> Add(AddCategoryRequest request)
        {
            var nameExists = await categoryQueryRepository.ExistsAsync((x) => x.Name.ToLower() == request.Name.ToLower());
            if (nameExists)
            {
                throw new ArgumentException("Category Name already exists");
            }

            Category newCategory = new() { Name = request.Name, Description = request.Description };
            await categoryCommandRepository.AddAsync(newCategory);
            var r = await categoryCommandRepository.SaveChangesAsync();

            return mapper.Map<CategoryModel>(newCategory);
        }

        public async Task<Pagination<CategoryModel>> GetAllPagination(int pageIndex = 0, int pageSize = 20)
        {
            (List<Category> categories, int total) = await categoryQueryRepository.GetAllPagination(pageIndex, pageSize);

            return new Pagination<CategoryModel>
            {
                Items = mapper.Map<List<CategoryModel>>(categories),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<CategoryModel> Update(UpdateCategoryRequest request)
        {
            var category = await categoryQueryRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            category.Name = request.Name;
            category.Description = request.Description;

            await categoryCommandRepository.UpdateAsync(category);
            var r = await categoryCommandRepository.SaveChangesAsync();

            return mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel> Delete(string id)
        {
            var category = await categoryQueryRepository.GetByIdAsync(Guid.Parse(id));
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            await categoryCommandRepository.DeleteAsync(category);
            var r = await categoryCommandRepository.SaveChangesAsync();
            return mapper.Map<CategoryModel>(category);
        }

        public async Task DeleteMany(DeleteManyCategoriesRequest request)
        {
            await categoryCommandRepository.DeleteRangeAsync(request.Ids.Select(x => Guid.Parse(x)));
            var r = await categoryCommandRepository.SaveChangesAsync();
        }
    }
}
