using Machete.Service.BackgroundServices;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace Machete.Test.UnitTests.Services
{
    [TestClass]
    public class BackgroundServiceTests
    {
        private Mock<ILogger<RecurringBackgroundService>> _logger;
        private Mock<IWorkerActions> _workerActions;
        RecurringBackgroundService _recurringBackgroundService;
        CancellationTokenSource _cts;

        [TestInitialize]
        public void TestInitinialize()
        {
            _logger = new Mock<ILogger<RecurringBackgroundService>>();
            _workerActions = new Mock<IWorkerActions>();

            _recurringBackgroundService =
                new RecurringBackgroundService(
                    _logger.Object,
                    _workerActions.Object);

            _logger
                .Setup(x => x.BeginScope<RecurringBackgroundService>(_recurringBackgroundService));
            
            _recurringBackgroundService.DelayMinutes = 1;
            _recurringBackgroundService.RecurringMinutes = 1;

            _cts = new CancellationTokenSource();
        }


        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public async Task Should_Execute_Start()
        {
            // Arrange
            _workerActions
                .Setup(x => x.Execute())
                .Returns(true);

            Assert.IsTrue(_recurringBackgroundService.State == HostedBackgroundServiceState.NotStarted);
            
            // act
            await _recurringBackgroundService.StartAsync(CancellationToken.None);

            //assert
            Assert.IsTrue(_recurringBackgroundService.State == HostedBackgroundServiceState.Started);

            // cleanup
            await _recurringBackgroundService.StopAsync(_cts.Token);
        }

        ///
        /// Testing that .Net Core IHostedService does what it promises 
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public async Task Should_Start_Run_and_Stop()
        {
             // Arrange
            _workerActions
                .Setup(x => x.Execute())
                .Returns(true);
            
            //assert
            Assert.IsTrue(_recurringBackgroundService.State == HostedBackgroundServiceState.NotStarted);
            
            // act
            await _recurringBackgroundService.StartAsync(CancellationToken.None);
            
            //assert
            Assert.IsTrue(_recurringBackgroundService.State == HostedBackgroundServiceState.Started);
            
            await Task
                .Delay(((int)_recurringBackgroundService.DelayMinutes) * 62000); // delay until slightly after the first iteration
            
            //assert
            Assert.IsTrue(_recurringBackgroundService.State == HostedBackgroundServiceState.RanOnce);
            
            //act
            await _recurringBackgroundService.StopAsync(CancellationToken.None);
            await Task.Delay(100);
            
            //assert
            Assert.IsTrue(_recurringBackgroundService.State == HostedBackgroundServiceState.Stopped);
        }
    }
}
