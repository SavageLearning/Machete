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

        private int _delayMinutes;
        private int _recurringMinutes;
        private bool _executed;
        public int DelayMinutes { get => _delayMinutes; set => _delayMinutes = value; }
        public int RecurringMinutes { get => _recurringMinutes; set => _recurringMinutes = value; }
        public bool Executed { get => _executed; set => _executed = value; }


        public RecurringBackgroundService(
            ILogger<RecurringBackgroundService> logger,
            IWorkerActions workerActions)
        {
            _logger = logger;
            _workerActions = workerActions;

            _delayMinutes = 0;
            _recurringMinutes = 1;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executed = false;
            try
            {
                _timer = new Timer(
                    x => _executed = _workerActions.Execute(), // callback
                    null, // object state
                    TimeSpan.FromMinutes(_delayMinutes), // delayTime
                    TimeSpan.FromMinutes(_recurringMinutes) // period
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
