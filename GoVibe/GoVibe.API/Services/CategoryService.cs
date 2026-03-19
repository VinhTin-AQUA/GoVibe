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
        private readonly ICategoryCommandRepository _categoryCommandRepository;
        private readonly ICategoryQueryRepository _categoryQueryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryCommandRepository categoryCommandRepository,
            ICategoryQueryRepository categoryQueryRepository,
            IMapper mapper)
        {
            _categoryCommandRepository = categoryCommandRepository;
            _categoryQueryRepository = categoryQueryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryModel> Add(AddCategoryRequest request)
        {
            var nameExists = await _categoryQueryRepository.ExistsAsync((x) => x.Name.ToLower() == request.Name.ToLower());
            if (nameExists)
            {
                throw new ArgumentException("Category Name already exists");
            }

            Category newCategory = new() { Name = request.Name, Description = request.Description };
            await _categoryCommandRepository.AddAsync(newCategory);
            var r = await _categoryCommandRepository.SaveChangesAsync();

            return _mapper.Map<CategoryModel>(newCategory);
        }
        
        public async Task<CategoryModel> GetById(string id)
        {
            var category = await _categoryQueryRepository.GetByIdAsync(Guid.Parse(id));

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<Pagination<CategoryModel>> GetAllPagination(string searchString = "", int pageIndex = 1, int pageSize = 20)
        {
            pageIndex = Math.Max(pageIndex, 1);   // >= 1
            pageSize = Math.Min(pageSize, 50);    // <= 50

            (List<Category> categories, int total) = await _categoryQueryRepository.GetAllPagination(searchString, pageIndex, pageSize);

            return new Pagination<CategoryModel>
            {
                Items = _mapper.Map<List<CategoryModel>>(categories),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                TotalPage = total / pageSize + 1
            };
        }

        public async Task<CategoryModel> Update(UpdateCategoryRequest request)
        {
            var category = await _categoryQueryRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            category.Name = request.Name;
            category.Description = request.Description;

            await _categoryCommandRepository.UpdateAsync(category);
            var r = await _categoryCommandRepository.SaveChangesAsync();

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel> Delete(string id)
        {
            var category = await _categoryQueryRepository.GetByIdAsync(Guid.Parse(id));
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            await _categoryCommandRepository.DeleteAsync(category);
            var r = await _categoryCommandRepository.SaveChangesAsync();
            return _mapper.Map<CategoryModel>(category);
        }

        public async Task DeleteMany(DeleteManyCategoriesRequest request)
        {
            await _categoryCommandRepository.DeleteRangeAsync(request.Ids.Select(x => Guid.Parse(x)));
            var r = await _categoryCommandRepository.SaveChangesAsync();
        }

        public async Task<List<Options<string, string>>> GetOptions()
        {
            var list = await _categoryQueryRepository.GetAllAsync();
            return _mapper.Map<List<Options<string, string>>>(list);
        }
    }
}
