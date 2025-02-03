using KConcurrent.Locker;

namespace KConcurrent.Queue
{
    public class BacklogQueue<T> : IQueueIn<T>
    {
        #region Properties
        private const string ERROR_OBJECT_DISPOSED_EXCEPTION = "The BacklogQueue instance has been disposed",
            ERROR_OPERATION_CANCELED_EXCEPTION = "CancellationToken was canceled",
            ERROR_LOCKER_ENTER_FAIL = "Locker enter fail";
        private static readonly T[] ARRAY_EMPTY = [];

        private readonly Queue<T> queueValue;
        private TaskCompletionSource<T>? taskSource;
        private readonly TicketLock lockObject;
        private InterValueBool isAvailable;
        private InterValueInt interCount;

        public bool IsAvailable => isAvailable;
        public int Count => interCount;
        #endregion

        #region Construction
        public BacklogQueue()
        {
            queueValue = new();
            lockObject = new();
            isAvailable = new();
            interCount = new();
        }
        public BacklogQueue(int capacity)
        {
            queueValue = new(capacity);
            lockObject = new();
            isAvailable = new();
            interCount = new();
        }
        #endregion

        #region Method
        public void Active()
        {
            isAvailable.Exchange(true);
        }
        #endregion

        #region In
        public void Disable()
        {
            if (!isAvailable.TryExchange(false))
                return;
            //
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
                return;
            //
            if (taskSource == null)
                return;
            //
            TaskCompletionSource<T>? currentTaskSource = taskSource;
            taskSource = null;
            ticket.Release();
            //
            currentTaskSource?.TrySetException(new ObjectDisposedException(ERROR_OBJECT_DISPOSED_EXCEPTION));
        }
        public void Enqueue(T value)
        {
            if (!IsAvailable)
                return;
            //
            do
            {
                Ticket ticket = lockObject.Wait();
                if (!ticket.IsAccept)
                    return;
                //
                TaskCompletionSource<T> currentTaskSource;
                try
                {
                    if (!IsAvailable)
                        return;
                    //
                    if (taskSource == null)
                    {
                        queueValue.Enqueue(value);
                        interCount.Increment();
                        return;
                    }
                    currentTaskSource = taskSource;
                    taskSource = null;
                }
                catch (Exception)
                {
                    return;
                }
                finally
                {
                    ticket.Release();
                }
                //
                if (currentTaskSource.TrySetResult(value))
                    return;
                //
            } while (true);
        }
        public async Task EnqueueAsync(T value, CancellationToken cancellationToken = default)
        {
            if (!IsAvailable)
                return;
            //
            do
            {
                Ticket ticket = await lockObject.WaitAsync(cancellationToken);
                if (!ticket.IsAccept)
                    return;
                //
                TaskCompletionSource<T> currentTaskSource;
                try
                {
                    if (!IsAvailable)
                        return;
                    //
                    if (taskSource == null)
                    {
                        queueValue.Enqueue(value);
                        interCount.Increment();
                        return;
                    }
                    currentTaskSource = taskSource;
                    taskSource = null;
                }
                catch (Exception)
                {
                    return;
                }
                finally
                {
                    ticket.Release();
                }
                //
                if (currentTaskSource.TrySetResult(value))
                    return;
                //
            } while (true);
        }
        public bool TryEnqueue(T value)
        {
            if (!IsAvailable)
                return false;
            //
            do
            {
                Ticket ticket = lockObject.Wait();
                if (!ticket.IsAccept)
                    return false;
                //
                TaskCompletionSource<T> currentTaskSource;
                try
                {
                    if (!IsAvailable)
                        return false;
                    //
                    if (taskSource == null)
                    {
                        queueValue.Enqueue(value);
                        interCount.Increment();
                        return true;
                    }
                    currentTaskSource = taskSource;
                    taskSource = null;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    ticket.Release();
                }
                //
                if (currentTaskSource.TrySetResult(value))
                    return true;
                //
            } while (true);
        }
        public async Task<bool> TryEnqueueAsync(T value, CancellationToken cancellationToken = default)
        {
            if (!IsAvailable)
                return false;
            //
            do
            {
                Ticket ticket = await lockObject.WaitAsync(cancellationToken);
                if (!ticket.IsAccept)
                    return false;
                //
                TaskCompletionSource<T> currentTaskSource;
                try
                {
                    if (!IsAvailable)
                        return false;
                    //
                    if (taskSource == null)
                    {
                        queueValue.Enqueue(value);
                        interCount.Increment();
                        return true;
                    }
                    currentTaskSource = taskSource;
                    taskSource = null;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    ticket.Release();
                }
                //
                if (currentTaskSource.TrySetResult(value))
                    return true;
                //
            } while (true);
        }
        #endregion

