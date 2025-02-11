using System.Threading.Tasks;

namespace KLibStandard.Concurrent.Locker
{
    public readonly struct Ticket
    {
        #region Properties
        private readonly TaskCompletionSource<bool> taskSource;
        #endregion

        #region Construction
        public Ticket(TaskCompletionSource<bool> taskSource)
        {
            this.taskSource = taskSource;
        }
        #endregion

        #region Method
        /// <summary>
        /// Releases the TicketLock object.
        /// </summary>
        public readonly void Release()
        {
            taskSource?.TrySetResult(true);
        }
        #endregion
    }
}
