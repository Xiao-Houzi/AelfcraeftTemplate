using Godot;
using System;

public partial class Splash : BaseGameState
{
    private double elapsedDelta = 0;
    private bool loadingCompleteMessageSent = false;

    public Splash()
    {
        freeable = true;
    }

    public override void _Ready()
    {
        base._Ready(); // Ensure the base class initialization is called
        if (Services == null)
        {
            LogService.LogErr("Services object is not initialized. Ensure it is set before using.");
        }
        Services.GetService<MessengerService>().SendMessage(MessengerService.MessageType.SplashLoadingComplete);
    }

    public override void _Process(double delta)
    {
        
    }
}