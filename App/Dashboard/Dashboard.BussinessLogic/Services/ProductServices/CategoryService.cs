using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Products;
using Dashboard.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.BussinessLogic.Services.ProductServices
{
    public interface ICategoryService 
    {
        Task<int> GetCountAsync();
        Task<bool> IsCategoryNameExistsAsync(string name, long? excludeId = null);
        Task<PagedList<CategoryDto>> GetCategoriesAsync(GetCategoriesInput input);
        Task<PagedList<CategoryDto>> GetAllCategories();
        Task<CategoryDto?> CreateCategoryAsync(CreateCategoryInput input);
        Task<CategoryDto?> UpdateCategoryAsync(long id, UpdateCategoryInput input);
        Task<bool> DeleteCategory(long id);
        Task<CategoryDto?> GetCategoryByIdAsync(long id);
        Task<PagedList<CategoryDto>> GetCategoriesContainsAsync(string queryString, string propertyName = nameof(Category.Name));

    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper) : base()
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(long id)
        {
            var entity = await _categoryRepository.GetAsync(id);
            if (entity == null) return null;
            return _mapper.Map<CategoryDto>(entity);
        }
        public async Task<CategoryDto?> CreateCategoryAsync(CreateCategoryInput input)
        {
            var createdEntity = await _categoryRepository.AddAsync(_mapper.Map<Category>(input));
            if (createdEntity == null) return null; 
            return _mapper.Map<CategoryDto>(createdEntity);
        }
        public async Task<int> GetCountAsync()
        {
            var count = await  _categoryRepository.GetAllAsync();
            return count.Count();
        }
        public async Task<PagedList<CategoryDto>> GetCategoriesAsync(GetCategoriesInput input)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var mappedCategories = _mapper.Map<PagedList<CategoryDto>>(categories);
            return mappedCategories;
        }

        public Task<bool> IsCategoryNameExistsAsync(string name, long? excludeId = null)
        {
            var query = _categoryRepository.GetQueryable(true)
                .Where(c => c.Name.ToLower().Contains(name.ToLower()));
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }
            return Task.FromResult(query.Any());    
        }
        public async Task<CategoryDto?> UpdateCategoryAsync(long id, UpdateCategoryInput input)
        { 
            var existingCategory = await _categoryRepository.GetAsync(id);
            if (existingCategory == null) return null;
            existingCategory.Name = input.Name ?? existingCategory.Name;
            _categoryRepository.Update(existingCategory);
            return _mapper.Map<CategoryDto>(existingCategory);

        }

        public async Task<bool> DeleteCategory(long id)
        {
            var existingCategory = await _categoryRepository.GetAsync(id);
            if (existingCategory == null) return false;

            _categoryRepository.Remove(existingCategory);
            return true;
        }

        public async Task<PagedList<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var pagedCategories = new PagedList<CategoryDto>
            {
                Items = _mapper.Map<IEnumerable<CategoryDto>>(categories),
                TotalRecords = categories.Count(),
                PageSize = categories.Count(),
                PageNumber = 1
            };
            return pagedCategories;
        }

        public async Task<PagedList<CategoryDto>> GetCategoriesContainsAsync(string queryString, string propertyName = nameof(Category.Name))
        {
            var categories = await _categoryRepository.GetContainString(propertyName, queryString, true);
            var mappedCategories = _mapper.Map<PagedList<CategoryDto>>(categories);
            return mappedCategories;
        }

    }
}
