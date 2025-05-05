using NUnit.Framework;
using Aelfcraeft.Infrastructure.Services;
using Aelfcraeft.Infrastructure.Management;
using Moq;

namespace Aelfcraeft.Tests.Infrastructure.Services
{
    [TestFixture]
    public class StateStorageServiceTests
    {
        private StateStorageService _stateStorageService;
        private Mock<ILogService> _mockLogService;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();

            var mockServiceCollection = new Mock<ServiceCollection>();
            mockServiceCollection.Setup(sc => sc.GetService<ILogService>()).Returns(_mockLogService.Object);

            BaseService.Initialize(mockServiceCollection.Object);

            _stateStorageService = new StateStorageService();
        }

        [Test]
        public void LoadStateFromConfig_ConfigServiceIsNull_LogsError()
        {
            // Arrange
            var expectedMessage = "ConfigService is not initialized. Cannot load state from config.";

            // Act
            _stateStorageService.Initialize();

            // Assert
            _mockLogService.Verify(
                log => log.LogErr(It.Is<string>(msg => msg == expectedMessage), It.IsAny<string>(), It.IsAny<int>()),
                Times.Once);
        }
    }
}