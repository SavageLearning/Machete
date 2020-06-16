using Machete.Service.BackgroundServices;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;
        RecurringBackgroundService _cronJobHostedService;

        [TestInitialize]
        public void TestInitinialize()
        {
            _logger = new Mock<ILogger<RecurringBackgroundService>>();
            _workerActions = new Mock<IWorkerActions>();

            _cronJobHostedService =
                new RecurringBackgroundService(
                    _logger.Object,
                    _workerActions.Object);

            _logger
                .Setup(x => x.BeginScope<RecurringBackgroundService>(_cronJobHostedService));
        }


        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Service), TestCategory(TC.Workers)]
        public async Task Should_Execute_Start()
        {
            // Arrange
            _workerActions
                .Setup(x => x.Execute())
                .Verifiable();
            _cronJobHostedService.DelayMinutes = 0;
            _cronJobHostedService.RecurringMinutes = 1;

            // act
            await _cronJobHostedService.StartAsync(CancellationToken.None);
            await Task
                .Delay((_cronJobHostedService.DelayMinutes) * 60000);

            //assert
            Assert.IsTrue(_cronJobHostedService.Executed);

            // cleanup
            await _cronJobHostedService.StopAsync(CancellationToken.None);
        }
    }
}
