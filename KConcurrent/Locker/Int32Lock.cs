namespace KConcurrent.Locker
{
    public struct Int32Lock
    {
        #region Properties
        private const int ENTER_WAIT_TIME = 1;

        private int valueA,
            valueB;
        #endregion

        #region Construction
        public Int32Lock()
        {
            valueA = 0;
            valueB = valueA + 1;
        }
        #endregion

        #region Method
        public void Enter()
        {
            int newValueA = Interlocked.Add(ref valueA, 1);
            if (newValueA == Interlocked.CompareExchange(ref valueB, 0, 0))
                return;
            //
            SpinWait spinWait = new();
            do
            {
                spinWait.SpinOnce();
            } while (newValueA != Interlocked.CompareExchange(ref valueB, 0, 0));
        }
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
        public void Exit()
        {
            Interlocked.Add(ref valueB, 1);
        }
        #endregion
    }
}
