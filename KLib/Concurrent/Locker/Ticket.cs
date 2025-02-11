namespace KLib.Concurrent.Locker
{
    public readonly struct Ticket
    {
        #region Properties
        /// <summary>
        /// True if ticket enter the TicketLock.
        /// </summary>
        public readonly bool IsAccept;
        private readonly TaskCompletionSource? taskSource;
        #endregion

        #region Construction
        public Ticket()
        {
            IsAccept = false;
            taskSource = null;
        }
        public Ticket(TaskCompletionSource taskSource)
        {
            IsAccept = true;
            this.taskSource = taskSource;
        }
        #endregion

        #region Method
        /// <summary>
        /// Releases the TicketLock object.
        /// </summary>
        public readonly void Release()
        {
            taskSource?.TrySetResult();
        }
        #endregion
    }
}
