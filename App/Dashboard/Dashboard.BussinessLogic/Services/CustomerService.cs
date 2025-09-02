using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.CustomerDtos;
using Dashboard.Common.Enums;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;

namespace Dashboard.BussinessLogic.Services;

public interface ICustomerService
{
    Task<PagedList<CustomerDto>> GetCustomersAsync(GetCustomersInput input);
    Task<CustomerDto?> GetCustomerByIdAsync(long id);
    Task<CustomerDto?> GetCustomerByEmailAsync(string findStr);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerInput input);
    Task<CustomerDto> UpdateCustomerAsync(long id, UpdateCustomerInput input);
    Task<bool> CustomerExistsAsync(string email, string phoneNumber, long? excludeId = null);
    Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int count = 10, DateTime? fromDate = null, DateTime? toDate = null);
    Task<CustomerSummaryDto> GetCustomerSummaryAsync(DateTime fromDate, DateTime toDate);
    Task DisableCustomerAsync(long id);
}

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public CustomerService(
        IUnitOfWork unitOfWork,
        ICustomerRepository customerRepository,
        IMapper mapper,
        IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _orderRepository = orderRepository;
    }

    public async Task<PagedList<CustomerDto>> GetCustomersAsync(GetCustomersInput input)
    {
        var specification = new Specification<Customer>(c =>
            (string.IsNullOrEmpty(input.SearchTerm) ||
             c.Fullname.Contains(input.SearchTerm) ||
            (string.IsNullOrEmpty(input.Email) || c.Email!.Contains(input.Email)) &&
            (string.IsNullOrEmpty(input.PhoneNumber) || c.PhoneNumber!.Contains(input.PhoneNumber)) &&
            (!input.IsActive.HasValue || c.IsActive() == input.IsActive) &&
            (!input.FromDate.HasValue || c.CreatedAt >= input.FromDate.Value) &&
            (!input.ToDate.HasValue || c.CreatedAt.Date <= input.ToDate.Value))
        );

        specification.Includes.Add(c => c.Orders!);

        var allCustomers = await _customerRepository.GetAllWithSpecAsync(specification, true);

        if (input.MinSpent.HasValue || input.MaxSpent.HasValue)
        {
            allCustomers = allCustomers.Where(c =>
            {
                var totalSpent = c.Orders?.Sum(o => o.TotalMoney ?? 0) ?? 0;
                return (!input.MinSpent.HasValue || totalSpent >= input.MinSpent.Value) &&
                       (!input.MaxSpent.HasValue || totalSpent <= input.MaxSpent.Value);
            });
        }

        var totalCount = allCustomers.Count();

        IEnumerable<Customer> sortedCustomers = allCustomers;
        if (input.SortBy.HasValue)
        {
            sortedCustomers = input.SortBy switch
            {
                SortByEnum.Name => input.IsDescending
                    ? allCustomers.OrderByDescending(c => c.Fullname)
                    : allCustomers.OrderBy(c => c.Fullname),
                SortByEnum.Email => input.IsDescending
                    ? allCustomers.OrderByDescending(c => c.Email)
                    : allCustomers.OrderBy(c => c.Email),
                SortByEnum.CreatedDate => input.IsDescending
                    ? allCustomers.OrderByDescending(c => c.CreatedAt)
                    : allCustomers.OrderBy(c => c.CreatedAt),
                _ => allCustomers.OrderByDescending(c => c.CreatedAt),
            };
        }
        else
        {
            sortedCustomers = allCustomers.OrderByDescending(c => c.CreatedAt);
        }

        var pagedCustomers = sortedCustomers
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var customerDtos = _mapper.Map<List<CustomerDto>>(pagedCustomers);

        return new PagedList<CustomerDto>
        {
            Items = customerDtos,
            TotalRecords = totalCount,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(long id)
    {
        var specification = new Specification<Customer>(c => c.Id == id);
        specification.Includes.Add(c => c.Orders!);

        var customer = await _customerRepository.GetWithSpecAsync(specification);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto?> GetCustomerByEmailAsync(string findStr)
    {
        var specification = new Specification<Customer>(c => findStr == c.PhoneNumber || findStr == c.Email);
        specification.Includes.Add(c => c.Orders!);

        var customer = await _customerRepository.GetWithSpecAsync(specification);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerInput input)
    {
        var existingCustomer = await CustomerExistsAsync(input.Email, input.PhoneNumber);
        if (existingCustomer)
        {
            throw new ArgumentException("Customer with this email or phone number already exists");
        }

        var customer = _mapper.Map<Customer>(input);
        customer.CreatedAt = DateTime.Now;

        var createdCustomer = await _customerRepository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(createdCustomer);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(long id, UpdateCustomerInput input)
    {
        var customer = await _customerRepository.GetAsync(id)
            ?? throw new ArgumentException($"Customer with id {id} not found");

        if (customer.Email != input.Email || customer.PhoneNumber != input.PhoneNumber)
        {
            var existingCustomer = await CustomerExistsAsync(input.Email, input.PhoneNumber, id);
            if (existingCustomer)
            {
                throw new ArgumentException("Customer with this email or phone number already exists");
            }
        }

        _mapper.Map(input, customer);
        customer.LastModified = DateTime.Now;

        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task DisableCustomerAsync(long id)
    {
        var customer = await _customerRepository.GetAsync(id)
            ?? throw new ArgumentException($"Customer with id {id} not found");
        if (customer.IsActive())
        {
            customer.InverseActiveStatus();
            await _unitOfWork.SaveChangesAsync();
        }        
    }


    public async Task<bool> CustomerExistsAsync(string email, string phoneNumber, long? excludeId = null)
    {
        return await  _customerRepository.AnyAsnc(o =>
            (o.Email == email || o.PhoneNumber == phoneNumber) &&
            (!excludeId.HasValue || o.Id != excludeId.Value)) != null;
    }
    public async Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int count = 10, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var specification = new Specification<Customer>(c => true);
        specification.Includes.Add(c => c.Orders!);

        var customersWithOrders = await _customerRepository.GetAllWithSpecAsync(specification);

        var topCustomers = customersWithOrders
            .Select(c => new
            {
                Customer = c,
                RelevantOrders = c.Orders!.Where(o =>
                    (!fromDate.HasValue || o.CreatedAt >= fromDate.Value) &&
                    (!toDate.HasValue || o.CreatedAt <= toDate.Value))
            })
            .Where(x => x.RelevantOrders.Any())
            .Select(x => new TopCustomerDto
            {
                Id = x.Customer.Id,
                FullName = x.Customer.Fullname,
                Email = x.Customer.Email ?? "N/A",
                PhoneNumber = x.Customer.PhoneNumber ?? "N/A",
                TotalOrders = x.RelevantOrders.Count(),
                TotalSpent = x.RelevantOrders.Sum(o => o.TotalMoney ?? 0),
                LastOrderDate = x.RelevantOrders.Max(o => o.CreatedAt)
            })
            .OrderByDescending(c => c.TotalSpent)
            .Take(count)
            .ToList();

        return topCustomers;
    }

    public async Task<CustomerSummaryDto> GetCustomerSummaryAsync(DateTime fromDate, DateTime toDate)
    {
        var allCustomers = await _customerRepository.GetAllAsync();
        var totalCustomers = allCustomers.Count();
        var activeCustomers = allCustomers.Count(c => c.IsActive());
        var inactiveCustomers = totalCustomers - activeCustomers;

        var newCustomersThisMonth = allCustomers.Count(c =>
            c.CreatedAt >= fromDate && c.CreatedAt <= toDate);

        // Get customers with orders for more detailed statistics
        var specification = new Specification<Customer>(c => true);
        specification.Includes.Add(c => c.Orders!);
        var customersWithOrders = await _customerRepository.GetAllWithSpecAsync(specification);

        var totalCustomerSpending = customersWithOrders
            .SelectMany(c => c.Orders!)
            .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
            .Sum(o => o.TotalMoney ?? 0);

        var totalOrdersInPeriod = customersWithOrders
            .SelectMany(c => c.Orders!)
            .Count(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate);

        var averageOrderValue = totalOrdersInPeriod > 0 ? totalCustomerSpending / totalOrdersInPeriod : 0;

        var topCustomers = await GetTopCustomersAsync(10, fromDate, toDate);

        return new CustomerSummaryDto
        {
            TotalCustomers = totalCustomers,
            ActiveCustomers = activeCustomers,
            InactiveCustomers = inactiveCustomers,
            NewCustomersThisMonth = newCustomersThisMonth,
            AverageOrderValue = averageOrderValue,
            TotalCustomerSpending = totalCustomerSpending,
            TopCustomers = topCustomers
        };
    }


    public Task DeleteCustomerAsync(long id)
    {
        throw new NotImplementedException();
    }
}