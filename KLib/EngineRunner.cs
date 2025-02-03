namespace KLib
{
    public static class EngineRunner
    {
        #region Properties
        private const string ERROR_THREAD_RUN_FAIL = "The thread start fail: ",
            ERROR_TASK_RUN_FAIL = "The task start fail: ";
        public const float DEFAULT_FPS = 30;
        private const int MAX_DELAY_TIME = 1000;
        #endregion

        #region Method
        public static Result StartThread(IEngine engine, float fps = DEFAULT_FPS)
        {            
            if (!engine.IsRun)
            {
                Result result = engine.Start();
                if (!result.IsSuccess)
                    return result;
            }
            //
            try
            {
                ThreadStart threadStart = new(() =>
                {
                    if (fps > 0)
                        Run(engine, fps);
                    else
                        Run(engine);
                });
                Thread thread = new(threadStart);
                thread.Start();
                //
                return new Result();
            }
            catch (Exception ex)
            {
                engine.Stopped();
                //
                return new Result(ERROR_THREAD_RUN_FAIL + ex.Message);
            }
        }
        public static Result StartTask(IEngine engine, float fps = DEFAULT_FPS, CancellationToken cancellationToken = default)
        {
            if (!engine.IsRun)
            {
                Result result = engine.Start();
                if (!result.IsSuccess)
                    return result;
            }
            //
            try
            {
                Task.Run(async () =>
                {
                    if (fps > 0)
                        _ = await RunAsync(engine, fps, cancellationToken);
                    else
                        _ = await RunAsync(engine, cancellationToken);
                });
                //
                return new Result();
            }
            catch (Exception ex)
            {
                engine.Stopped();
                //
                return new Result(ERROR_TASK_RUN_FAIL + ex.Message);
            }
        }
        #endregion

        #region Run
        public static Result Run(IEngine engine, float fps)
        {
            DateTime currentTime;
            if (!engine.IsRun)
            {
                currentTime = DateTime.Now;
                Result result = engine.Start();
                if (!result.IsSuccess)
                    return result;
            }
            else
                currentTime = DateTime.MinValue;
            //
            double minDeltaTime = 1000 / fps;
            SpinWait spinWait = new();
            while (engine.IsRun)
            {
                if (engine.IsStopping)
                {
                    engine.Stopped();
                    continue;
                }
                //
                DateTime nowTime = DateTime.Now;
                double deltaTime = (nowTime - currentTime).TotalMilliseconds,
                    delayTime = minDeltaTime - deltaTime;
                if (delayTime > 0)
                {
                    try
                    {
                        Thread.Sleep(Math.Min((int)delayTime, MAX_DELAY_TIME));
                    }
                    catch (Exception)
                    {

                    }
                    continue;
                }
                currentTime = nowTime;
                //
                spinWait.SpinOnce();
                engine.Update();
            }
            //
            return new Result();
        }
        public static Result Run(IEngine engine)
        {
            if (!engine.IsRun)
            {
                Result result = engine.Start();
                if (!result.IsSuccess)
                    return result;
            }
            //
            SpinWait spinWait = new();
            while (engine.IsRun)
            {
                if (engine.IsStopping)
                {
                    engine.Stopped();
                    continue;
                }
                //
                spinWait.SpinOnce();
                engine.Update();
            }
            //
            return new Result();
        }
        public static async Task<Result> RunAsync(IEngine engine, float fps, CancellationToken cancellationToken = default)
        {
            DateTime currentTime;
            if (!engine.IsRun)
            {
                currentTime = DateTime.Now;
                Result result = await engine.StartAsync();
                if (!result.IsSuccess)
                    return result;
            }
            else
                currentTime = DateTime.MinValue;
            //
            double minDeltaTime = 1000 / fps;
            while (engine.IsRun)
            {
                if (engine.IsStopping || cancellationToken.IsCancellationRequested)
                {
                    await engine.StoppedAsync();
                    continue;
                }
                //
                DateTime nowTime = DateTime.Now;
                double deltaTime = (nowTime - currentTime).TotalMilliseconds,
                    delayTime = minDeltaTime - deltaTime;
                if (delayTime > 0)
                {
                    try
                    {
                        await Task.Delay(Math.Min((int)delayTime, MAX_DELAY_TIME), cancellationToken);
                    }
                    catch (Exception)
                    {

                    }
                    continue;
                }
                currentTime = nowTime;
                //
                await engine.UpdateAsync();
            }
            //
            return new Result();
        }
        public static async Task<Result> RunAsync(IEngine engine, CancellationToken cancellationToken = default)
        {
            if (!engine.IsRun)
            {
                Result result = await engine.StartAsync();
                if (!result.IsSuccess)
                    return result;
            }
            //
            while (engine.IsRun)
            {
                if (engine.IsStopping || cancellationToken.IsCancellationRequested)
                {
                    await engine.StoppedAsync();
                    continue;
                }
                //
                await engine.UpdateAsync();
            }
            //
            return new Result();
        }
        #endregion
    }
}
