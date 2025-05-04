using System;
using System.Collections.Generic;
using Survival.Infrastructure.Services; // Add this to reference LogService

public partial class MessengerService : BaseService
{
    public class Args : IArguments
    {
        public string Message { get; set; }
        public object Sender { get; set; }
    }

    public MessengerService()
    {
        // Initialize the message queue
        messageQueue = new Queue<Message>();
        LinkMessages();
    }

    private readonly object queueLock = new object();

    public void ProcessMessages()
    {
        lock (queueLock)
        {
            while (messageQueue.Count > 0)
            {
                var message = messageQueue.Dequeue();
                LogService.Log($"ProcessMessages: Processing message of type {message.MessageType}");
                Events[message.MessageType]?.Invoke();
            }
        }
    }

    public virtual void SendMessage(MessageType messageType)
    {
        lock (queueLock)
        {
            LogService.Log($"SendMessage: Enqueuing message of type {messageType}");
            messageQueue.Enqueue(new Message(messageType));
        }
    }

    public void SendMessage(MessageType messageType, IArguments args)
    {
        lock (queueLock)
        {
            LogService.Log($"SendMessage: Enqueuing message of type {messageType} with args");
            messageQueue.Enqueue(new Message(messageType, args));
        }
    }

    
    private Queue<Message> messageQueue = new Queue<Message>();
    private Dictionary<MessageType, Action> Events = new Dictionary<MessageType, Action>();

}
