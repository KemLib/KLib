using KLib.Concurrent.Locker;

namespace KLib.Concurrent.Queue
{
    public class SingleQueue<T> : IQueueIn<T>
    {
        #region Properties
        private const string ERROR_OPERATION_CANCELED_EXCEPTION = "CancellationToken was canceled",
           ERROR_LOCKER_ENTER_FAIL = "Locker enter fail";
        private static readonly T[] ARRAY_EMPTY = [];

        private readonly Queue<T> queue;
        private readonly TicketLock lockObject;
        private InterValueInt interCount;
        private InterValueBool isAvailable;
        /// <summary>
        /// Gets the number of elements contained in the Queue<T>.
        /// </summary>
        public int Count => interCount.Value;

        public bool IsAvailable => isAvailable;
        #endregion

        #region Construction
        public SingleQueue()
        {
            queue = [];
            lockObject = new();
            interCount = new();
            isAvailable = new();
        }
        public SingleQueue(int capacity)
        {
            queue = new(Math.Max(0, capacity));
            lockObject = new();
            interCount = new();
            isAvailable = new();
        }
        #endregion

        #region Method
        public void Active()
        {
            isAvailable.Exchange(true);
        }
        #endregion

        #region In Add
        public void Disable()
        {
            isAvailable.Exchange(false);
        }
        public void Enqueue(T value)
        {
            if (!IsAvailable)
                return;
            //
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
                return;
            //
            try
            {
                if (!IsAvailable)
                    return;
                //
                queue.Enqueue(value);
                interCount.Increment();
            }
            finally
            {
                ticket.Release();
            }
        }
        public async Task EnqueueAsync(T value, CancellationToken cancellationToken = default)
        {
            if (!IsAvailable)
                return;
            //
            Ticket ticket = await lockObject.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return;
            //
            try
            {
                if (!IsAvailable)
                    return;
                //
                queue.Enqueue(value);
                interCount.Increment();
            }
            finally
            {
                ticket.Release();
            }
        }
        public bool TryEnqueue(T value)
        {
            if (!IsAvailable)
                return false;
            //
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
                return false;
            //
            try
            {
                if (!IsAvailable)
                    return false;
                //
                queue.Enqueue(value);
                interCount.Increment();
                //
                return true;
            }
            finally
            {
                ticket.Release();
            }
        }
        public async Task<bool> TryEnqueueAsync(T value, CancellationToken cancellationToken = default)
        {
            if (!IsAvailable)
                return false;
            //
            Ticket ticket = await lockObject.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return false;
            //
            try
            {
                if (!IsAvailable)
                    return false;
                //
                queue.Enqueue(value);
                interCount.Increment();
                //
                return true;
            }
            finally
            {
                ticket.Release();
            }
        }
        #endregion

        #region Dequeue
        /// <summary>
        /// Removes and returns the object at the beginning of the Queue<T>.
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public T Dequeue()
        {
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
                throw new Exception(ERROR_LOCKER_ENTER_FAIL);
            //
            try
            {
                T value = queue.Dequeue();
                interCount.Decrement();
                //
                return value;
            }
            finally
            {
                ticket.Release();
            }
        }
        /// <summary>
        /// Removes and returns the object at the beginning of the Queue<T>.
        /// </summary>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
        {
            Ticket ticket = await lockObject.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                throw new OperationCanceledException(ERROR_OPERATION_CANCELED_EXCEPTION);
            //
            try
            {
                T value = queue.Dequeue();
                interCount.Decrement();
                //
                return value;
            }
            finally
            {
                ticket.Release();
            }
        }
        /// <summary>
        /// Try removes and returns the object at the beginning of the Queue<T>.
        /// </summary>
        public bool TryDequeue(out T? value)
        {
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
            {
                value = default;
                return false;
            }
            //
            try
            {
                if (queue.TryDequeue(out value))
                {
                    interCount.Decrement();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                value = default;
                return false;
            }
            finally
            {
                ticket.Release();
            }
        }
        #endregion

        #region Dequeue All
        /// <summary>
        /// Removes and copies the object at the beginning of Queue List<T> to a new array.
        /// </summary>
        public T[] DequeueAll()
        {
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
            //
            T[] values;
            if (queue.Count == 0)
            {
                values = ARRAY_EMPTY;
            }
            else
            {
                values = [.. queue];
                queue.Clear();
                interCount.Exchange(0);
            }
            //
            ticket.Release();
            return values;
        }
        /// <summary>
        /// Removes and copies the object at the beginning of Queue List<T> to a new array.
        /// </summary>
        public async Task<T[]> DequeueAllAsync(CancellationToken cancellationToken = default)
        {
            Ticket ticket = await lockObject.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
            //
            T[] values;
            if (queue.Count == 0)
            {
                values = ARRAY_EMPTY;
            }
            else
            {
                values = [.. queue];
                queue.Clear();
                interCount.Exchange(0);
            }
            //
            ticket.Release();
            return values;
        }
        /// <summary>
        /// Try removes and copies the object at the beginning of Queue List<T> to a new array.
        /// </summary>
        public bool TryDequeueAll([NotNullWhen(true)] out T[]? values)
        {
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
            {
                values = null;
                return false;
            }
            //
            try
            {
                if (queue.Count == 0)
                {
                    values = null;
                    return false;
                }
                //
                values = [.. queue];
                queue.Clear();
                interCount.Exchange(0);
                //
                return true;
            }
            finally
            {
                ticket.Release();
            }
        }
        #endregion
    }
}
