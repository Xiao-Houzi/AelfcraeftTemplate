using Godot;
using Aelfcraeft.Infrastructure.Services;
using System;
using Aelfcraeft.Infrastructure.Management;

namespace Aelfcraeft.GameStates
{
    public abstract partial class BaseGameState : Node3D
    {
        protected static ServiceCollection Services;

        public static void Initialise(ServiceCollection services)
        {
            Services = services;
            LogService = services.GetService<LogService>();
            MessengerService = services.GetService<MessengerService>();
            StateStorageService = services.GetService<StateStorageService>();
            ConfigService = services.GetService<ConfigService>();
            AssetLoaderService = services.GetService<AssetLoaderService>();
            InputService = services.GetService<InputService>();
            MissionService = services.GetService<MissionService>();
        }

        public virtual void Initialise()
        {
            // Initialize the game state
            MessengerService?.SendMessage(MessengerService.MessageType.Initialised, new MessengerService.Args() { Sender = this });
        }

        protected virtual void OnInitialised()
        {
            // Optional override for derived classes to handle initialization
        }

        protected static LogService LogService { get; private set; }
        protected static MessengerService MessengerService { get; private set; }
        protected static StateStorageService StateStorageService { get; private set; }
        protected static AssetLoaderService AssetLoaderService { get; private set; }
        protected static InputService InputService { get; private set; }
        protected static MissionService MissionService { get; private set; }
        protected static ConfigService ConfigService { get; private set; }

        protected bool freeable = false;

        //////////
    }
}