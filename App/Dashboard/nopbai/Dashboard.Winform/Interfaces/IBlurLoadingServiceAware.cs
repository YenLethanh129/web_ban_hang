namespace Dashboard.Winform.Interfaces
{
    /// <summary>
    /// Interface for forms that can receive and use IBlurLoadingService
    /// </summary>
    public interface IBlurLoadingServiceAware
    {
        /// <summary>
        /// Sets the blur loading service for the form to use
        /// </summary>
        /// <param name="blurLoadingService">The blur loading service instance</param>
        void SetBlurLoadingService(IBlurLoadingService blurLoadingService);
    }
}
