using Dashboard.BussinessLogic.Shared;
using Dashboard.Winform.Controls;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Dashboard.Winform;

public partial class BaseForm : Form
{
    private readonly ILogger? _logger;
    private BlurLoadingOverlay? _blurLoadingOverlay;
    private Stopwatch? _loadingStopwatch;

    public BaseForm()
    {
        InitializeComponent();
        EnableDoubleBuffering();
    }

    public BaseForm(ILogger logger) : this()
    {
        _logger = logger;
    }

    private void EnableDoubleBuffering()
    {
        // Enable double buffering để giảm flickering
        SetStyle(ControlStyles.AllPaintingInWmPaint | 
                 ControlStyles.UserPaint | 
                 ControlStyles.DoubleBuffer | 
                 ControlStyles.ResizeRedraw, true);
        
        UpdateStyles();
    }

    protected async Task ExecuteWithLoadingAsync(Func<Task> asyncAction, string loadingMessage = "Đang tải...", bool useFadeEffect = false)
    {
        try
        {
            _loadingStopwatch = Stopwatch.StartNew();
            _logger?.LogInformation("Starting blur loading overlay");

            if (InvokeRequired)
            {
                await Task.Run(() => Invoke(new Action(async () => await ShowBlurLoadingAsync(loadingMessage, useFadeEffect))));
            }
            else
            {
                await ShowBlurLoadingAsync(loadingMessage, useFadeEffect);
            }

            await asyncAction();

            if (_loadingStopwatch != null)
            {
                _loadingStopwatch.Stop();
                _loadingStopwatch = null;
            }

            if (InvokeRequired)
            {
                await Task.Run(() => Invoke(new Action(async () => await HideBlurLoadingAsync(useFadeEffect))));
            }
            else
            {
                await HideBlurLoadingAsync(useFadeEffect);
            }

            _logger?.LogInformation("Loading completed successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error in ExecuteWithLoadingAsync\n {ex.Message}");

            if (InvokeRequired)
            {
                Invoke(new Action(() => HideBlurLoading()));
            }
            else
            {
                HideBlurLoading();
            }

            MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task ShowBlurLoadingAsync(string message, bool useFadeEffect = false)
    {
        try
        {
            if (_blurLoadingOverlay == null)
            {
                _blurLoadingOverlay = new BlurLoadingOverlay();
            }
            
            if (useFadeEffect)
            {
                await _blurLoadingOverlay.ShowLoadingWithFadeAsync(this, message);
            }
            else
            {
                _blurLoadingOverlay.ShowLoading(this, message);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to show blur loading overlay, falling back to simple message");
            Cursor = Cursors.WaitCursor;
        }
    }

    private async Task HideBlurLoadingAsync(bool useFadeEffect = false)
    {
        try
        {
            if (_blurLoadingOverlay != null)
            {
                if (useFadeEffect)
                {
                    await _blurLoadingOverlay.HideLoadingWithFadeAsync();
                }
                else
                {
                    _blurLoadingOverlay.HideLoading();
                }
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error hiding blur loading overlay");
        }
        finally
        {
            // Always restore cursor
            Cursor = Cursors.Default;
            Activate();
            Focus();
        }
    }

    private void HideBlurLoading()
    {
        try
        {
            if (_blurLoadingOverlay != null)
            {
                _blurLoadingOverlay.HideLoading();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error hiding blur loading overlay");
        }
        finally
        {
            // Always restore cursor
            Cursor = Cursors.Default;
            Activate();
            Focus();
        }
    }


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _blurLoadingOverlay?.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }
}
