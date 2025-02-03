namespace KConcurrent.Queue
{
    public interface IQueueIn<T>
    {
        #region Properties
        public bool IsAvailable
        {
            get;
        }
        #endregion

        #region Method
        public void Disable();
        public void Enqueue(T value);
        public Task EnqueueAsync(T value, CancellationToken cancellationToken = default);
        public bool TryEnqueue(T value);
        public Task<bool> TryEnqueueAsync(T value, CancellationToken cancellationToken = default);
        #endregion
    }
}
