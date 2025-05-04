using System;
using Survival.Infrastructure.Services;

public class MockMessengerService : MessengerService
{
    public event Action WelcomeKeyPressEvent;

    public override void SendMessage(MessageType messageType)
    {
        if (messageType == MessageType.WelcomeKeyPress)
        {
            WelcomeKeyPressEvent?.Invoke();
        }
    }
}