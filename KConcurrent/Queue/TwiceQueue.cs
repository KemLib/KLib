using KConcurrent.Locker;

namespace KConcurrent.Queue
{
    public class TwiceQueue<T>
    {
        #region Properties
        private const string ERROR_OPERATION_CANCELED_EXCEPTION = "CancellationToken was canceled",
            ERROR_LOCKER_ENTER_FAIL = "Locker enter fail";
        private static readonly T[] ARRAY_EMPTY = [];

        private readonly List<TwiceQueueIn<T>> listQueueIn;
        private InterInt indexIn,
            indexOut;
        private readonly TicketLock lockQueueIn;

        public int Count => indexIn.Value;
        #endregion

        #region Construction
        public TwiceQueue()
        {
            listQueueIn = [];
            indexIn = new();
            indexOut = new();
            lockQueueIn = new();
        }
        #endregion

        #region Method
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IQueueIn<T> CreateQueueIn()
        {
            Ticket ticket = lockQueueIn.Wait();
            if (!ticket.IsAccept)
                throw new Exception(ERROR_LOCKER_ENTER_FAIL);
            //
            try
            {
                TwiceQueueIn<T> queueIn = new(indexIn);
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
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<IQueueIn<T>> CreateQueueInAsync(CancellationToken cancellationToken = default)
        {
            Ticket ticket = await lockQueueIn.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                throw new OperationCanceledException(ERROR_OPERATION_CANCELED_EXCEPTION);
            //
            try
            {
                TwiceQueueIn<T> queueIn = new(indexIn);
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
        public bool TryCreateQueueIn([NotNullWhen(true)] out IQueueIn<T>? queueIn)
        {
            Ticket ticket = lockQueueIn.Wait();
            if (!ticket.IsAccept)
            {
                queueIn = default;
                return false;
            }
            //
            try
            {
                TwiceQueueIn<T> tmpQueueIn = new(indexIn);
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
        public T[] DequeueAll()
        {
            Ticket ticket = lockQueueIn.Wait();
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
            //
            int countQueue = listQueueIn.Count;
            List<int> removeIndexs = new(countQueue);
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
        public async Task<T[]> DequeueAllAsync(CancellationToken cancellationToken = default)
        {
            Ticket ticket = await lockQueueIn.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
            //
            int countQueue = listQueueIn.Count;
            List<int> removeIndexs = new(countQueue);
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
        public bool TryDequeueAll([NotNullWhen(true)] out T[]? values)
        {
            Ticket ticket = lockQueueIn.Wait();
            if (!ticket.IsAccept)
            {
                values = null;
                return false;
            }
            //
            int countQueue = listQueueIn.Count;
            List<int> removeIndexs = new(countQueue);
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
        public T[] Clear()
        {
            Ticket ticket = lockQueueIn.Wait();
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
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
        public async Task<T[]> ClearAsync(CancellationToken cancellationToken = default)
        {
            Ticket ticket = await lockQueueIn.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return ARRAY_EMPTY;
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
        public bool TryClear([NotNullWhen(true)] out T[]? values)
        {
            Ticket ticket = lockQueueIn.Wait();
            if (!ticket.IsAccept)
            {
                values = null;
                return false;
            }
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