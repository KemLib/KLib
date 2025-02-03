namespace KConcurrent.Locker
{
    public readonly struct Ticket
    {
        #region Properties
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
        public readonly void Release()
        {
            taskSource?.TrySetResult();
        }
        #endregion
    }
}
