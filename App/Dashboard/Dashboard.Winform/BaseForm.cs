using Dashboard.BussinessLogic.Shared;
using Microsoft.Extensions.Logging;

namespace Dashboard.Winform;

public partial class BaseForm : Form
{
    private readonly ILogger _logger;

    public BaseForm(ILogger logger)
    {
        InitializeComponent();
        _logger = logger;
    }

    protected void ExecuteWithHandling(Action action, Action? finallyAction = null)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            var result = ExceptionHandler.Handle(ex, _logger);
            MessageBox.Show($"Error: {result.Title}\n{result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            finallyAction?.Invoke();
        }
    }

    protected async Task ExecuteWithHandlingAsync(Func<Task> asyncAction, Action? finallyAction = null)
    {
        try
        {
            await asyncAction();
        }
        catch (Exception ex)
        {
            var result = ExceptionHandler.Handle(ex, _logger);
            MessageBox.Show($"Error: {result.Title}\n{result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            finallyAction?.Invoke();
        }
    }
}
