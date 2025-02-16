namespace TAG.Core.Pooling
{
    using UnityEngine;
    using System.Collections.Generic;
    using TAG.Core.Services;

    /// <summary>
    /// Object pooling service that holds a pool of objects tied to a key.
    /// </summary>
    public class ObjectPoolingService : IService
    {
        private const int DEFAULT_POOL_OBJECTS_TO_INIT = 10;

        private Dictionary<string, List<IPooledObject>> _pooledObjectDictionary = default;

        /// <summary>
        /// Initializes the object pooling service.
        /// </summary>
        public void Init()
        {
            _pooledObjectDictionary = new Dictionary<string, List<IPooledObject>>();
        }

        /// <summary>
        /// Initializes a new pool for the object.
        /// </summary>
        /// <param name="sourceObject">Object to create a pool for</param>
        /// <param name="count">Number of copies to make</param>
        /// <param name="includeSourceInPool">Should the source object be included in the pool</param>
        public void InitPool(IPooledObject sourceObject, int count = DEFAULT_POOL_OBJECTS_TO_INIT, bool includeSourceInPool = false)
        {
            string poolId = sourceObject.PoolID;

            // If this is a new key, initialize a new list for it
            if (!_pooledObjectDictionary.ContainsKey(poolId))
            {
                _pooledObjectDictionary.Add(poolId, new List<IPooledObject>());
                if (includeSourceInPool)
                {
                    _pooledObjectDictionary[poolId].Add(sourceObject);
                }
            }

            // Create a new set of pool objets and add it to the key's list
            for (int i = 0; i < count; i++)
            {
                IPooledObject createdPooledObject = sourceObject.Create();
                createdPooledObject.Deactivated();
                _pooledObjectDictionary[poolId].Add(createdPooledObject);
            }
        }

        /// <summary>
        /// Gets an unused object from the pool.
        /// </summary>
        /// <param name="poolId">Id to fetch from</param>
        /// <returns>Available pool object, if none available a new object is created and returned</returns>
        public IPooledObject GetObjectFromPool(string poolId)
        {
            Debug.Assert(_pooledObjectDictionary.ContainsKey(poolId), $"Attempting to access a non-existent Pool ID: {poolId}");

            // Try and find the first free pool object
            IPooledObject pooledObject = _pooledObjectDictionary[poolId].Find(poolObject => !poolObject.IsActive);
            if (pooledObject == null)
            {
                // If no available pool objects are found, create a copy of the first one in the list and return that
                IPooledObject firstPooledObjectInList = _pooledObjectDictionary[poolId][0];
                IPooledObject createdPooledObject = firstPooledObjectInList.Create();
                _pooledObjectDictionary[poolId].Add(createdPooledObject);
                pooledObject = createdPooledObject;
            }

            pooledObject.Activated();
            return pooledObject;
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="pooledObject">Object to return</param>
        public void ReturnToPool(IPooledObject pooledObject)
        {
            pooledObject.Deactivated();
        }

        /// <summary>
        /// Destroys all objects within a pool and removes it from the pool dictionary.
        /// </summary>
        /// <param name="poolId">Id to destroy</param>
        public void DestroyPool(string poolId)
        {
            if (!_pooledObjectDictionary.ContainsKey(poolId))
            {
                Debug.LogWarning($"Attempting to destroy a non-existent pool ID: {poolId}");
                return;
            }

            for (int i = _pooledObjectDictionary[poolId].Count - 1; i >= 0; i--)
            {
                _pooledObjectDictionary[poolId][i].Destroyed();
            }
            _pooledObjectDictionary.Remove(poolId);
        }

        /// <summary>
        /// Clear pool dictonary
        /// </summary>
        public void Destroy()
        {
            foreach (List<IPooledObject> pooledObjectList in _pooledObjectDictionary.Values)
            {
                for (int i = pooledObjectList.Count - 1; i >= 0; i--)
                {
                    pooledObjectList[i].Destroyed();
                }
            }

            _pooledObjectDictionary.Clear();
        }
    }
}