using AutoMapper;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;

namespace Dashboard.Winform.Presenters;

public interface IGoodManagementPresenter : IManagementPresenter<GoodsManagementModel>
{
    Task LoadDataAsync(long? categoryId = null, string? searchTerm = null,
        int? pageSize = 10, int? page = 1, bool forceRefresh = false);
    Task SearchAsync(string searchTerm);
    Task FilterByStatusAsync(string status);
    Task SortBy(string? sortBy);
    Task GoToNextPageAsync();
    Task GoToPreviousPageAsync();
    Task ChangePageSizeAsync(int pageSize);
    Task RefreshCacheAsync();
}


public class GoodManagementPresenter : IGoodManagementPresenter
{
    private readonly ILogger<GoodManagementPresenter> _logger;
    private readonly IMapper _mapper;

    public GoodsManagementModel Model { get; }
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    //private bool _isLoading = false;

    IManagableModel IManagementPresenter<GoodsManagementModel>.Model { get => Model; set => throw new NotImplementedException(); }

    public event EventHandler? OnDataLoaded;

    public GoodManagementPresenter(ILogger<GoodManagementPresenter> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        Model = new GoodsManagementModel();
    }

    public Task ChangePageSizeAsync(int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task FilterByStatusAsync(string status)
    {
        OnDataLoaded?.Invoke(this, new ProductsLoadedEventArgs());
        throw new NotImplementedException();
    }

    public Task GoToNextPageAsync()
    {
        throw new NotImplementedException();
    }

    public Task GoToPreviousPageAsync()
    {
        throw new NotImplementedException();
    }

    public Task LoadDataAsync(long? categoryId = null, string? searchTerm = null, int? pageSize = 10, int? page = 1, bool forceRefresh = false)
    {
        throw new NotImplementedException();
    }

    public Task LoadDataAsync()
    {
        throw new NotImplementedException();
    }

    public Task RefreshCacheAsync()
    {
        throw new NotImplementedException();
    }

    public Task SearchAsync(string searchTerm)
    {
        throw new NotImplementedException();
    }

    public Task SortBy(string? sortBy)
    {
        throw new NotImplementedException();
    }
}
