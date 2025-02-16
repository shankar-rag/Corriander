namespace TAG.Core.Pooling
{
    /// <summary>
    /// Defines necessary functionality for a pooled object.
    /// </summary>
    public interface IPooledObject
    {
        /// <summary>
        /// ID of the object. This is used to fetch items from the pool.
        /// </summary>
        public string PoolID { get; }

        /// <summary>
        /// Is the pooled object active and being used.
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Creates a new pooled object.
        /// </summary>
        /// <returns>A new IPooledObject object</returns>
        public IPooledObject Create();

        /// <summary>
        /// Called when a pool object is fetched from the pool and activated.
        /// </summary>
        public void Activated();

        /// <summary>
        /// Called when a pool object is deactivated and returned to the pool.
        /// </summary>
        public void Deactivated();

        /// <summary>
        /// Called when the a pool object is destroyed.
        /// </summary>
        public void Destroyed();
    }
}
