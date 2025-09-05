using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Services;

public interface ISupplierPriceService
{
    Task<PagedList<SupplierIngredientPriceDto>> GetSupplierPricesAsync(GetSupplierPricesInput input);
    Task<IEnumerable<SupplierIngredientPriceDto>> GetPricesBySupplierAsync(long supplierId);
    Task<IEnumerable<SupplierIngredientPriceDto>> GetPricesByIngredientAsync(long ingredientId);
    Task<IEnumerable<SupplierIngredientPriceDto>> GetActivePricesAsync();
    Task<SupplierIngredientPriceDto?> GetSupplierPriceByIdAsync(long id);
    Task<SupplierIngredientPriceDto?> GetSupplierPriceAsync(long supplierId, long ingredientId);
    Task<SupplierIngredientPriceDto> CreateSupplierPriceAsync(CreateSupplierPriceInput input);
    Task<SupplierIngredientPriceDto> UpdateSupplierPriceAsync(UpdateSupplierPriceInput input);
    Task<bool> DeleteSupplierPriceAsync(long id);
    Task<IEnumerable<SupplierIngredientPriceDto>> GetBestPricesForIngredientAsync(long ingredientId);
}

public class SupplierPriceService : BaseTransactionalService, ISupplierPriceService
{
    private readonly IMapper _mapper;

    public SupplierPriceService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<PagedList<SupplierIngredientPriceDto>> GetSupplierPricesAsync(GetSupplierPricesInput input)
    {
        var specification = SupplierPriceSpecifications.BySearchCriteria(input);
        
        // Get total count for pagination
        var allPrices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true);
        var totalCount = allPrices.Count();
        
