using NUnit.Framework;
using Aelfcraeft.Infrastructure.Services;
using Aelfcraeft.Infrastructure.Management;

namespace Aelfcraeft.Tests.Infrastructure.Scripts
{
    [TestFixture]
    public class SceneManagerTests
    {
        private MockLogService _mockLogService;
        private MockMessengerService _mockMessengerService;
        private SceneManager _sceneManager;

        [SetUp]
        public void SetUp()
        {
            // Initialize the ServiceCollection
            BaseService.Initialize(new ServiceCollection());

            _mockLogService = new MockLogService();
            _mockMessengerService = new MockMessengerService();

            BaseService.Services.AddService(_mockLogService);
            BaseService.Services.AddService(_mockMessengerService);
            BaseService.Services.AddService(new AssetLoaderService());
            BaseService.Services.AddService(new StateStorageService());

            _sceneManager = new SceneManager();
            _sceneManager._Ready();
        }

        [Test]
        public void CheckAndLoadWelcomeScene_BothConditionsMet_ChangesToWelcomeScene()
        {
            // Arrange
            _sceneManager.GetType().GetField("isLoadingComplete", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_sceneManager, true);
            _sceneManager.GetType().GetField("isStartupDelayComplete", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_sceneManager, true);

            // Act
            _sceneManager.GetType().GetMethod("CheckAndLoadWelcomeScene", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(_sceneManager, null);

            // Assert
            Assert.That(_mockLogService.LoggedMessages.Exists(msg => msg.Contains("Loading Welcome scene")), Is.True, "Welcome scene was not loaded as expected.");
        }
    }
}