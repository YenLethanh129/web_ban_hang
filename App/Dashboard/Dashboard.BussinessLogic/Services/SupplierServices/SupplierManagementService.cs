using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Suppliers;

namespace Dashboard.BussinessLogic.Services.SupplierServices;

public interface ISupplierManagementService
{
    Task<PagedList<SupplierDto>> GetSuppliersAsync(GetSuppliersInput input);
    Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
    Task<SupplierDto?> GetSupplierByIdAsync(long id);
    Task<SupplierDto> CreateSupplierAsync(CreateSupplierInput input);
    Task<SupplierDto> UpdateSupplierAsync(UpdateSupplierInput input);
    Task<bool> DeleteSupplierAsync(long id);
    Task<bool> SupplierExistsAsync(string name, long? excludeId = null);
}

public class SupplierManagementService : BaseTransactionalService, ISupplierManagementService
{
    private readonly IMapper _mapper;

    public SupplierManagementService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<PagedList<SupplierDto>> GetSuppliersAsync(GetSuppliersInput input)
    {
        var specification = SupplierSpecifications.BySearchCriteria(input);
        
        if (input.HasActiveOrders == true)
        {
            // Need to filter suppliers with active orders
            var allSuppliers = await _unitOfWork.Repository<Supplier>()
                .GetAllWithSpecAsync(specification, true);

            var suppliersWithOrders = allSuppliers
                .Where(s => s.IngredientPurchaseOrders.Any())
                .ToList();

            var totalCount = suppliersWithOrders.Count();
            var pagedSuppliers = suppliersWithOrders
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            var supplierDtos = _mapper.Map<List<SupplierDto>>(pagedSuppliers);

            return new PagedList<SupplierDto>
            {
                Items = supplierDtos,
                TotalRecords = totalCount,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize
            };
        }
        else
        {
            // Get total count for pagination
            var allSuppliers = await _unitOfWork.Repository<Supplier>()
                .GetAllWithSpecAsync(specification, true);
            var totalCount = allSuppliers.Count();

            // Get paged data using skip and take parameters
            var pagedSuppliers = await _unitOfWork.Repository<Supplier>()
                .GetAllWithSpecAsync(specification, true,
                    skip: (input.PageNumber - 1) * input.PageSize,
                    take: input.PageSize);

            var supplierDtos = _mapper.Map<List<SupplierDto>>(pagedSuppliers);

            return new PagedList<SupplierDto>
            {
                Items = supplierDtos,
                TotalRecords = totalCount,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize
            };
        }
    }

    public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
    {
        var specification = SupplierSpecifications.WithIncludes();
        var suppliers = await _unitOfWork.Repository<Supplier>()
            .GetAllWithSpecAsync(specification, true);

        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }

    public async Task<SupplierDto?> GetSupplierByIdAsync(long id)
    {
        var specification = SupplierSpecifications.ById(id);
        var supplier = await _unitOfWork.Repository<Supplier>()
            .GetWithSpecAsync(specification);

        return supplier != null ? _mapper.Map<SupplierDto>(supplier) : null;
    }

    public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierInput input)
    {
        // Validate required fields
        if (string.IsNullOrEmpty(input.Name))
            throw new ArgumentException("Supplier name is required");

        // Check for duplicate name
        if (await SupplierExistsAsync(input.Name))
            throw new InvalidOperationException($"Supplier with name '{input.Name}' already exists");

        // Validate email format if provided
        if (!string.IsNullOrEmpty(input.Email) && !IsValidEmail(input.Email))
            throw new ArgumentException("Invalid email format");

        // Validate phone format if provided
        if (!string.IsNullOrEmpty(input.Phone) && !IsValidPhone(input.Phone))
            throw new ArgumentException("Invalid phone format");

        var supplier = _mapper.Map<Supplier>(input);
        await _unitOfWork.Repository<Supplier>().AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        return await GetSupplierByIdAsync(supplier.Id) 
            ?? throw new InvalidOperationException("Failed to retrieve created supplier");
    }

    public async Task<SupplierDto> UpdateSupplierAsync(UpdateSupplierInput input)
    {
        // Validate required fields
        if (string.IsNullOrEmpty(input.Name))
            throw new ArgumentException("Supplier name is required");

        var existingSupplier = await _unitOfWork.Repository<Supplier>().GetAsync(input.Id);
        if (existingSupplier == null)
            throw new InvalidOperationException($"Supplier with ID {input.Id} not found");

        // Check for duplicate name (excluding current supplier)
        if (await SupplierExistsAsync(input.Name, input.Id))
            throw new InvalidOperationException($"Supplier with name '{input.Name}' already exists");

        // Validate email format if provided
        if (!string.IsNullOrEmpty(input.Email) && !IsValidEmail(input.Email))
            throw new ArgumentException("Invalid email format");

        // Validate phone format if provided
        if (!string.IsNullOrEmpty(input.Phone) && !IsValidPhone(input.Phone))
            throw new ArgumentException("Invalid phone format");

        _mapper.Map(input, existingSupplier);
        _unitOfWork.Repository<Supplier>().Remove(existingSupplier);
        _unitOfWork.Repository<Supplier>().Add(existingSupplier);
        await _unitOfWork.SaveChangesAsync();

        return await GetSupplierByIdAsync(input.Id)
            ?? throw new InvalidOperationException("Failed to retrieve updated supplier");
    }

    public async Task<bool> DeleteSupplierAsync(long id)
    {
        var existingSupplier = await _unitOfWork.Repository<Supplier>().GetAsync(id);
        if (existingSupplier == null)
            throw new InvalidOperationException($"Supplier with ID {id} not found");

        // Check if supplier has any active orders or prices
        var specification = SupplierSpecifications.ById(id);
        var supplierWithRelations = await _unitOfWork.Repository<Supplier>()
            .GetWithSpecAsync(specification);

        if (supplierWithRelations?.IngredientPurchaseOrders.Any() == true)
            throw new InvalidOperationException("Cannot delete supplier with existing purchase orders");

        if (supplierWithRelations?.SupplierIngredientPrices.Any() == true)
            throw new InvalidOperationException("Cannot delete supplier with existing ingredient prices");

        _unitOfWork.Repository<Supplier>().Remove(existingSupplier);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SupplierExistsAsync(string name, long? excludeId = null)
    {
        var specification = SupplierSpecifications.ByName(name, excludeId);
        var existingSupplier = await _unitOfWork.Repository<Supplier>()
            .GetWithSpecAsync(specification);

        return existingSupplier != null;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhone(string phone)
    {
        // Simple phone validation - can be enhanced based on requirements
        return phone.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ' || c == '(' || c == ')') 
               && phone.Any(char.IsDigit);
    }
}