        // Get paged data using skip and take parameters
        var pagedPrices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true, 
                skip: (input.PageNumber - 1) * input.PageSize, 
                take: input.PageSize);

        var priceDtos = _mapper.Map<List<SupplierIngredientPriceDto>>(pagedPrices);

        return new PagedList<SupplierIngredientPriceDto>
        {
            Items = priceDtos,
            TotalRecords = totalCount,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<IEnumerable<SupplierIngredientPriceDto>> GetPricesBySupplierAsync(long supplierId)
    {
        var specification = SupplierPriceSpecifications.BySupplier(supplierId);
        var prices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierIngredientPriceDto>>(prices);
    }

    public async Task<IEnumerable<SupplierIngredientPriceDto>> GetPricesByIngredientAsync(long ingredientId)
    {
        var specification = SupplierPriceSpecifications.ByIngredient(ingredientId);
        var prices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierIngredientPriceDto>>(prices);
    }

    public async Task<IEnumerable<SupplierIngredientPriceDto>> GetActivePricesAsync()
    {
        var specification = SupplierPriceSpecifications.ActivePrices();
        var prices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierIngredientPriceDto>>(prices);
    }

    public async Task<SupplierIngredientPriceDto?> GetSupplierPriceByIdAsync(long id)
    {
        var specification = SupplierPriceSpecifications.WithIncludes();
        var price = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetWithSpecAsync(SupplierPriceSpecifications.ById(id));

        if (price != null)
        {
            // Manually load includes since we can't modify the specification after creation
            var priceWithIncludes = await _unitOfWork.Repository<SupplierIngredientPrice>()
                .GetAllWithSpecAsync(specification, true);
            
            price = priceWithIncludes.FirstOrDefault(p => p.Id == id);
        }

        return price != null ? _mapper.Map<SupplierIngredientPriceDto>(price) : null;
    }

    public async Task<SupplierIngredientPriceDto?> GetSupplierPriceAsync(long supplierId, long ingredientId)
    {
        var specification = SupplierPriceSpecifications.BySupplierAndIngredient(supplierId, ingredientId);
        var price = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetWithSpecAsync(specification);

        return price != null ? _mapper.Map<SupplierIngredientPriceDto>(price) : null;
    }

    public async Task<SupplierIngredientPriceDto> CreateSupplierPriceAsync(CreateSupplierPriceInput input)
    {
        // Validate required fields
        if (input.Price <= 0)
            throw new ArgumentException("Price must be greater than zero");

        if (string.IsNullOrEmpty(input.Unit))
            throw new ArgumentException("Unit is required");

        // Validate dates
        if (input.EffectiveDate.HasValue && input.ExpiredDate.HasValue && 
            input.EffectiveDate >= input.ExpiredDate)
            throw new ArgumentException("Effective date must be before expired date");

        // Validate supplier exists
        await ValidateSupplierAsync(input.SupplierId);
        await ValidateIngredientAsync(input.IngredientId);

        // Check for existing active price for same supplier-ingredient combination
        var existingSpec = SupplierPriceSpecifications.BySupplierAndIngredient(input.SupplierId, input.IngredientId);
        var existingPrices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(existingSpec, true);

        var now = DateTime.Now;
        var activeExistingPrice = existingPrices.FirstOrDefault(p =>
            (p.EffectiveDate == null || p.EffectiveDate <= now) &&
            (p.ExpiredDate == null || p.ExpiredDate > now));

        if (activeExistingPrice != null)
        {
            throw new InvalidOperationException(
                $"An active price already exists for supplier {input.SupplierId} and ingredient {input.IngredientId}. " +
                "Please expire the existing price before creating a new one.");
        }

        var price = _mapper.Map<SupplierIngredientPrice>(input);
        await _unitOfWork.Repository<SupplierIngredientPrice>().AddAsync(price);
        await _unitOfWork.SaveChangesAsync();

        return await GetSupplierPriceByIdAsync(price.Id)
            ?? throw new InvalidOperationException("Failed to retrieve created price");
    }

    public async Task<SupplierIngredientPriceDto> UpdateSupplierPriceAsync(UpdateSupplierPriceInput input)
    {
        // Validate required fields
        if (input.Price <= 0)
            throw new ArgumentException("Price must be greater than zero");

        if (string.IsNullOrEmpty(input.Unit))
            throw new ArgumentException("Unit is required");

        // Validate dates
        if (input.EffectiveDate.HasValue && input.ExpiredDate.HasValue && 
            input.EffectiveDate >= input.ExpiredDate)
            throw new ArgumentException("Effective date must be before expired date");

        var existingPrice = await _unitOfWork.Repository<SupplierIngredientPrice>().GetAsync(input.Id);
        if (existingPrice == null)
            throw new InvalidOperationException($"Supplier price with ID {input.Id} not found");

        _mapper.Map(input, existingPrice);
        _unitOfWork.Repository<SupplierIngredientPrice>().Remove(existingPrice);
        _unitOfWork.Repository<SupplierIngredientPrice>().Add(existingPrice);
        await _unitOfWork.SaveChangesAsync();

        return await GetSupplierPriceByIdAsync(input.Id)
            ?? throw new InvalidOperationException("Failed to retrieve updated price");
    }

    public async Task<bool> DeleteSupplierPriceAsync(long id)
    {
        var existingPrice = await _unitOfWork.Repository<SupplierIngredientPrice>().GetAsync(id);
        if (existingPrice == null)
            throw new InvalidOperationException($"Supplier price with ID {id} not found");

        _unitOfWork.Repository<SupplierIngredientPrice>().Remove(existingPrice);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<SupplierIngredientPriceDto>> GetBestPricesForIngredientAsync(long ingredientId)
    {
        var specification = SupplierPriceSpecifications.ByIngredient(ingredientId);
        var allPrices = await _unitOfWork.Repository<SupplierIngredientPrice>()
            .GetAllWithSpecAsync(specification, true);

        var now = DateTime.Now;
        var activePrices = allPrices
            .Where(p => (p.EffectiveDate == null || p.EffectiveDate <= now) &&
                       (p.ExpiredDate == null || p.ExpiredDate > now))
            .OrderBy(p => p.Price)
            .Take(5) // Top 5 best prices
            .ToList();

        return _mapper.Map<IEnumerable<SupplierIngredientPriceDto>>(activePrices);
    }

    private async Task<bool> ValidateSupplierAsync(long supplierId)
    {
        var supplier = await _unitOfWork.Repository<Supplier>().GetAsync(supplierId);
        if (supplier == null)
            throw new InvalidOperationException($"Supplier with ID {supplierId} not found");

        return true;
    }

    private async Task<bool> ValidateIngredientAsync(long ingredientId)
    {
        var ingredient = await _unitOfWork.Repository<Ingredient>().GetAsync(ingredientId);
        if (ingredient == null)
            throw new InvalidOperationException($"Ingredient with ID {ingredientId} not found");

        return true;
    }
}
