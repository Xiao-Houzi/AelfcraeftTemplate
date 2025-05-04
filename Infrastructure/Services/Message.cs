using System;

public interface IArguments
{
    // Define any methods or properties needed for arguments
}

public class EmptyArguments : IArguments
{
    // This class represents an empty implementation of the IArguments interface.
}

class Message
{
    public MessengerService.MessageType MessageType { get; set; }
    public IArguments Arguments { get; set; }
    public DateTime Timestamp { get; set; }

    public Message(MessengerService.MessageType type, IArguments arguments)
    {
        MessageType = type;
        Arguments = arguments;
        Timestamp = DateTime.Now;
    }

    public Message(MessengerService.MessageType type)
    {
        MessageType = type;
        Arguments = new EmptyArguments(); // Use EmptyArguments instead of null
        Timestamp = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Timestamp}: - {MessageType}";
    }
}