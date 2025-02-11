using KLibStandard.Concurrent.Locker;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace KLibStandard.Concurrent.Queue
{
    public class BacklogQueue<T> : IQueueIn<T>
    {
        #region Properties
        private const string ERROR_OBJECT_DISPOSED_EXCEPTION = "The BacklogQueue instance has been disposed";
        private const string OBJECT_NAME = "BacklogQueue";

        private static readonly T[] ARRAY_EMPTY = new T[0];

        private readonly Queue<T> queueValue;
        private TaskCompletionSource<T> taskSource;
        private readonly TicketLock lockObject;
        private InterValueBool isAvailable;
        private InterValueInt interCount;

        public bool IsAvailable => isAvailable;
        /// <summary>
        /// Gets the number of elements contained in the Queue<T>.
        /// </summary>
        public int Count => interCount;
        #endregion

        #region Construction
        public BacklogQueue()
        {
            queueValue = new Queue<T>();
            lockObject = new TicketLock();
            isAvailable = new InterValueBool();
            interCount = new InterValueInt();
        }
        public BacklogQueue(int capacity)
        {
            queueValue = new Queue<T>(capacity);
            lockObject = new TicketLock();
            isAvailable = new InterValueBool();
            interCount = new InterValueInt();
        }
        #endregion

        #region Method
        /// <summary>
        /// Enable enqueue feature.
        /// </summary>
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
            //
            if (taskSource == null)
                return;
            //
            TaskCompletionSource<T> currentTaskSource = taskSource;
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
        public async Task EnqueueAsync(T value)
        {
            if (!IsAvailable)
                return;
            //
            do
            {
                Ticket ticket = await lockObject.WaitAsync();
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
        public async Task<bool> TryEnqueueAsync(T value)
        {
            if (!IsAvailable)
                return false;
            //
            do
            {
                Ticket ticket = await lockObject.WaitAsync();
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
        /// Removes and returns the object at the beginning of the Queue<T>.
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Dequeue()
        {
            if (!isAvailable)
                throw new ObjectDisposedException(OBJECT_NAME, ERROR_OBJECT_DISPOSED_EXCEPTION);
            //
            Ticket ticket = lockObject.Wait();
            //
            TaskCompletionSource<T> taskSource;
            try
            {
                if (!isAvailable)
                    throw new ObjectDisposedException(OBJECT_NAME, ERROR_OBJECT_DISPOSED_EXCEPTION);
                //
                if (queueValue.TryDequeue(out T value))
                {
                    interCount.Decrement();
                    return value;
                }
                //
                taskSource = new TaskCompletionSource<T>();
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
        /// Removes and returns the object at the beginning of the Queue<T>.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<T> DequeueAsync()
        {
            if (!isAvailable)
                throw new ObjectDisposedException(OBJECT_NAME, ERROR_OBJECT_DISPOSED_EXCEPTION);
            //
            Ticket ticket = await lockObject.WaitAsync();
            //
            TaskCompletionSource<T> taskSource;
            try
            {
                if (!isAvailable)
                    throw new ObjectDisposedException(OBJECT_NAME, ERROR_OBJECT_DISPOSED_EXCEPTION);
                //
                if (queueValue.TryDequeue(out T value))
                {
                    interCount.Decrement();
                    return value;
                }
                //
                taskSource = new TaskCompletionSource<T>();
                this.taskSource = taskSource;
            }
            finally
            {
                ticket.Release();
            }
            //
            try
            {
                await taskSource.Task;
                return taskSource.Task.Result;
            }
            catch (OperationCanceledException ex)
            {
                if (!taskSource.TrySetException(ex) && taskSource.Task.IsCompletedSuccessfully)
                    return taskSource.Task.Result;
                throw;
            }
        }
        /// <summary>
        /// Try removes and returns the object at the beginning of the Queue<T>.
        /// </summary>
        public bool TryDequeue(out T value)
        {
            if (!IsAvailable)
            {
                value = default;
                return false;
            }
            //
            Ticket ticket = lockObject.Wait();
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
                taskSource = new TaskCompletionSource<T>();
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
        /// <summary>
        /// Removes all objects from the Queue<T>.
        /// </summary>
        public T[] Clear()
        {
            Ticket ticket = lockObject.Wait();
            //
            try
            {
                if (queueValue.Count == 0)
                    return ARRAY_EMPTY;
                //
                T[] values = queueValue.ToArray();
                queueValue.Clear();
                interCount.Exchange(0);
                return values;
            }
            finally
            {
                ticket.Release();
            }
        }
        /// <summary>
        /// Removes all objects from the Queue<T>.
        /// </summary>
        public async Task<T[]> ClearAsync()
        {
            Ticket ticket = await lockObject.WaitAsync();
            //
            try
            {
                if (queueValue.Count == 0)
                    return ARRAY_EMPTY;
                //
                T[] values = queueValue.ToArray();
                queueValue.Clear();
                interCount.Exchange(0);
                return values;
            }
            finally
            {
                ticket.Release();
            }
        }
        /// <summary>
        /// Removes all objects from the Queue<T>.
        /// </summary>
        public bool TryClear([NotNullWhen(true)] out T[] values)
        {
            Ticket ticket = lockObject.Wait();
            //
            try
            {
                if (queueValue.Count == 0)
                {
                    values = null;
                    return false;
                }
                //
                values = queueValue.ToArray();
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
