using Godot;
using System;
using Survival.Infrastructure.Services; // Add this to reference LogService

public partial class MainMenu : BaseGameState
{
    public override void _Ready()
    {
        // Add initialization logic here
    }

    protected override void OnInitialised()
    {
        // Perform any additional initialization specific to MainMenu
    }

private void OnStartButtonPressed()
    {
        Services.GetService<LogService>().Log("ExitButton pressed. Sending ExitGame message.");
        Services.GetService<MessengerService>().SendMessage(MessengerService.MessageType.StartGame);
    }

    private void OnOptionsButtonPressed()
    {
        Services.GetService<LogService>().Log("ExitButton pressed. Sending ExitGame message.");
        Services.GetService<MessengerService>().SendMessage(MessengerService.MessageType.OpenOptions);
    }

    private void OnExitButtonPressed()
    {
        Services.GetService<LogService>().Log("ExitButton pressed. Sending ExitGame message.");
        Services.GetService<MessengerService>().SendMessage(MessengerService.MessageType.ExitGame);
    }

}