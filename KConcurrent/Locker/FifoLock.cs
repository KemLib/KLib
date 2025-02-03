﻿namespace KConcurrent.Locker
{
    public class FifoLock : IDisposable
    {
        #region Properties
        private readonly Queue<TaskCompletionSource<bool>> queueTask;
        private SpinLock lockObject;
        private InterValueBool isLock,
            isDisposed;

        public bool IsLock => isLock;
        public bool IsDisposed => isDisposed;
        #endregion

        #region Construction
        public FifoLock()
        {
            queueTask = new();
            lockObject = new();
            isLock = new();
            isDisposed = new();
        }
        ~FifoLock()
        {
            Dispose(false);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!isDisposed.TryExchange(true))
                return;
            //
            if (disposing)
            {

            }
            List<TaskCompletionSource<bool>> taskSources = [];
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    taskSources.AddRange([.. queueTask]);
                    queueTask.Clear();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
            //
            foreach (var taskSource in taskSources)
                taskSource.TrySetResult(false);
        }
        #endregion

        #region Wait 
        public bool Wait()
        {
            if (!TryEnqueue(out TaskCompletionSource<bool>? taskSource))
                return false;
            //
            if (taskSource == null)
                return true;
            //
            taskSource.Task.Wait();
            return taskSource.Task.Result;
        }
        public async Task<bool> WaitAsync()
        {
            if (!TryEnqueue(out TaskCompletionSource<bool>? taskSource))
                return false;
            //
            if (taskSource == null)
                return true;
            //
            await taskSource.Task;
            return taskSource.Task.Result;
        }
        public async Task<bool> WaitAsync(CancellationToken cancellationToken = default)
        {
            if (!TryEnqueue(out TaskCompletionSource<bool>? taskSource))
                return false;
            //
            if (taskSource == null)
                return true;
            //
            try
            {
                return await taskSource.Task.WaitAsync(cancellationToken);
            }
            catch (Exception)
            {
                if (!taskSource.TrySetResult(false) && taskSource.Task.IsCompletedSuccessfully && taskSource.Task.Result)
                    Release();
                return false;
            }
        }
        private bool TryEnqueue(out TaskCompletionSource<bool>? taskSource)
        {
            if (IsDisposed)
            {
                taskSource = null;
                return false;
            }
            //
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    if (IsDisposed)
                    {
                        taskSource = null;
                        return false;
                    }
                    //
                    if (isLock.Value)
                    {
                        taskSource = new();
                        queueTask.Enqueue(taskSource);
                    }
                    else
                    {
                        taskSource = null;
                        isLock.Value = true;
                    }
                    return true;
                }
                else
                {
                    taskSource = null;
                    return false;
                }
            }
            catch (Exception)
            {
                taskSource = null;
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        #endregion

        #region Release
        public void Release()
        {
            if (IsDisposed)
                return;
            //
            TaskCompletionSource<bool>? taskSource;
            do
            {
                bool lockToken = false;
                try
                {
                    lockObject.Enter(ref lockToken);
                    if (lockToken)
                    {
                        if (IsDisposed)
                        {
                            isLock.Value = false;
                            return;
                        }
                        //
                        if (!queueTask.TryDequeue(out taskSource))
                            isLock.Value = false;
                    }
                    else
                    {
                        isLock.Value = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    isLock.Value = false;
                    return;
                }
                finally
                {
                    if (lockToken)
                        lockObject.Exit();
                }
                //
                if (taskSource == null || taskSource.TrySetResult(true))
                    return;
            } while (true);
        }
        #endregion
    }
}
