using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.Common.Enums;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Models.Entities.Products;
using Dashboard.DataAccess.Models.Entities.Orders;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.Extensions.Logging;
using Dashboard.BussinessLogic.Shared;
using System.Text.RegularExpressions;
using Dashboard.Common.Utitlities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Dashboard.BussinessLogic.Services.FileServices;

namespace Dashboard.BussinessLogic.Services.ProductServices;

public interface IProductService
{
    Task<int> GetCountAsync();
    Task<PagedList<ProductDto>> GetProductsAsync(GetProductsInput input);
    Task<ProductDto?> GetProductByIdAsync(long id);
    Task<ProductDto> CreateProductAsync(CreateProductInput input);
    Task<ProductDto> UpdateProductAsync(UpdateProductInput input);
    Task<bool> DeleteProductAsync(long id);
    Task<bool> IsProductNameExistsAsync(string name, long? excludeId = null);
    Task<int> GetAmount(GetProductsInput input);
    Task<Dictionary<long, int>> GetSoldQuantitiesAsync();
    Task<List<ProductImageDto>> GetProductImagesAsync(long id);
    Task<ProductImageDto> GetThumbnailImageAsync(long productId);
    Task<bool> DeleteProductImageAsync(long productId, long imageId); 
}

public class ProductService : BaseTransactionalService, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly IImageUrlValidator _imageUrlValidator;
    private readonly IImageUploadService _imageUploadService;


    public ProductService(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ProductService> logger,
        IImageUrlValidator imageUrlValidator,
        IImageUploadService imageUploadService) : base(unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _imageUrlValidator = imageUrlValidator;
        _imageUploadService = imageUploadService;
    }

    public async Task<int> GetCountAsync()
    {
        return await _productRepository.GetCountAsync();
    }

    public async Task<PagedList<ProductDto>> GetProductsAsync(GetProductsInput input)
    {
        var specification = new Specification<Product>(p =>
            (string.IsNullOrEmpty(input.Name) || p.Name.Contains(input.Name)) &&
            (!input.CategoryId.HasValue || p.CategoryId == input.CategoryId.Value) &&
            (!input.IsActive.HasValue || p.IsActive == input.IsActive.Value) &&
            (!input.MinPrice.HasValue || p.Price >= input.MinPrice.Value) &&
            (!input.MaxPrice.HasValue || p.Price <= input.MaxPrice.Value) &&
            (!input.StartDate.HasValue || p.CreatedAt.Date == input.StartDate.Value) &&
            (!input.EndDate.HasValue || p.CreatedAt.Date <= input.EndDate.Value)
        );

        specification.IncludeStrings.Add("Category");
        specification.IncludeStrings.Add("ProductImages");
        specification.IncludeStrings.Add("Tax");

        var allProducts = await _productRepository.GetAllWithSpecAsync(specification, true);

        IEnumerable<Product> sortedProducts = allProducts;
        if (input.SortBy.HasValue)
        {
            sortedProducts = input.SortBy switch
            {
                SortByEnum.Name => input.IsDescending
                                            ? allProducts.OrderByDescending(p => p.Name)
                                            : allProducts.OrderBy(p => p.Name),
                SortByEnum.Price => input.IsDescending
                                            ? allProducts.OrderByDescending(p => p.Price)
                                            : allProducts.OrderBy(p => p.Price),
                SortByEnum.CreatedDate => input.IsDescending
                                            ? allProducts.OrderByDescending(p => p.CreatedAt)
                                            : allProducts.OrderBy(p => p.CreatedAt),
                _ => allProducts.OrderByDescending(p => p.CreatedAt),
            };
        }
        else
        {
            sortedProducts = allProducts.OrderByDescending(p => p.CreatedAt);
        }

        var totalRecords = sortedProducts.Count();

        var pagedProducts = sortedProducts
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(pagedProducts);

        return new PagedList<ProductDto>
        {
            PageNumber = input.PageNumber,
            PageSize = input.PageSize,
            TotalRecords = totalRecords,
            Items = productDtos.ToList()
        };
    }

    public async Task<ProductDto?> GetProductByIdAsync(long id)
    {
        var product = await _productRepository.GetProductWithDetailsAsync(id);
        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    public async Task<ProductImageDto> GetThumbnailImageAsync(long productId)
    {
        var latestImage = await _unitOfWork.Repository<ProductImage>()
            .GetQueryable()
            .AsNoTracking()
            .Where(pi => pi.ProductId == productId)
            .OrderByDescending(pi => pi.Id)
            .FirstOrDefaultAsync();

        return _mapper.Map<ProductImageDto>(latestImage);

    }

    public async Task<int> GetAmount(GetProductsInput input)
    {
        var specification = new Specification<Product>(p =>
                        (string.IsNullOrEmpty(input.Name) || p.Name.Contains(input.Name)) &&
                        (!input.CategoryId.HasValue || p.CategoryId == input.CategoryId.Value) &&
                        (!input.IsActive.HasValue || p.IsActive == input.IsActive.Value) &&
                        (!input.MinPrice.HasValue || p.Price >= input.MinPrice.Value) &&
                        (!input.MaxPrice.HasValue || p.Price <= input.MaxPrice.Value) &&
                        (!input.StartDate.HasValue || p.CreatedAt.Date == input.StartDate.Value) &&
                        (!input.EndDate.HasValue || p.CreatedAt.Date <= input.EndDate.Value)
                    );
        var allProducts = await _productRepository.GetAllWithSpecAsync(specification, true);

        return allProducts.Count();
    }

    public async Task<Dictionary<long, int>> GetSoldQuantitiesAsync()
    {
        try
        {
            var orderDetails = await _unitOfWork.Repository<OrderDetail>()
                .GetQueryable()
                .AsNoTracking()
                .ToListAsync();

            return orderDetails
                .GroupBy(od => od.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(od => od.Quantity));
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error getting sold quantities");
            return [];
        }
    }

    public async Task<bool> IsProductNameExistsAsync(string name, long? excludeId = null)
    {
        return await _productRepository.IsProductNameExistsAsync(name, excludeId);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductInput input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var uploadedImageUrls = new List<string>();

            if (input.ImageUrls?.Any() == true)
            {
                foreach (var imageSource in input.ImageUrls.Where(u => !string.IsNullOrWhiteSpace(u)))
                {
                    try
                    {
                        if (await _imageUrlValidator.ValidateAsync(imageSource!))
                        {
                            var uploadedUrl = await _imageUploadService.UploadImageAsync(imageSource!);
                            uploadedImageUrls.Add(uploadedUrl);
                        }
                        else
                        {
                            _logger.LogWarning("Invalid or unsafe image URL: {Url}", imageSource);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to upload image: {ImageSource}", imageSource);
                    }
                }
            }

            var product = _mapper.Map<Product>(input);
            product.CreatedAt = DateTime.UtcNow;

            if (uploadedImageUrls.Count != 0)
            {
                product.ProductImages = [.. uploadedImageUrls
                    .Select(url => new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = url,
                    })];
            }

            await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDto>(product);
        });
    }


    public async Task<ProductDto> UpdateProductAsync(UpdateProductInput input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var product = await _productRepository.GetProductWithDetailsAsync(input.Id)
                ?? throw new ArgumentException($"Product with id {input.Id} not found");

            var existingUrls = product.ProductImages?
                .Select(pi => pi.ImageUrl?.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string?>(StringComparer.OrdinalIgnoreCase);

            var newUrlsToUpload = input.ImageUrls
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Select(u => u!.Trim())
                .Where(url => !existingUrls.Contains(url)) 
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList() ?? new List<string>();

            var uploadedUrls = new List<string>();
            foreach (var imageSource in newUrlsToUpload)
            {
                try
                {
                    if (await _imageUrlValidator.ValidateAsync(imageSource))
                    {
                        var uploadedUrl = await _imageUploadService.UploadImageAsync(imageSource);
                        uploadedUrls.Add(uploadedUrl);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to upload image during update: {ImageSource}", imageSource);
                }
            }

            if (uploadedUrls.Any())
            {
                product.ProductImages ??= [];
                foreach (var url in uploadedUrls)
                {
                    product.ProductImages.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = url,
                    });
                }
                _logger.LogInformation("Added {Count} new images to product {ProductId}", uploadedUrls.Count, product.Id);
            }

            var updateInputCopy = new UpdateProductInput(
                input.Id,
                input.Description,
                input.Price,
                input.CategoryId,
                input.IsActive,
                input.TaxId
            )
            {
                Name = input.Name
            };

            _mapper.Map(updateInputCopy, product);
            product.LastModified = DateTime.UtcNow;

            _productRepository.Update(product);
            return _mapper.Map<ProductDto>(product);
        });
    }


    public async Task<List<ProductImageDto>> GetProductImagesAsync(long id)
    {
        var productImages = await _unitOfWork.Repository<ProductImage>()
                .GetQueryable()
                .AsNoTracking()
                .Where(pi => pi.ProductId == id)
                .OrderByDescending(pi => pi.Id)
                .ToListAsync();

        return _mapper.Map<List<ProductImageDto>>(productImages);
    }

    public async Task<bool> DeleteProductAsync(long id)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return false;
            }
            _productRepository.Remove(product);
            return true;
        });
    }


    public async Task<bool> DeleteProductImageAsync(long productId, long imageId)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            try
            {
                var productImage = await _unitOfWork.Repository<ProductImage>().GetAsync(imageId);

                if (productImage == null || productImage.ProductId != productId)
                {
                    _logger?.LogWarning("ProductImage with ID {ImageId} and ProductId {ProductId} not found", imageId, productId);
                    return false;
                }

                var imageUrl = productImage.ImageUrl;

                _unitOfWork.Repository<ProductImage>().Remove(productImage);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    try
                    {
                        await _imageUploadService.DeleteImageAsync(imageUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Failed to delete physical image file: {ImageUrl}", imageUrl);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting product image {ImageId} for product {ProductId}", imageId, productId);
                throw;
            }
        });
    }

}