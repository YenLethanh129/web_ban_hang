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

}

public class ProductService : BaseTransactionalService, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly IImageUrlValidator _imageUrlValidator;

    public ProductService(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ProductService> logger,
        IImageUrlValidator imageUrlValidator) : base(unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _imageUrlValidator = imageUrlValidator;
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
        return _mapper.Map<ProductImageDto>(await _productRepository.GetLastestImageAsync(productId));
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
        var orderDetails = await _unitOfWork.Repository<OrderDetail>().GetAllAsync();
        return orderDetails
            .GroupBy(od => od.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(od => od.Quantity));
    }

    public async Task<bool> IsProductNameExistsAsync(string name, long? excludeId = null)
    {
        return await _productRepository.IsProductNameExistsAsync(name, excludeId);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductInput input)
    {
        return await ExecuteInTransactionAsync(async () =>
        {
            // Validate and filter image URLs
            var validUrls = new List<string>();
            if (input.ImageUrls?.Any() == true)
            {
                foreach (var url in input.ImageUrls.Where(u => !string.IsNullOrWhiteSpace(u)))
                {
                    if (await _imageUrlValidator.ValidateAsync(url!))
                    {
                        validUrls.Add(url!);
                    }
                    else
                    {
                        _logger.LogWarning("Invalid or unsafe image URL: {Url}", url);
                    }
                }
            }

            var product = _mapper.Map<Product>(input);
            product.CreatedAt = DateTime.UtcNow;

            // Only add images if we have valid URLs
            if (validUrls.Any())
            {
                product.ProductImages = validUrls
                    .Select(url => new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = url,
                    })
                    .ToList();
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

            if (input.ImageUrls != null && input.ImageUrls.Any(u => !string.IsNullOrWhiteSpace(u)))
            {
                var newUrls = input.ImageUrls
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .Select(u => u!.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var existingUrls = product.ProductImages?
                    .Select(pi => pi.ImageUrl?.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase)
                    ?? new HashSet<string?>(StringComparer.OrdinalIgnoreCase);

                var toAdd = newUrls.Where(u => !existingUrls.Contains(u)).ToList();

                if (toAdd.Count != 0)
                {
                    product.ProductImages ??= [];

                    foreach (var url in toAdd)
                    {
                        product.ProductImages.Add(new ProductImage
                        {
                            ProductId = product.Id,
                            ImageUrl = url,
                        });
                    }
                }
                else
                {
                    _logger.LogInformation("No new product images to add for product {ProductId}", product.Id);
                }
            }
            else
            {
                _logger.LogDebug("No image URLs provided in update input for product {ProductId}; skipping image changes.", product.Id);
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
            .Where(pi => pi.ProductId == id).ToListAsync();

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
}