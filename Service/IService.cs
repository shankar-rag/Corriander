namespace TAG.Core.Services
{
    /// <summary>
    /// Defines necessary functionality for a service.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Initializes the service.
        /// </summary>
        public void Init();
        
        /// <summary>
        /// Destroys the service.
        /// </summary>
        public void Destroy();
    }
}