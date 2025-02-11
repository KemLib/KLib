using System.Threading.Tasks;

namespace KLibStandard.Concurrent.Locker
{
    public class TicketLock
    {
        #region Properties
        private InterValueClass<Task> currentTask;
        public bool IsLock
        {
            get
            {
                Task task = currentTask;
                return task != null && !task.IsCompleted;
            }
        }
        #endregion

        #region Construction
        public TicketLock()
        {
            currentTask = new InterValueClass<Task>();
        }
        #endregion

        #region Wait
        /// <summary>
        /// Blocks the current thread until it can enter the TicketLock.
        /// </summary>
        public Ticket Wait()
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
            Task task = currentTask.Exchange(taskSource.Task);
            if (task == null)
                return new Ticket(taskSource);
            //
            task.Wait();
            return new Ticket(taskSource);
        }
        /// <summary>
        /// Asynchronously waits to enter the TicketLock.
        /// </summary>
        public async Task<Ticket> WaitAsync()
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
            Task task = currentTask.Exchange(taskSource.Task);
            if (task == null)
                return new Ticket(taskSource);
            //
            await task;
            return new Ticket(taskSource);
        }
        #endregion
    }
}
