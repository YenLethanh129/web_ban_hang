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
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _taxService = taxService;
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
                    var allImages = dto.Images;
                    var latest = allImages
                        .Where(pi => pi.ProductId == id)
                        .OrderByDescending(pi => pi.Id)
                        .FirstOrDefault();

                    if (latest != null && !string.IsNullOrWhiteSpace(latest.ImageUrl))
                    {
                        vm.ThumbnailPath = latest.ImageUrl;
                    }
                }
                catch (Exception exImg)
                {
                    _logger.LogWarning(exImg, "Could not load latest product image for product {ProductId}", id);
                }

                vm.ProductImages ??= [];
                vm.Recipes ??= [];
                vm.ProductRecipes ??= [];

                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product {ProductId}", id);
                return null;
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
            try
            {
                var productVm = await LoadProductAsync(productId);
                if (productVm == null)
                    return false;

                if (productVm.ProductImages == null || !productVm.ProductImages.Any())
                    return false;

                var img = productVm.ProductImages.FirstOrDefault(i => i.Id == imageId);
                if (img == null)
                    return false;

                productVm.ProductImages.Remove(img);

                var result = await UpdateProductAsync(productVm);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image {ImageId} from product {ProductId}", imageId, productId);
                return false;
            }
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