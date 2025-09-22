using AutoMapper;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Services.GoodsAndStockServcies;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Winform.Presenters
{
    public interface IIngredientDetailPresenter
    {
        Task<IngredientDetailViewModel?> LoadIngredientAsync(long id);
        Task<bool> DeleteIngredientAsync(long id);
        Task SaveIngredientAsync(IngredientDetailViewModel model);
        Task<List<IngredientCategoryViewModel>> LoadCategoriesAsync();

        event EventHandler<IngredientDetailViewModel?>? OnIngredientSaved;
    }

    public class IngredientDetailPresenter : IIngredientDetailPresenter
    {
        private readonly ILogger<IngredientDetailPresenter> _logger;
        private readonly IIngredientManagementService _ingredientService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public event EventHandler<IngredientDetailViewModel?>? OnIngredientSaved;

        public IngredientDetailPresenter(
            ILogger<IngredientDetailPresenter> logger,
            IIngredientManagementService ingredientService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _ingredientService = ingredientService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IngredientDetailViewModel?> LoadIngredientAsync(long id)
        {
            try
            {
                var dto = await _ingredientService.GetIngredientByIdAsync(id);
                if (dto == null)
                {
                    _logger.LogWarning("Ingredient with ID {IngredientId} not found", id);
                    return null;
                }

                var vm = _mapper.Map<IngredientDetailViewModel>(dto);
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading ingredient {IngredientId}", id);
                throw;
            }
        }

        public async Task SaveIngredientAsync(IngredientDetailViewModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            IngredientDetailViewModel vm;
            if (model.Id == 0)
            {
                var input = _mapper.Map<CreateIngredientInput>(model);
                var created = await _ingredientService.CreateIngredientAsync(input);
                vm = _mapper.Map<IngredientDetailViewModel>(created);
            }
            else
            {
                var input = _mapper.Map<UpdateIngredientInput>(model);

                var updated = await _ingredientService.UpdateIngredientAsync(input);
                vm = _mapper.Map<IngredientDetailViewModel>(updated);
            }

            OnIngredientSaved?.Invoke(this, vm);
        }

        public async Task<bool> DeleteIngredientAsync(long id)
        {
            try
            {
                return await _ingredientService.DeleteIngredientAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ingredient {IngredientId}", id);
                throw;
            }
        }

        public async Task<List<IngredientCategoryViewModel>> LoadCategoriesAsync()
        {
            try
            {
                var repo = _unitOfWork.Repository<IngredientCategory>();
                var categories = await repo.GetAllAsync(true);

                return [.. categories.Select(c => new IngredientCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name ?? string.Empty,
                    Description = c.Description
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
                return GetDefaultCategories();
            }
        }

        private List<IngredientCategoryViewModel> GetDefaultCategories()
        {
            return new List<IngredientCategoryViewModel>
            {
                new() { Id = 1, Name = "Rau củ", Description = "Các loại rau củ quả" },
                new() { Id = 2, Name = "Thịt cá", Description = "Thịt và hải sản" },
                new() { Id = 3, Name = "Gia vị", Description = "Các loại gia vị" },
                new() { Id = 4, Name = "Ngũ cốc", Description = "Gạo, bột, ngũ cốc" },
                new() { Id = 5, Name = "Sữa và bơ", Description = "Các sản phẩm từ sữa" },
                new() { Id = 6, Name = "Dầu ăn", Description = "Dầu ăn và chất béo" },
                new() { Id = 7, Name = "Khác", Description = "Nguyên liệu khác" }
            };
        }
    }
}