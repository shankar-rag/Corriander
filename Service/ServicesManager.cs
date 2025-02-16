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
        public static ObjectPoolingService ObjectPoolingService { get; private set; } = new();
        #endregion

        private List<IService> _services = new List<IService>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            InitService(ObjectPoolingService);
        }

        private void InitService(IService service)
        {
            service.Init();
            _services.Add(service);
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