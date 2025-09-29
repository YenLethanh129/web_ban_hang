using AutoMapper;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.BussinessLogic.Services.ProductServices;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Products;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Winform.Presenters
{
    public interface IProductDetailPresenter
    {
        Task<ProductDetailViewModel?> LoadProductAsync(long id);
        Task<ProductDetailViewModel?> CreateProductAsync(ProductDetailViewModel model);
        Task<ProductDetailViewModel?> UpdateProductAsync(ProductDetailViewModel model);

        Task<bool> AddImageAsync(long productId, string imageUrl, bool isPrimary = false);
        Task<bool> DeleteImageAsync(long productId, long imageId);

        Task<bool> AddOrUpdateProductRecipeAsync(ProductRecipeViewModel productRecipe);
        Task<bool> DeleteProductRecipeAsync(long productId, long productRecipeId);

        // Recipe management methods
        Task<List<RecipeViewModel>> LoadAllRecipesAsync();
        Task<List<RecipeViewModel>> LoadRecipesByProductIdAsync(long productId);
        Task<bool> AssignRecipeToProductAsync(long productId, long recipeId);
        Task<bool> UnassignRecipeFromProductAsync(long productId, long recipeId);

        Task<List<ProductImageViewModel>> GetProductImagesAsync(long productId);


        // Load dropdown data
        Task<List<CategoryViewModel>> LoadCategoriesAsync();
        Task<List<TaxViewModel>> LoadTaxesAsync();

        event EventHandler<ProductDetailViewModel?>? OnProductSaved;
    }

    public class ProductDetailPresenter : IProductDetailPresenter
    {
        private readonly ILogger<ProductDetailPresenter> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ITaxService _taxService;
        private readonly IRecipeService _recipeService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public event EventHandler<ProductDetailViewModel?>? OnProductSaved;

        public ProductDetailPresenter(
            IServiceProvider serviceProvider,
            ILogger<ProductDetailPresenter> logger,
            IProductService productService,
            ICategoryService categoryService,
            ITaxService taxService,
            IRecipeService recipeService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _taxService = taxService;
            _recipeService = recipeService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDetailViewModel?> LoadProductAsync(long id)
        {
            try
            {
                var dto = await _productService.GetProductByIdAsync(id);
                if (dto == null)
                {
                    return null;
                }

                var vm = _mapper.Map<ProductDetailViewModel>(dto);

                try
                {
                    var productImages = await _productService.GetProductImagesAsync(id);
                    vm.ProductImages = new System.ComponentModel.BindingList<ProductImageViewModel>(
                        _mapper.Map<List<ProductImageViewModel>>(productImages));

                    var thumbnail = await _productService.GetThumbnailImageAsync(id);
                    if (thumbnail != null && !string.IsNullOrWhiteSpace(thumbnail.ImageUrl))
                    {
                        vm.ThumbnailPath = thumbnail.ImageUrl;
                    }
                }
                catch (Exception exImg)
                {
                    _logger.LogWarning(exImg, "Could not load product images for product {ProductId}", id);
                    vm.ProductImages = new System.ComponentModel.BindingList<ProductImageViewModel>();
                }

                vm.Recipes ??= new System.ComponentModel.BindingList<RecipeViewModel>();
                vm.ProductRecipes ??= new System.ComponentModel.BindingList<ProductRecipeViewModel>();

                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product {ProductId}", id);
                return null;
            }
        }


        public async Task<List<ProductImageViewModel>> GetProductImagesAsync(long productId)
        {
            try
            {
                var imageDtos = await _productService.GetProductImagesAsync(productId);
                return _mapper.Map<List<ProductImageViewModel>>(imageDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product images for product {ProductId}", productId);
                return new List<ProductImageViewModel>();
            }
        }

        public async Task<ProductDetailViewModel?> CreateProductAsync(ProductDetailViewModel model)
        {
            try
            {
                var input = _mapper.Map<CreateProductInput>(model);

                var imageUrls = new List<string>();

                if (!string.IsNullOrWhiteSpace(model.ThumbnailPath))
                {
                    imageUrls.Add(model.ThumbnailPath);
                }

                if (model.ProductImages != null)
                {
                    var additionalUrls = model.ProductImages
                        .Where(i => !string.IsNullOrWhiteSpace(i.ImageUrl))
                        .Select(i => i.ImageUrl!)
                        .Where(url => !imageUrls.Contains(url, StringComparer.OrdinalIgnoreCase))
                        .ToList();

                    imageUrls.AddRange(additionalUrls);
                }

                if (imageUrls.Count != 0)
                {
                    input.ImageUrls = imageUrls.Cast<string?>().ToList();
                }

                var created = await _productService.CreateProductAsync(input);
                var vm = _mapper.Map<ProductDetailViewModel>(created);

                try
                {
                    var latest = await _productService.GetThumbnailImageAsync(created.Id);

                    if (latest != null)
                        vm.ThumbnailPath = latest.ImageUrl;
                }
                catch { vm.ThumbnailPath = string.Empty; }

                OnProductSaved?.Invoke(this, vm);
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product {ProductName}", model?.Name);
                throw;
            }
        }

        public async Task<ProductDetailViewModel?> UpdateProductAsync(ProductDetailViewModel model)
        {
            try
            {
                var input = _mapper.Map<UpdateProductInput>(model);
                string? currentThumbnail = model.ThumbnailPath?.Trim();
                bool addThumbnailAsNewImage = false;

                if (model.Id > 0 && !string.IsNullOrEmpty(currentThumbnail))
                {
                    try
                    {
                        var allImages = await _unitOfWork.Repository<ProductImage>().GetAllAsync();
                        var latest = allImages
                            .Where(pi => pi.ProductId == model.Id)
                            .OrderByDescending(pi => pi.Id)
                            .FirstOrDefault();

                        var latestUrl = latest?.ImageUrl?.Trim();

                        if (!string.Equals(currentThumbnail, latestUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            addThumbnailAsNewImage = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not compare product image for product {ProductId}", model.Id);
                        addThumbnailAsNewImage = false;
                    }
                }
                else if (model.Id <= 0 && !string.IsNullOrEmpty(currentThumbnail))
                {
                    addThumbnailAsNewImage = true;
                }

                if (addThumbnailAsNewImage)
                {
                    input.ImageUrls = [currentThumbnail];
                }
                else
                {
                    input.ImageUrls = [];
                }

                var updated = await _productService.UpdateProductAsync(input);
                if (updated == null) return null;

                var vm = _mapper.Map<ProductDetailViewModel>(updated);
                try
                {
                    var latest = await _productService.GetThumbnailImageAsync(updated.Id);

                    if (latest != null)
                        vm.ThumbnailPath = latest.ImageUrl;
                }
                catch { vm.ThumbnailPath = string.Empty; }

                OnProductSaved?.Invoke(this, vm);
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", model?.Id);
                throw;
            }
        }

        public async Task<bool> AddImageAsync(long productId, string imageUrl, bool isPrimary = false)
        {
            try
            {
                var productVm = await LoadProductAsync(productId);
                if (productVm == null)
                    return false;

                if (productVm.ProductImages != null &&
                    productVm.ProductImages.Any(img => string.Equals(img.ImageUrl, imageUrl, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogWarning("Image URL already exists for product {ProductId}: {ImageUrl}", productId, imageUrl);
                    return false;
                }

                productVm.ProductImages ??= new System.ComponentModel.BindingList<ProductImageViewModel>();

                var newImage = new ProductImageViewModel
                {
                    Id = 0,
                    ProductId = productId,
                    ImageUrl = imageUrl,
                    CreatedAt = DateTime.Now
                };

                productVm.ProductImages.Add(newImage);

                var result = await UpdateProductAsync(productVm);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding image to product {ProductId}", productId);
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(long productId, long imageId)
        {
            bool result = await _productService.DeleteProductImageAsync(productId, imageId);

            if (result)
            {
                _logger?.LogInformation("Successfully deleted image {ImageId} from product {ProductId}", imageId, productId);
            }
            else
            {
                _logger?.LogWarning("Failed to delete image {ImageId} from product {ProductId}", imageId, productId);
            }

            return result;
 
        }
        public async Task<bool> AddOrUpdateProductRecipeAsync(ProductRecipeViewModel productRecipe)
        {
            try
            {
                if (productRecipe == null)
                    return false;

                var productVm = await LoadProductAsync(productRecipe.ProductId);
                if (productVm == null)
                    return false;

                productVm.ProductRecipes ??= new System.ComponentModel.BindingList<ProductRecipeViewModel>();

                var existing = productVm.ProductRecipes.FirstOrDefault(pr => pr.Id == productRecipe.Id && pr.Id > 0);
                if (existing != null)
                {
                    existing.IngredientId = productRecipe.IngredientId;
                    existing.IngredientName = productRecipe.IngredientName;
                    existing.Quantity = productRecipe.Quantity;
                    existing.UpdatedAt = DateTime.Now;
                }
                else
                {
                    productRecipe.CreatedAt = DateTime.Now;
                    productVm.ProductRecipes.Add(productRecipe);
                }

                var updated = await UpdateProductAsync(productVm);
                return updated != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding/updating product recipe for product {ProductId}", productRecipe?.ProductId);
                return false;
            }
        }

        public async Task<bool> DeleteProductRecipeAsync(long productId, long productRecipeId)
        {
            try
            {
                var productVm = await LoadProductAsync(productId);
                if (productVm == null)
                    return false;

                if (productVm.ProductRecipes == null || !productVm.ProductRecipes.Any())
                    return false;

                var toRemove = productVm.ProductRecipes.FirstOrDefault(pr => pr.Id == productRecipeId);
                if (toRemove == null)
                    return false;

                productVm.ProductRecipes.Remove(toRemove);

                var updated = await UpdateProductAsync(productVm);
                return updated != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product recipe {RecipeId} from product {ProductId}", productRecipeId, productId);
                return false;
            }
        }

        #region Recipe Management Methods

        public async Task<List<RecipeViewModel>> LoadAllRecipesAsync()
        {
            try
            {
                var recipeDtos = await _recipeService.GetAllRecipesAsync();
                return _mapper.Map<List<RecipeViewModel>>(recipeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all recipes");
                return new List<RecipeViewModel>();
            }
        }

        public async Task<List<RecipeViewModel>> LoadRecipesByProductIdAsync(long productId)
        {
            try
            {
                var recipeDtos = await _recipeService.GetRecipesByProductIdAsync(productId);
                return _mapper.Map<List<RecipeViewModel>>(recipeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading recipes for product {ProductId}", productId);
                return new List<RecipeViewModel>();
            }
        }

        public async Task<bool> AssignRecipeToProductAsync(long productId, long recipeId)
        {
            try
            {
                return await _recipeService.AssignRecipeToProductAsync(productId, recipeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning recipe {RecipeId} to product {ProductId}", recipeId, productId);
                return false;
            }
        }

        public async Task<bool> UnassignRecipeFromProductAsync(long productId, long recipeId)
        {
            try
            {
                return await _recipeService.UnassignRecipeFromProductAsync(productId, recipeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unassigning recipe {RecipeId} from product {ProductId}", recipeId, productId);
                return false;
            }
        }

        #endregion

        public async Task<List<CategoryViewModel>> LoadCategoriesAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return _mapper.Map<List<CategoryViewModel>>(categories.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
                return new List<CategoryViewModel>();
            }
        }

        public async Task<List<TaxViewModel>> LoadTaxesAsync()
        {
            try
            {
                var taxes = await _taxService.GetAllTaxesAsync();
                return _mapper.Map<List<TaxViewModel>>(taxes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading taxes");
                return new List<TaxViewModel>();
            }
        }
    }
}