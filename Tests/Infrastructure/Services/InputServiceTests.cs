using Godot;
using NUnit.Framework;
using System.Collections.Generic;
using Survival.Infrastructure.Services; // Added to reference InputService

namespace Survival.Tests.Infrastructure.Services
{
    [TestFixture]
    public class InputServiceTests
    {
        private InputService _inputService;
        private MockInputMap _mockInputMap;

        [SetUp]
        public void SetUp()
        {
            _mockInputMap = new MockInputMap();
            _inputService = new InputService(_mockInputMap, MockInput.IsActionPressed);

            // Use the mock InputMap
            _mockInputMap.AddAction("test_action");
        }

        [Test]
        public void TestBindKeyToAction()
        {
            _inputService.BindKeyToAction("test_action", "A"); // Updated to pass a string instead of Key.A
            string boundKey = _inputService.GetKeyForAction("test_action");

            Assert.That(boundKey, Is.EqualTo("A"));
        }

        [Test]
        public void TestIsActionPressed()
        {
            MockInput.SimulateActionPress("test_action"); // Simulate action press
            Assert.That(_inputService.IsActionPressed("test_action"), Is.True);

            MockInput.SimulateActionRelease("test_action"); // Simulate action release
            Assert.That(_inputService.IsActionPressed("test_action"), Is.False);
        }

        [Test]
        public void TestGetInputState()
        {
            _inputService.ProcessInput("MouseButton_1", true); // Replaced _Input with ProcessInput
            Assert.That(_inputService.GetInputState("MouseButton_1"), Is.True);

            _inputService.ProcessInput("MouseButton_1", false); // Replaced _Input with ProcessInput
            Assert.That(_inputService.GetInputState("MouseButton_1"), Is.False);
        }

        [Test]
        public void TestProcessInput()
        {
            _inputService.ProcessInput("test_action", true);
            Assert.That(_inputService.GetInputState("test_action"), Is.True);

            _inputService.ProcessInput("test_action", false);
            Assert.That(_inputService.GetInputState("test_action"), Is.False);
        }
    }
}