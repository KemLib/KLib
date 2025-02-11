using System.Threading;
using System.Threading.Tasks;

namespace KLibStandard.Concurrent.Locker
{
    public struct Int64Lock
    {
        #region Properties
        private const int ENTER_WAIT_TIME = 1;

        private long valueA,
            valueB;
        #endregion

        #region Construction
        public Int64Lock(long startValue = 0)
        {
            valueA = startValue;
            valueB = valueA + 1;
        }
        #endregion

        #region Method
        /// <summary>
        /// Blocks the current thread until it can enter the Int64Lock.
        /// </summary>
        public void Enter()
        {
            long newValueA = Interlocked.Add(ref valueA, 1);
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
        /// Asynchronously waits to enter the Int64Lock.
        /// </summary>
        public async Task EnterAsync()
        {
            long newValueA = Interlocked.Add(ref valueA, 1);
            if (newValueA == Interlocked.CompareExchange(ref valueB, 0, 0))
                return;
            //
            do
            {
                await Task.Delay(ENTER_WAIT_TIME);
            } while (newValueA != Interlocked.CompareExchange(ref valueB, 0, 0));
        }
        /// <summary>
        /// Releases the Int64Lock.
        /// </summary>
        public void Exit()
        {
            Interlocked.Add(ref valueB, 1);
        }
        #endregion
    }
}
