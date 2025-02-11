using KLibStandard.Concurrent.Locker;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace KLibStandard.Concurrent.Queue
{
    public class TwiceQueue<T>
    {
        #region Properties
        private static readonly T[] ARRAY_EMPTY = new T[0];

        private readonly List<TwiceQueueIn<T>> listQueueIn;
        private InterInt indexIn,
            indexOut;
        private readonly TicketLock lockQueueIn;

        /// <summary>
        /// Gets the number of elements contained in the Queue<T>.
        /// </summary>
        public int Count => indexIn.Value;
        #endregion

        #region Construction
        public TwiceQueue()
        {
            listQueueIn = new List<TwiceQueueIn<T>>();
            indexIn = new InterInt();
            indexOut = new InterInt();
            lockQueueIn = new TicketLock();
        }
        #endregion

        #region Method
        /// <summary>
        /// Create new QueueIn
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IQueueIn<T> CreateQueueIn()
        {
            Ticket ticket = lockQueueIn.Wait();
            //
            try
            {
                TwiceQueueIn<T> queueIn = new TwiceQueueIn<T>(indexIn);
                listQueueIn.Add(queueIn);
                return queueIn;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ticket.Release();
            }
        }
        /// <summary>
        /// Create new QueueIn
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<IQueueIn<T>> CreateQueueInAsync()
        {
            Ticket ticket = await lockQueueIn.WaitAsync();
            //
            try
            {
                TwiceQueueIn<T> queueIn = new TwiceQueueIn<T>(indexIn);
                listQueueIn.Add(queueIn);
                return queueIn;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ticket.Release();
            }
        }
        /// <summary>
        /// Try create new QueueIn
        /// </summary>
        public bool TryCreateQueueIn([NotNullWhen(true)] out IQueueIn<T> queueIn)
        {
            Ticket ticket = lockQueueIn.Wait();
            //
            try
            {
                TwiceQueueIn<T> tmpQueueIn = new TwiceQueueIn<T>(indexIn);
                listQueueIn.Add(tmpQueueIn);
                queueIn = tmpQueueIn;
                return true;
            }
            catch (Exception)
            {
                queueIn = default;
                return false;
            }
            finally
            {
                ticket.Release();
            }
        }
        private int Swap()
        {
            (indexIn, indexOut) = (indexOut, indexIn);
            //
            indexIn.Value = 0;
            int countQueue = listQueueIn.Count;
            for (int i = 0; i < countQueue; i++)
                listQueueIn[i].Swap(indexIn);
            //
            return indexOut;
        }
        private async Task<int> SwapAsync()
        {
            (indexIn, indexOut) = (indexOut, indexIn);
            indexIn.Value = 0;
            //
            int countQueue = listQueueIn.Count;
            Task[] tasks = new Task[countQueue];
            for (int i = 0; i < countQueue; i++)
                tasks[i] = listQueueIn[i].SwapAsync(indexIn);
            //
            await Task.WhenAll(tasks);
            return indexOut;
        }
        #endregion

        #region Out Dequeue
        /// <summary>
        /// Removes and copies the object at the beginning of Queue List<T> to a new array.
        /// </summary>
        public T[] DequeueAll()
        {
            Ticket ticket = lockQueueIn.Wait();
            //
            int countQueue = listQueueIn.Count;
            List<int> removeIndexs = new List<int>(countQueue);
            for (int i = 0; i < countQueue; i++)
                if (!listQueueIn[i].IsAvailable)
                    removeIndexs.Add(i);
            try
            {
                int count = Swap();
                if (count <= 0)
                    return ARRAY_EMPTY;
                //
                T[] values = new T[count];
                for (int i = 0; i < countQueue; i++)
                    listQueueIn[i].Dequeue(values);
                return values;
            }
            catch (Exception)
            {
                return ARRAY_EMPTY;
            }
            finally
            {
                for (int i = removeIndexs.Count - 1; i >= 0; i--)
                {
                    int index = removeIndexs[i];
                    listQueueIn.RemoveAt(index);
                }
                ticket.Release();
            }
        }
        /// <summary>
        /// Removes and copies the object at the beginning of Queue List<T> to a new array.
        /// </summary>
        public async Task<T[]> DequeueAllAsync()
        {
            Ticket ticket = await lockQueueIn.WaitAsync();
            //
            int countQueue = listQueueIn.Count;
            List<int> removeIndexs = new List<int>(countQueue);
            for (int i = 0; i < countQueue; i++)
                if (!listQueueIn[i].IsAvailable)
                    removeIndexs.Add(i);
            try
            {
                int count = await SwapAsync();
                if (count <= 0)
                    return ARRAY_EMPTY;
                //
                T[] values = new T[count];
                for (int i = 0; i < countQueue; i++)
                    listQueueIn[i].Dequeue(values);
                return values;
            }
            catch (Exception)
            {
                return ARRAY_EMPTY;
            }
            finally
            {
                for (int i = removeIndexs.Count - 1; i >= 0; i--)
                {
                    int index = removeIndexs[i];
                    listQueueIn.RemoveAt(index);
                }
                ticket.Release();
            }
        }
        /// <summary>
        /// Try removes and copies the object at the beginning of Queue List<T> to a new array.
        /// </summary>
        public bool TryDequeueAll([NotNullWhen(true)] out T[] values)
        {
            Ticket ticket = lockQueueIn.Wait();
            //
            int countQueue = listQueueIn.Count;
            List<int> removeIndexs = new List<int>(countQueue);
            for (int i = 0; i < countQueue; i++)
                if (!listQueueIn[i].IsAvailable)
                    removeIndexs.Add(i);
            try
            {
                int count = Swap();
                if (count <= 0)
                {
                    values = null;
                    return false;
                }
                //
                values = new T[count];
                for (int i = 0; i < countQueue; i++)
                    listQueueIn[i].Dequeue(values);
                return true;
            }
            catch (Exception)
            {
                values = null;
                return false;
            }
            finally
            {
                for (int i = removeIndexs.Count - 1; i >= 0; i--)
                {
                    int index = removeIndexs[i];
                    listQueueIn.RemoveAt(index);
                }
                ticket.Release();
            }
        }
        #endregion

        #region Out Clear
        /// <summary>
        /// Removes all objects from the Queue<T>.
        /// </summary>
        public T[] Clear()
        {
            Ticket ticket = lockQueueIn.Wait();
            //
            int countQueue = listQueueIn.Count;
            for (int i = 0; i < countQueue; i++)
                listQueueIn[i].Disable();
            //
            try
            {
                int count = Swap();
                if (count <= 0)
                    return ARRAY_EMPTY;
                //
                T[] values = new T[count];
                for (int i = 0; i < countQueue; i++)
                    listQueueIn[i].Dequeue(values);
                return values;
            }
            catch (Exception)
            {
                return ARRAY_EMPTY;
            }
            finally
            {
                listQueueIn.Clear();
                ticket.Release();
            }
        }
        /// <summary>
        /// Removes all objects from the Queue<T>.
        /// </summary>
        public async Task<T[]> ClearAsync()
        {
            Ticket ticket = await lockQueueIn.WaitAsync();
            //
            int countQueue = listQueueIn.Count;
            for (int i = 0; i < countQueue; i++)
                listQueueIn[i].Disable();
            //
            try
            {
                int count = await SwapAsync();
                if (count <= 0)
                    return ARRAY_EMPTY;
                //
                T[] values = new T[count];
                for (int i = 0; i < countQueue; i++)
                    listQueueIn[i].Dequeue(values);
                return values;
            }
            catch (Exception)
            {
                return ARRAY_EMPTY;
            }
            finally
            {
                listQueueIn.Clear();
                ticket.Release();
            }
        }
        /// <summary>
        /// Try removes all objects from the Queue<T>.
        /// </summary>
        public bool TryClear([NotNullWhen(true)] out T[] values)
        {
            Ticket ticket = lockQueueIn.Wait();
            //
            int countQueue = listQueueIn.Count;
            for (int i = 0; i < countQueue; i++)
                listQueueIn[i].Disable();
            //
            try
            {
                int count = Swap();
                if (count <= 0)
                {
                    values = null;
                    return false;
                }
                //
                values = new T[count];
                for (int i = 0; i < countQueue; i++)
                    listQueueIn[i].Dequeue(values);
                return true;
            }
            catch (Exception)
            {
                values = null;
                return false;
            }
            finally
            {
                listQueueIn.Clear();
                ticket.Release();
            }
        }
        #endregion
    }
}