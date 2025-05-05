using System;
using Aelfcraeft.Infrastructure.Services;

public class MockMessengerService : MessengerService
{
    public new event Action StartGame;
    public new event Action OpenOptions;

    public override void SendMessage(MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.StartGame:
                StartGame?.Invoke();
                break;
            case MessageType.OpenOptions:
                OpenOptions?.Invoke();
                break;
            default:
                base.SendMessage(messageType);
                break;
        }
    }
}