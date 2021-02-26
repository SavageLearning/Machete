using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Machete.Service.BackgroundServices
{
    public class RecurringBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<RecurringBackgroundService> _logger;
        private IWorkerActions _workerActions;
        private Timer _timer;
        public HostedBackgroundServiceState State { get; set; }

        public double DelayMinutes { get; set; }
        public double RecurringMinutes { get; set; }


        public RecurringBackgroundService(
            ILogger<RecurringBackgroundService> logger,
            IWorkerActions workerActions)
        {
            _logger = logger;
            _workerActions = workerActions;
            
            // State
            State = HostedBackgroundServiceState.NotStarted;

            // When to first run the task setting it to 6:00 am UTC (~11pm PST, ~2am NYC)
            var nextRunTime = DateTime
                                .Today
                                // Today at 00.00 is always x hours ago, so add 1 day to go to the next day at 00:00
                                .AddHours(24)
                                .AddHours(6); // Then set it to 6:00 UTC of the next day.
            var currTime = DateTime.Now;
	        var firstInterval = nextRunTime.Subtract(currTime).TotalMinutes;

            DelayMinutes = firstInterval;
            // recurring daily
            RecurringMinutes = 24 * 60;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            State = HostedBackgroundServiceState.Started;
            try
            {
                _timer = new Timer(
                    x =>
                    {
                        _workerActions.Execute();
                        if (State != HostedBackgroundServiceState.RanOnce) 
                            State = HostedBackgroundServiceState.RanOnce;
                    }, // callback
                    null, // object state
                    TimeSpan.FromMinutes(DelayMinutes), // delayByMinutes
                    TimeSpan.FromMinutes(RecurringMinutes) // interval
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to execute background task: " + ex.Message + ex.InnerException);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            State = HostedBackgroundServiceState.Stopped;
            _logger.LogInformation($"Timed Hosted Service {nameof(RecurringBackgroundService)} is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

    public enum HostedBackgroundServiceState
    {
        NotStarted,
        Started,
        Stopped,
        RanOnce
    }
}
