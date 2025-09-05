using AutoMapper;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ExpenseDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;

namespace Dashboard.BussinessLogic.Services;

public interface IExpenseService
{
    Task<PagedList<ExpenseDto>> GetExpensesAsync(GetExpensesInput input);
    Task<ExpenseDto> GetExpenseByIdAsync(long id);
    Task<ExpenseDto> CreateExpenseAsync(CreateExpenseInput input);
    Task<ExpenseDto> UpdateExpenseAsync(long id, UpdateExpenseInput input);
    Task DeleteExpenseAsync(long id);
    Task<IEnumerable<ExpenseDto>> GetExpensesByBranchAsync(long branchId, DateOnly? fromDate = null, DateOnly? toDate = null);
    Task<IEnumerable<ExpenseDto>> GetExpensesByTypeAsync(string expenseType, DateOnly? fromDate = null, DateOnly? toDate = null);
    Task<decimal> GetTotalExpensesAsync(long? branchId = null, DateOnly? fromDate = null, DateOnly? toDate = null);
    Task<IEnumerable<ExpenseSummaryDto>> GetExpensesSummaryAsync(DateOnly fromDate, DateOnly toDate, long? branchId = null);
}

public class ExpenseService : BaseTransactionalService, IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;

    public ExpenseService(IUnitOfWork unitOfWork, IExpenseRepository expenseRepository, IMapper mapper) 
        : base(unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<ExpenseDto>> GetExpensesAsync(GetExpensesInput input)
    {
        var specification = ExpenseSpecifications.WithAdvancedFilter(input);
        var expenses = await _unitOfWork.Repository<BranchExpense>().GetAllWithSpecAsync(specification, true);
        
        var sortedExpenses = expenses.OrderByDescending(e => e.StartDate);
        var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(sortedExpenses);
        
        return new PagedList<ExpenseDto>
        {
            Items = expenseDtos.Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize).ToList(),
            TotalRecords = expenseDtos.Count(),
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<ExpenseDto> GetExpenseByIdAsync(long id)
    {
        var specification = new Specification<BranchExpense>(e => e.Id == id);
        specification.IncludeStrings.Add("Branch");
        
        var expense = await _unitOfWork.Repository<BranchExpense>().GetWithSpecAsync(specification);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with ID {id} not found");

        return _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseInput input)
    {
        var expense = _mapper.Map<BranchExpense>(input);
        
        await _unitOfWork.Repository<BranchExpense>().AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        return await GetExpenseByIdAsync(expense.Id);
    }

    public async Task<ExpenseDto> UpdateExpenseAsync(long id, UpdateExpenseInput input)
    {
        var expense = await _unitOfWork.Repository<BranchExpense>().GetAsync(id);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with ID {id} not found");

        _mapper.Map(input, expense);
        
        _unitOfWork.Repository<BranchExpense>().Remove(expense);
        _unitOfWork.Repository<BranchExpense>().Add(expense);
        await _unitOfWork.SaveChangesAsync();

        return await GetExpenseByIdAsync(id);
    }

    public async Task DeleteExpenseAsync(long id)
    {
        var expense = await _unitOfWork.Repository<BranchExpense>().GetAsync(id);
        if (expense == null)
            throw new KeyNotFoundException($"Expense with ID {id} not found");

        _unitOfWork.Repository<BranchExpense>().Remove(expense);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ExpenseDto>> GetExpensesByBranchAsync(long branchId, DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        var specification = ExpenseSpecifications.ByBranch(branchId, fromDate, toDate);
        var expenses = await _unitOfWork.Repository<BranchExpense>().GetAllWithSpecAsync(specification, true);
        
        var sortedExpenses = expenses.OrderByDescending(e => e.StartDate);
        return _mapper.Map<IEnumerable<ExpenseDto>>(sortedExpenses);
    }

    public async Task<IEnumerable<ExpenseDto>> GetExpensesByTypeAsync(string expenseType, DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        var specification = ExpenseSpecifications.ByExpenseType(expenseType, fromDate, toDate);
        var expenses = await _unitOfWork.Repository<BranchExpense>().GetAllWithSpecAsync(specification, true);
        
        var sortedExpenses = expenses.OrderByDescending(e => e.StartDate);
        return _mapper.Map<IEnumerable<ExpenseDto>>(sortedExpenses);
    }

    public async Task<decimal> GetTotalExpensesAsync(long? branchId = null, DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        var specification = ExpenseSpecifications.ForTotalCalculation(branchId, fromDate, toDate);
        var expenses = await _unitOfWork.Repository<BranchExpense>().GetAllWithSpecAsync(specification, true);
        
        return expenses.Sum(e => e.Amount);
    }

    public async Task<IEnumerable<ExpenseSummaryDto>> GetExpensesSummaryAsync(DateOnly fromDate, DateOnly toDate, long? branchId = null)
    {
        var summaries = await _expenseRepository.GetExpensesSummaryAsync(fromDate, toDate, branchId);
        return _mapper.Map<IEnumerable<ExpenseSummaryDto>>(summaries);
    }
}
