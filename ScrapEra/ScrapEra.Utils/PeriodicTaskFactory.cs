using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapEra.Utils
{
    public class PeriodicTaskFactory
    {
        public static Task Start(Action action,
            int intervalInMilliseconds = Timeout.Infinite,
            int delayInMilliseconds = 0,
            int duration = Timeout.Infinite,
            int maxIterations = -1,
            bool synchronous = false,
            CancellationToken cancelToken = new CancellationToken(),
            TaskCreationOptions periodicTaskCreationOptions = TaskCreationOptions.None)
        {
            var stopWatch = new Stopwatch();
            Action wrapperAction = () =>
            {
                CheckIfCancelled(cancelToken);
                action();
            };

            Action mainAction =
                () =>
                {
                    MainPeriodicTaskAction(intervalInMilliseconds, delayInMilliseconds, duration, maxIterations,
                        cancelToken, stopWatch, synchronous, wrapperAction, periodicTaskCreationOptions);
                };

            return Task.Factory.StartNew(mainAction, cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        private static void MainPeriodicTaskAction(int intervalInMilliseconds,
            int delayInMilliseconds,
            int duration,
            int maxIterations,
            CancellationToken cancelToken,
            Stopwatch stopWatch,
            bool synchronous,
            Action wrapperAction,
            TaskCreationOptions periodicTaskCreationOptions)
        {
            var subTaskCreationOptions = TaskCreationOptions.AttachedToParent | periodicTaskCreationOptions;

            CheckIfCancelled(cancelToken);

            if (delayInMilliseconds > 0)
            {
                Thread.Sleep(delayInMilliseconds);
            }

            if (maxIterations == 0)
            {
                return;
            }

            var iteration = 0;

            using (var periodResetEvent = new ManualResetEventSlim(false))
            {
                while (true)
                {
                    CheckIfCancelled(cancelToken);

                    var subTask = Task.Factory.StartNew(wrapperAction, cancelToken, subTaskCreationOptions,
                        TaskScheduler.Current);

                    if (synchronous)
                    {
                        stopWatch.Start();
                        try
                        {
                            subTask.Wait(cancelToken);
                        }
                        catch
                        {
                            Console.WriteLine();
                        }
                        stopWatch.Stop();
                    }

                    if (intervalInMilliseconds == Timeout.Infinite)
                    {
                        break;
                    }

                    // iteration++;

                    if (maxIterations > 0 && iteration >= maxIterations)
                    {
                        break;
                    }

                    try
                    {
                        stopWatch.Start();
                        periodResetEvent.Wait(intervalInMilliseconds, cancelToken);
                        stopWatch.Stop();
                    }
                    finally
                    {
                        periodResetEvent.Reset();
                    }

                    CheckIfCancelled(cancelToken);

                    if (duration > 0 && stopWatch.ElapsedMilliseconds >= duration)
                    {
                        break;
                    }
                }
            }
        }

        private static void CheckIfCancelled(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
                throw new ArgumentNullException("cancellationToken");

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}