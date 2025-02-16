namespace TAG.Core.Services
{
    using System.Collections.Generic;
    using TAG.Core.Pooling;
    using UnityEngine;

    /// <summary>
    /// Initializes, manages and provides access to all services.
    /// </summary>
    public class ServicesManager : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Object pooling service.
        /// </summary>
        public static ObjectPoolingService ObjectPoolingService { get; private set; }
        #endregion

        private List<IService> _services = default;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            InitServices();
        }

        private void InitServices()
        {
            // Define and add all services to the master list
            ObjectPoolingService = new ObjectPoolingService();

            _services = new List<IService>
            {
                ObjectPoolingService
            };

            // Initialize all services
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].Init();
            }
        }

        private void OnDestroy()
        {
            // Destroy all services
            for (int i = 0; i < _services.Count; i++)
            {
                _services[i].Destroy();
            }

            _services.Clear();
        }
    }
}