        #region Out Dequeue
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Dequeue()
        {
            ObjectDisposedException.ThrowIf(!IsAvailable, this);
            //
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
                throw new Exception(ERROR_LOCKER_ENTER_FAIL);
            //
            TaskCompletionSource<T> taskSource;
            try
            {
                ObjectDisposedException.ThrowIf(!IsAvailable, this);
                //
                if (queueValue.TryDequeue(out T? value))
                {
                    interCount.Decrement();
                    return value;
                }
                //
                taskSource = new();
                this.taskSource = taskSource;
            }
            finally
            {
                ticket.Release();
            }
            //
            taskSource.Task.Wait();
            //
            return taskSource.Task.Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
        {
            ObjectDisposedException.ThrowIf(!IsAvailable, this);
            //
            Ticket ticket = await lockObject.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                throw new OperationCanceledException(ERROR_OPERATION_CANCELED_EXCEPTION);
            //
            TaskCompletionSource<T> taskSource;
            try
            {
                ObjectDisposedException.ThrowIf(!IsAvailable, this);
                //
                if (queueValue.TryDequeue(out T? value))
                {
                    interCount.Decrement();
                    return value;
                }
                //
                taskSource = new();
                this.taskSource = taskSource;
            }
            finally
            {
                ticket.Release();
            }
            //
            try
            {
                await taskSource.Task.WaitAsync(cancellationToken);
                return taskSource.Task.Result;
            }
            catch (OperationCanceledException ex)
            {
                if (!taskSource.TrySetException(ex) && taskSource.Task.IsCompletedSuccessfully)
                    return taskSource.Task.Result;
                throw;
            }
        }
        public bool TryDequeue(out T? value)
        {
            if (!IsAvailable)
            {
                value = default;
                return false;
            }
            //
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
            {
                value = default;
                return false;
            }
            //
            TaskCompletionSource<T> taskSource;
            try
            {
                if (!IsAvailable)
                {
                    value = default;
                    return false;
                }
                //
                if (queueValue.TryDequeue(out value))
                {
                    interCount.Decrement();
                    return true;
                }
                //
                taskSource = new();
                this.taskSource = taskSource;
            }
            finally
            {
                ticket.Release();
            }
            //
            try
            {
                taskSource.Task.Wait();
                value = taskSource.Task.Result;
                return true;
            }
            catch (Exception)
            {
                value = default;
                return false;
            }
        }
        #endregion

        #region Out Clear
        public T[] Clear()
        {
            Ticket ticket = lockObject.Wait();
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
            //
            try
            {
                if (queueValue.Count == 0)
                    return ARRAY_EMPTY;
                //
                T[] values = [.. queueValue];
                queueValue.Clear();
                interCount.Exchange(0);
                return values;
            }
            finally
            {
                ticket.Release();
            }
        }
        public async Task<T[]> ClearAsync(CancellationToken cancellationToken = default)
        {
            Ticket ticket = await lockObject.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
            //
            try
            {
                if (queueValue.Count == 0)
                    return ARRAY_EMPTY;
                //
                T[] values = [.. queueValue];
                queueValue.Clear();
                interCount.Exchange(0);
                return values;
            }
            finally
            {
                ticket.Release();
            }
        }
        public bool TryClear([NotNullWhen(true)] out T[]? values)
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
                if (queueValue.Count == 0)
                {
                    values = null;
                    return false;
                }
                //
                values = [.. queueValue];
                queueValue.Clear();
                interCount.Exchange(0);
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
