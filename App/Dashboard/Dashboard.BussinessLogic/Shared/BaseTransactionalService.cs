using Dashboard.DataAccess.Data;

namespace Dashboard.BussinessLogic.Shared;

public abstract class BaseTransactionalService
{
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseTransactionalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await action();
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    protected async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var result = await action();
            await _unitOfWork.CommitAsync();
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
