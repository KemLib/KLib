using System;
using System.Threading;
using System.Threading.Tasks;

namespace KLibStandard.Concurrent.Locker
{
    public struct Int32Lock
    {
        #region Properties
        private const int ENTER_WAIT_TIME = 1;

        private int valueA,
            valueB;
        #endregion

        #region Construction
        public Int32Lock(int startValue = 0)
        {
            valueA = startValue;
            valueB = valueA + 1;
        }
        #endregion

        #region Method
        /// <summary>
        /// Blocks the current thread until it can enter the Int32Lock.
        /// </summary>
        public void Enter()
        {
            int newValueA = Interlocked.Add(ref valueA, 1);
            if (newValueA == Interlocked.CompareExchange(ref valueB, 0, 0))
                return;
            //
            SpinWait spinWait = new SpinWait();
            do
            {
                spinWait.SpinOnce();
            } while (newValueA != Interlocked.CompareExchange(ref valueB, 0, 0));
        }
        /// <summary>
        /// Asynchronously waits to enter the Int32Lock.
        /// </summary>
        public async Task EnterAsync()
        {
            int newValueA = Interlocked.Add(ref valueA, 1);
            if (newValueA == Interlocked.CompareExchange(ref valueB, 0, 0))
                return;
            //
            do
            {
                await Task.Delay(ENTER_WAIT_TIME);
            } while (newValueA != Interlocked.CompareExchange(ref valueB, 0, 0));
        }
        /// <summary>
        /// Releases the Int32Lock.
        /// </summary>
        public void Exit()
        {
            Interlocked.Add(ref valueB, 1);
        }
        #endregion
    }
}
