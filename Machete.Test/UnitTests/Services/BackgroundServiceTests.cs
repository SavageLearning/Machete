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
            
            _recurringBackgroundService.DelayMinutes = 0;
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

            // act
            await _recurringBackgroundService.StartAsync(CancellationToken.None);
            await Task
                .Delay(10000);

            //assert
            Assert.IsTrue(_recurringBackgroundService.Executed);

            // cleanup
            await _recurringBackgroundService.StopAsync(_cts.Token);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public async Task Should_Not_Start()
        {
             // Arrange
            _workerActions
                .Setup(x => x.Execute())
                .Returns(true);

            // act
            await _recurringBackgroundService.StartAsync(_cts.Token);
            await Task
                .Delay(((int)_recurringBackgroundService.DelayMinutes) * 60000);
            
            //assert
            Assert.IsFalse(_recurringBackgroundService.Executed);
        }
    }
}
