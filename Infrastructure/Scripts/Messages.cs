using System;
using System.Collections.Generic;
using Survival.Infrastructure.Services;

public partial class MessengerService : BaseService
{
    public event Action SplashLoadingComplete;
    public event Action StartupDelayComplete;
    public event Action WelcomeKeyPress;
    public event Action ExitGame;
    public event Action AssetsPreloaded;
    public event Action SplashInstantiated;
    public event Action Initialised;
    public event Action StartGame;
    public event Action OpenOptions;
    public event Action MissionStageChanged;

   
    public enum MessageType
    {
        None,
        SplashLoadingComplete,
        StartupDelayComplete,
        WelcomeKeyPress,
        ExitGame,
        AssetsPreloaded,
        SplashInstantiated,
        Initialised,
        StartGame,
        OpenOptions,
        MissionStageChanged
    }

    public void LinkMessages()
    {
        Events = new Dictionary<MessageType, Action>
        {
            { MessageType.SplashLoadingComplete,    () => { SplashLoadingComplete?.Invoke(); } },
            { MessageType.StartupDelayComplete,     () => { StartupDelayComplete?.Invoke(); } },
            { MessageType.AssetsPreloaded,          () => { AssetsPreloaded?.Invoke(); } },
            { MessageType.WelcomeKeyPress,          () => { WelcomeKeyPress?.Invoke(); } },
            { MessageType.ExitGame,                 () => { ExitGame?.Invoke(); } },
            { MessageType.SplashInstantiated,       () => { SplashInstantiated?.Invoke(); } },
            { MessageType.Initialised,              () => { Initialised?.Invoke(); } },
            { MessageType.StartGame,                () => { StartGame?.Invoke(); } },
            { MessageType.OpenOptions,              () => { OpenOptions?.Invoke(); } },
            { MessageType.MissionStageChanged,      () => { MissionStageChanged?.Invoke(); } }
        };
    }
}