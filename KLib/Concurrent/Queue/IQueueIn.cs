namespace KLib.Concurrent.Queue
{
    public interface IQueueIn<T>
    {
        #region Properties
        /// <summary>
        /// Queue available to be Enqueue.
        /// </summary>
        bool IsAvailable
        {
            get;
        }
        #endregion

        #region Method
        /// <summary>
        /// Disable enqueue feature.
        /// </summary>
        void Disable();
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        void Enqueue(T value);
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        Task EnqueueAsync(T value, CancellationToken cancellationToken = default);
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        bool TryEnqueue(T value);
        /// <summary>
        /// Adds an object to the end of the Queue<T>.
        /// </summary>
        Task<bool> TryEnqueueAsync(T value, CancellationToken cancellationToken = default);
        #endregion
    }
}
