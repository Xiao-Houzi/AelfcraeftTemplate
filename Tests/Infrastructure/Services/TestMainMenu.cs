using Godot;
using NUnit.Framework;
using Aelfcraeft.GameStates;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.Tests.Infrastructure.Services
{
    [TestFixture]
    public class TestMainMenu
    {
        private MainMenu _mainMenu;
        private MockMessengerService _mockMessengerService;

        [SetUp]
        public void SetUp()
        {
            _mockMessengerService = new MockMessengerService();
            BaseService.Services.AddService(_mockMessengerService);

            _mainMenu = GD.Load<PackedScene>("res://Assets/GameStates/MainMenu.tscn").Instantiate<MainMenu>();
            Assert.That(_mainMenu, Is.Not.Null, "MainMenu scene could not be instantiated.");
        }

        [Test]
        public void TestNewGameButton()
        {
            var newGameButton = _mainMenu.GetNode<Button>("VBoxContainer/NewGameButton");
            Assert.That(newGameButton, Is.Not.Null, "New Game button not found.");

            bool messageSent = false;
            _mockMessengerService.StartGame += () => messageSent = true;

            newGameButton.EmitSignal("pressed");

            Assert.That(messageSent, Is.True, "StartGame message was not sent when New Game button was pressed.");
        }

        [Test]
        public void TestOptionsButton()
        {
            var optionsButton = _mainMenu.GetNode<Button>("VBoxContainer/OptionsButton");
            Assert.That(optionsButton, Is.Not.Null, "Options button not found.");

            bool messageSent = false;
            _mockMessengerService.OpenOptions += () => messageSent = true;

            optionsButton.EmitSignal("pressed");

            Assert.That(messageSent, Is.True, "OpenOptions message was not sent when Options button was pressed.");
        }

        [Test]
        public void TestExitButton()
        {
            var exitButton = _mainMenu.GetNode<Button>("VBoxContainer/ExitButton");
            Assert.That(exitButton, Is.Not.Null, "Exit button not found.");

            bool applicationQuit = false;
            _mockMessengerService.ExitGame += () => applicationQuit = true;

            exitButton.EmitSignal("pressed");

            Assert.That(applicationQuit, Is.True, "ExitGame message was not sent when Exit button was pressed.");
        }
    }
}