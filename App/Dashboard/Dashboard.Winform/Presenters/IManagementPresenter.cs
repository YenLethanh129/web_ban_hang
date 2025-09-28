using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Presenters;

public interface IManagementPresenter <TModel> where TModel : IManagableModel
{
    IManagableModel Model { get; set; }
    event EventHandler? OnDataLoaded;
    Task LoadDataAsync();
}