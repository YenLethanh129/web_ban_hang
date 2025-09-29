using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Services.ProductServices;
using Dashboard.BussinessLogic.Services.GoodsAndStockServcies;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Winform.Presenters.RecipePresenters
{
    public interface IRecipeDetailPresenter
    {
        Task<RecipeDetailViewModel?> LoadRecipeAsync(long id);
        Task<RecipeDetailViewModel?> CreateRecipeAsync(RecipeDetailViewModel model);
        Task<RecipeDetailViewModel?> UpdateRecipeAsync(RecipeDetailViewModel model);
        Task<List<ProductViewModel>> LoadProductsAsync();
        Task<List<IngredientViewModel>> LoadIngredientsAsync();
        Task<bool> DeleteRecipeAsync(long id);

        // Recipe Ingredient operations
        Task<RecipeIngredientViewModel> AddIngredientToRecipeAsync(long recipeId, long ingredientId, decimal quantity = 1);
        Task<RecipeIngredientViewModel> UpdateRecipeIngredientAsync(RecipeIngredientViewModel model);
        Task<bool> DeleteIngredientFromRecipeAsync(long recipeId, long ingredientId);

        event EventHandler<RecipeDetailViewModel?>? OnRecipeSaved;
        event EventHandler? OnDataLoaded;
        event EventHandler<RecipeIngredientViewModel>? OnIngredientAdded;
        event EventHandler<RecipeIngredientViewModel>? OnIngredientUpdated;
        event EventHandler<long>? OnIngredientDeleted;
    }

    public class RecipeDetailPresenter : IRecipeDetailPresenter
    {
        private readonly ILogger<RecipeDetailPresenter> _logger;
        private readonly IRecipeService _recipeService;
        private readonly IProductService _productService;
        private readonly IIngredientManagementService _ingredientService;
        private readonly IMapper _mapper;

        public event EventHandler<RecipeDetailViewModel?>? OnRecipeSaved;
        public event EventHandler? OnDataLoaded;
        public event EventHandler<RecipeIngredientViewModel>? OnIngredientAdded;
        public event EventHandler<RecipeIngredientViewModel>? OnIngredientUpdated;
        public event EventHandler<long>? OnIngredientDeleted;

        public RecipeDetailPresenter(
            ILogger<RecipeDetailPresenter> logger,
            IRecipeService recipeService,
            IProductService productService,
            IIngredientManagementService ingredientService,
            IMapper mapper)
        {
            _logger = logger;
            _recipeService = recipeService;
            _productService = productService;
            _ingredientService = ingredientService;
            _mapper = mapper;
        }

        public async Task<RecipeDetailViewModel?> LoadRecipeAsync(long id)
        {
            try
            {
                _logger.LogInformation("Loading recipe {RecipeId}", id);

                var dto = await _recipeService.GetRecipeByIdAsync(id);
                if (dto == null)
                {
                    _logger.LogWarning("Recipe {RecipeId} not found", id);
                    return null;
                }

                var viewModel = _mapper.Map<RecipeDetailViewModel>(dto);

                if (dto.RecipeIngredients?.Count != 0)
                {
                    viewModel.RecipeIngredients.Clear();
                    foreach (var ingredient in dto.RecipeIngredients!)
                    {
                        var ingredientVm = _mapper.Map<RecipeIngredientViewModel>(ingredient);
                        viewModel.RecipeIngredients.Add(ingredientVm);
                    }
                }

                _logger.LogInformation("Successfully loaded recipe {RecipeId}: {RecipeName}", id, viewModel.Name);
                OnDataLoaded?.Invoke(this, EventArgs.Empty);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading recipe {RecipeId}", id);
                throw;
            }
        }

        public async Task<RecipeDetailViewModel?> CreateRecipeAsync(RecipeDetailViewModel model)
        {
            try
            {
                _logger.LogInformation("Creating new recipe: {RecipeName}", model.Name);

                var input = _mapper.Map<CreateRecipeInput>(model);
                var created = await _recipeService.CreateRecipeAsync(input);

                if (created == null)
                {
                    _logger.LogWarning("Failed to create recipe: {RecipeName}", model.Name);
                    return null;
                }

                var viewModel = _mapper.Map<RecipeDetailViewModel>(created);

                _logger.LogInformation("Successfully created recipe {RecipeId}: {RecipeName}", created.Id, created.Name);
                OnRecipeSaved?.Invoke(this, viewModel);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating recipe: {RecipeName}", model?.Name);
                throw;
            }
        }

        public async Task<RecipeDetailViewModel?> UpdateRecipeAsync(RecipeDetailViewModel model)
        {
            try
            {
                _logger.LogInformation("Updating recipe {RecipeId}: {RecipeName}", model.Id, model.Name);

                var input = _mapper.Map<UpdateRecipeInput>(model);
                var updated = await _recipeService.UpdateRecipeAsync(input);

                if (updated == null)
                {
                    _logger.LogWarning("Failed to update recipe {RecipeId}: {RecipeName}", model.Id, model.Name);
                    return null;
                }

                var viewModel = _mapper.Map<RecipeDetailViewModel>(updated);

                _logger.LogInformation("Successfully updated recipe {RecipeId}: {RecipeName}", updated.Id, updated.Name);
                OnRecipeSaved?.Invoke(this, viewModel);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe {RecipeId}: {RecipeName}", model?.Id, model?.Name);
                throw;
            }
        }

        public async Task<List<ProductViewModel>> LoadProductsAsync()
        {
            try
            {
                _logger.LogInformation("Loading products for recipe selection");

                var input = new GetProductsInput
                {
                    PageNumber = 1,
                    PageSize = int.MaxValue,
                    IsActive = true
                };

                var pagedResult = await _productService.GetProductsAsync(input);
                var viewModels = _mapper.Map<List<ProductViewModel>>(pagedResult.Items);

                _logger.LogInformation("Loaded {ProductCount} products for recipe selection", viewModels.Count);
                return viewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products for recipe selection");

                return new List<ProductViewModel>
                {
                    new() { Id = 1, Name = "Cà phê đen" },
                    new() { Id = 2, Name = "Cà phê sữa" },
                    new() { Id = 3, Name = "Bánh mì thịt" }
                };
            }
        }

        public async Task<List<IngredientViewModel>> LoadIngredientsAsync()
        {
            try
            {
                _logger.LogInformation("Loading ingredients for recipe");

                var input = new GetIngredientsInput
                {
                    PageNumber = 1,
                    PageSize = int.MaxValue,
                    SearchTerm = string.Empty
                };

                var pagedResult = await _ingredientService.GetIngredientsAsync(input);
                var viewModels = _mapper.Map<List<IngredientViewModel>>(pagedResult.Items);

                _logger.LogInformation("Loaded {IngredientCount} ingredients", viewModels.Count);
                return viewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading ingredients for recipe");
                return new List<IngredientViewModel>();
            }
        }

        public async Task<bool> DeleteRecipeAsync(long id)
        {
            try
            {
                _logger.LogInformation("Deleting recipe {RecipeId}", id);

                var result = await _recipeService.DeleteRecipeAsync(id);

                if (result)
                {
                    _logger.LogInformation("Successfully deleted recipe {RecipeId}", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete recipe {RecipeId}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting recipe {RecipeId}", id);
                throw;
            }
        }

        #region Recipe Ingredient Operations

        public async Task<RecipeIngredientViewModel> AddIngredientToRecipeAsync(long recipeId, long ingredientId, decimal quantity = 1)
        {
            try
            {
                _logger.LogInformation("Adding ingredient {IngredientId} to recipe {RecipeId}", ingredientId, recipeId);

                var input = new CreateRecipeIngredientInput
                {
                    IngredientId = ingredientId,
                    Quantity = quantity,
                    WastePercentage = 0,
                    IsOptional = false
                };

                var dto = await _recipeService.AddRecipeIngredientAsync(recipeId, input);
                var viewModel = _mapper.Map<RecipeIngredientViewModel>(dto);

                _logger.LogInformation("Successfully added ingredient {IngredientId} to recipe {RecipeId}", ingredientId, recipeId);
                OnIngredientAdded?.Invoke(this, viewModel);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ingredient {IngredientId} to recipe {RecipeId}", ingredientId, recipeId);
                throw;
            }
        }

        public async Task<RecipeIngredientViewModel> UpdateRecipeIngredientAsync(RecipeIngredientViewModel model)
        {
            try
            {
                _logger.LogInformation("Updating recipe ingredient {RecipeIngredientId}", model.Id);

                var input = _mapper.Map<UpdateRecipeIngredientInput>(model);
                var dto = await _recipeService.UpdateRecipeIngredientAsync(input);
                var viewModel = _mapper.Map<RecipeIngredientViewModel>(dto);

                _logger.LogInformation("Successfully updated recipe ingredient {RecipeIngredientId}", model.Id);
                OnIngredientUpdated?.Invoke(this, viewModel);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating recipe ingredient {RecipeIngredientId}", model.Id);
                throw;
            }
        }

        public async Task<bool> DeleteIngredientFromRecipeAsync(long recipeId, long ingredientId)
        {
            try
            {
                _logger.LogInformation("Deleting ingredient {IngredientId} from recipe {RecipeId}", ingredientId, recipeId);

                var result = await _recipeService.DeleteRecipeIngredientAsync(recipeId, ingredientId);

                if (result)
                {
                    _logger.LogInformation("Successfully deleted ingredient {IngredientId} from recipe {RecipeId}", ingredientId, recipeId);
                    OnIngredientDeleted?.Invoke(this, ingredientId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete ingredient {IngredientId} from recipe {RecipeId}", ingredientId, recipeId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ingredient {IngredientId} from recipe {RecipeId}", ingredientId, recipeId);
                throw;
            }
        }

        #endregion
    }
}