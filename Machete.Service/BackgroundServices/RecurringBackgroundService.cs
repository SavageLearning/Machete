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

        private double _delayMinutes;
        private double _recurringMinutes;
        private bool _executed;
        public double DelayMinutes { get => _delayMinutes; set => _delayMinutes = value; }
        public double RecurringMinutes { get => _recurringMinutes; set => _recurringMinutes = value; }
        public bool Executed { get => _executed; set => _executed = value; }


        public RecurringBackgroundService(
            ILogger<RecurringBackgroundService> logger,
            IWorkerActions workerActions)
        {
            _logger = logger;
            _workerActions = workerActions;

            // When to first run the task
            //DateTime.Today = midnight 00.00, setting it to 6:00 am UTC (~11pm PST, ~2am NYC)
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(6);
            var currTime = DateTime.Now;
	        var firstInterval = nextRunTime.Subtract(currTime).TotalMinutes;

            _delayMinutes = firstInterval;
            // recurring daily
            _recurringMinutes = 24 * 60;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executed = false;
            try
            {
                _timer = new Timer(
                    x => _executed = _workerActions.Execute(), // callback
                    null, // object state
                    TimeSpan.FromMinutes(_delayMinutes), // delayByMinutes
                    TimeSpan.FromMinutes(_recurringMinutes) // interval
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
            _logger.LogInformation($"Timed Hosted Service {nameof(RecurringBackgroundService)} is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
