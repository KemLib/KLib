using KLib.Concurrent.Locker;

namespace KLib.Concurrent.Queue
{
    internal class TwiceQueueIn<T> : IQueueIn<T>
    {
        #region Properties
        private InterInt index;
        private Dictionary<int, T> dicIn,
            dicOut;
        private readonly TicketLock lockDicIn;
        private InterValueBool isAvailable;

        public bool IsAvailable => isAvailable;
        #endregion

        #region Construction
        internal TwiceQueueIn(InterInt index)
        {
            this.index = index;
            dicIn = [];
            dicOut = [];
            lockDicIn = new();
            isAvailable = new(true);
        }
        #endregion

        #region Method
        internal void Swap(InterInt index)
        {
            Ticket ticket = lockDicIn.Wait();
            if (!ticket.IsAccept)
                return;
            //
            try
            {
                (dicIn, dicOut) = (dicOut, dicIn);
                //
                this.index = index;
            }
            catch (Exception)
            {

            }
            finally
            {
                ticket.Release();
            }
        }
        internal async Task SwapAsync(InterInt index)
        {
            Ticket ticket = await lockDicIn.WaitAsync();
            if (!ticket.IsAccept)
                return;
            //
            try
            {
                (dicIn, dicOut) = (dicOut, dicIn);
                //
                this.index = index;
            }
            catch (Exception)
            {

            }
            finally
            {
                ticket.Release();
            }
        }
        internal void Dequeue(T[] values)
        {
            foreach (int index in dicOut.Keys)
                values[index] = dicOut[index];
            dicOut.Clear();
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
            Ticket ticket = lockDicIn.Wait();
            if (!ticket.IsAccept)
                return;
            try
            {
                if (!IsAvailable)
                    return;
                //
                int i = index.Increment() - 1;
                dicIn.Add(i, value);
            }
            catch (Exception)
            {

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
            Ticket ticket = await lockDicIn.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return;
            try
            {
                if (!IsAvailable)
                    return;
                //
                int i = index.Increment() - 1;
                dicIn.Add(i, value);
            }
            catch (Exception)
            {

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
            Ticket ticket = lockDicIn.Wait();
            if (!ticket.IsAccept)
                return false;
            try
            {
                if (!IsAvailable)
                    return false;
                //
                int i = index.Increment() - 1;
                dicIn.Add(i, value);
                //
                return true;
            }
            catch (Exception)
            {
                return false;
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
            Ticket ticket = await lockDicIn.WaitAsync(cancellationToken);
            if (!ticket.IsAccept)
                return false;
            try
            {
                if (!IsAvailable)
                    return false;
                //
                int i = index.Increment() - 1;
                dicIn.Add(i, value);
                //
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                ticket.Release();
            }
        }
        #endregion
    }
}
