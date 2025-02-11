using KLibStandard.Concurrent.Locker;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace KLibStandard.Concurrent.Queue
{
    public class SingleQueue<T> : IQueueIn<T>
    {
        #region Properties
        private static readonly T[] ARRAY_EMPTY = new T[0];

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
            queue = new Queue<T>();
            lockObject = new TicketLock();
            interCount = new InterValueInt();
            isAvailable = new InterValueBool();
        }
        public SingleQueue(int capacity)
        {
            queue = new Queue<T>(Math.Max(0, capacity));
            lockObject = new TicketLock();
            interCount = new InterValueInt();
            isAvailable = new InterValueBool();
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
            isAvailable.Exchange(false);
        }
        public void Enqueue(T value)
        {
            if (!IsAvailable)
                return;
            //
            Ticket ticket = lockObject.Wait();
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
        public async Task EnqueueAsync(T value)
        {
            if (!IsAvailable)
                return;
            //
            Ticket ticket = await lockObject.WaitAsync();
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
        public async Task<bool> TryEnqueueAsync(T value)
        {
            if (!IsAvailable)
                return false;
            //
            Ticket ticket = await lockObject.WaitAsync();
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
        public async Task<T> DequeueAsync()
        {
            Ticket ticket = await lockObject.WaitAsync();
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
        public bool TryDequeue(out T value)
        {
            Ticket ticket = lockObject.Wait();
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
            //
            T[] values;
            if (queue.Count == 0)
            {
                values = ARRAY_EMPTY;
            }
            else
            {
                values = queue.ToArray();
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
        public async Task<T[]> DequeueAllAsync()
        {
            Ticket ticket = await lockObject.WaitAsync();
            //
            T[] values;
            if (queue.Count == 0)
            {
                values = ARRAY_EMPTY;
            }
            else
            {
                values = queue.ToArray();
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
        public bool TryDequeueAll([NotNullWhen(true)] out T[] values)
        {
            Ticket ticket = lockObject.Wait();
            //
            try
            {
                if (queue.Count == 0)
                {
                    values = null;
                    return false;
                }
                //
                values = queue.ToArray();
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
