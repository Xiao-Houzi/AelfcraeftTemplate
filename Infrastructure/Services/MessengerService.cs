using System;
using System.Collections.Generic;

namespace Aelfcraeft.Infrastructure.Services
{
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

        public virtual void RegisterCustomMessageHandler(MessageType messageType, Action handler)
        {
            if (!Events.ContainsKey(messageType))
            {
                Events[messageType] = handler;
            }
            else
            {
                Events[messageType] += handler;
            }
        }

        private Queue<Message> messageQueue = new Queue<Message>();
        private Dictionary<MessageType, Action> Events = new Dictionary<MessageType, Action>();

    }
}
