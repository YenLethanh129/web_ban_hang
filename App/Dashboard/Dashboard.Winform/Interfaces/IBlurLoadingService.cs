using System;
using System.Threading.Tasks;

namespace Dashboard.Winform.Interfaces
{
    /// <summary>
    /// Interface for centralized blur loading management
    /// Allows child forms to request blur loading from parent container
    /// </summary>
    public interface IBlurLoadingService
    {
        /// <summary>
        /// Execute an async action with blur loading overlay
        /// </summary>
        /// <param name="asyncAction">The async action to execute</param>
        /// <param name="loadingMessage">Message to display during loading</param>
        /// <param name="useFadeEffect">Whether to use fade effect</param>
        /// <returns>Task representing the operation</returns>
        Task ExecuteWithLoadingAsync(Func<Task> asyncAction, string loadingMessage = "Đang tải...", bool useFadeEffect = false);

        /// <summary>
        /// Execute an async function with blur loading overlay and return result
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="asyncFunction">The async function to execute</param>
        /// <param name="loadingMessage">Message to display during loading</param>
        /// <param name="useFadeEffect">Whether to use fade effect</param>
        /// <returns>Task with result</returns>
        Task<T> ExecuteWithLoadingAsync<T>(Func<Task<T>> asyncFunction, string loadingMessage = "Đang tải...", bool useFadeEffect = false);

        /// <summary>
        /// Shows blur loading overlay manually
        /// </summary>
        /// <param name="message">Loading message</param>
        /// <param name="useFadeEffect">Whether to use fade effect</param>
        Task ShowLoadingAsync(string message = "Đang tải...", bool useFadeEffect = false);

        /// <summary>
        /// Hides blur loading overlay manually
        /// </summary>
        /// <param name="useFadeEffect">Whether to use fade effect</param>
        Task HideLoadingAsync(bool useFadeEffect = false);

        /// <summary>
        /// Indicates whether loading is currently active
        /// </summary>
        bool IsLoading { get; }
    }
}
