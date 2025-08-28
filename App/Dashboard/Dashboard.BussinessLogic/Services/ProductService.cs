using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.Extensions.Logging;

namespace Dashboard.BussinessLogic.Services;

public interface IProductService 
{
    Task<PagedList<ProductDto>> GetProductsAsync(GetProductsInput input);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductInput input);
    Task<ProductDto> UpdateProductAsync(UpdateProductInput input);
    Task<bool> DeleteProductAsync(int id);
    Task<bool> IsProductNameExistsAsync(string name, int? excludeId = null);
}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<ProductDto>> GetProductsAsync(GetProductsInput input)
    {
        try
        {
            var specification = new Specification<Product>(p =>
                (string.IsNullOrEmpty(input.Name) || p.Name.Contains(input.Name)) &&
                (!input.CategoryId.HasValue || p.CategoryId == input.CategoryId.Value) &&
                //(!input.IsActive.HasValue || p.IsActive == input.IsActive.Value) &&
                (!input.MinPrice.HasValue || p.Price >= input.MinPrice.Value) &&
                (!input.MaxPrice.HasValue || p.Price <= input.MaxPrice.Value)
            );

            specification.Includes.Add(p => p.Category!);
            specification.Includes.Add(p => p.ProductImages);

            var allProducts = await _productRepository.GetAllWithSpecAsync(specification);

            IEnumerable<Product> sortedProducts = allProducts;
            if (!string.IsNullOrEmpty(input.SortBy))
            {
                switch (input.SortBy.ToLower())
                {
                    case "name":
                        sortedProducts = input.IsDescending
                            ? allProducts.OrderByDescending(p => p.Name)
                            : allProducts.OrderBy(p => p.Name);
                        break;
                    case "price":
                        sortedProducts = input.IsDescending
                            ? allProducts.OrderByDescending(p => p.Price)
                            : allProducts.OrderBy(p => p.Price);
                        break;
                    case "createddate":
                        sortedProducts = input.IsDescending
                            ? allProducts.OrderByDescending(p => p.CreatedAt)
                            : allProducts.OrderBy(p => p.CreatedAt);
                        break;
                    default:
                        sortedProducts = allProducts.OrderByDescending(p => p.CreatedAt);
                        break;
                }
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

            // Map sang DTO
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(pagedProducts);

            return new PagedList<ProductDto>
            {
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalRecords = totalRecords,
                Items = [.. productDtos]
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting products");
            throw;
        }
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetProductWithDetailsAsync(id);
            return product != null ? _mapper.Map<ProductDto>(product) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting product by id: {Id}", id);
            throw;
        }
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductInput input)
    {
        try
        {
            var product = _mapper.Map<Product>(input);
            product.CreatedAt = DateTime.UtcNow;

            // Add product images
            if (input.ImageUrls.Any())
            {
                product.ProductImages = [.. input.ImageUrls.Select((url) => new ProductImage
                {
                    ImageUrl = url,
                })];
            }
             
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating product");
            throw;
        }
    }

    public async Task<ProductDto> UpdateProductAsync(UpdateProductInput input)
    {
        try
        {
            var product = await _productRepository.GetProductWithDetailsAsync(input.Id);
            if (product == null)
            {
                throw new ArgumentException($"Product with id {input.Id} not found");
            }

            _mapper.Map(input, product);
            product.LastModified = DateTime.UtcNow;

            product.ProductImages.Clear();
            if (input.ImageUrls.Any())
            {
                product.ProductImages = input.ImageUrls.Select((url) => new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = url,
                }).ToList();
            }

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating product");
            throw;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return false;
            }

            _productRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting product");
            throw;
        }
    }

    public async Task<bool> IsProductNameExistsAsync(string name, int? excludeId = null)
    {
        try
        {
            return await _productRepository.IsProductNameExistsAsync(name, excludeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking product name exists");
            throw;
        }
    }
}