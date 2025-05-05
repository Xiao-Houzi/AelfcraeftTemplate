using System;
using NUnit.Framework;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.Tests.Infrastructure.Services
{
    [TestFixture]
    public class MessengerServiceTests
    {
        private MessengerService _messengerService;

        [SetUp]
        public void SetUp()
        {
            _messengerService = new MessengerService();
        }

        [Test]
        public void SendMessage_WelcomeKeyPress_InvokesEvent()
        {
            // Arrange
            bool eventInvoked = false;
            _messengerService.WelcomeKeyPress += () => eventInvoked = true;

            // Act
            _messengerService.SendMessage(MessengerService.MessageType.WelcomeKeyPress);

            // Assert
            Assert.That(eventInvoked, Is.True, "WelcomeKeyPress event was not invoked.");
        }
    }
}