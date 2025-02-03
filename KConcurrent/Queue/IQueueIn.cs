namespace KConcurrent.Queue
{
    public interface IQueueIn<T>
    {
        #region Properties
        /// <summary>
        /// Queue available to be Enqueue.
        /// </summary>
        public bool IsAvailable
        {
            get;
        }
        #endregion

        #region Method
        /// <summary>
        /// Disable enqueue feature.
        /// </summary>
        public void Disable();
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        public void Enqueue(T value);
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        public Task EnqueueAsync(T value, CancellationToken cancellationToken = default);
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        public bool TryEnqueue(T value);
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        public Task<bool> TryEnqueueAsync(T value, CancellationToken cancellationToken = default);
        #endregion
    }
}
