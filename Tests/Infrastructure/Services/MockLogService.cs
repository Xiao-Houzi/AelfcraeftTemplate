using System;
using System.Collections.Generic;
using Aelfcraeft.Infrastructure.Services;

public class MockLogService : LogService
{
    public List<string> LoggedMessages { get; } = new List<string>();

    public override void Log(string message, string fileName = "", int lineNumber = 0)
    {
        LoggedMessages.Add(message);
    }
}