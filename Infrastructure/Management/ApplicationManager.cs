using Godot;
using System;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.Infrastructure.Management // Added namespace
{
    public partial class ApplicationManager : Node
    {
        #region Public API

        public ApplicationManager()
        {
            // Initialize the base service collection
            BaseService.Initialize(new ServiceCollection());
        }

        public override void _Ready()
        {
            base._Ready();

            // Initialize all required services
            InitialiseServices();

            // Retrieve the input service and set up default bindings
            _inputService = BaseService.Services.GetService<InputService>();
            SetupDefaultBindings();

            // Locate the BaseScene node and set it in the state storage
            var baseScene = GetTree().Root.GetNodeOrNull("BaseScene");
            _logService.Log($"ApplicationManager: BaseScene node found: {baseScene != null}");
            if (baseScene != null)
            {
                _stateStorage.SetState("CurrentScene", baseScene);
                _logService.Log("BaseScene set as current scene in state storage.");
            }

            // Add the SceneManager as a child node
            var sceneManager = new SceneManager();
            AddChild(sceneManager);

            // Subscribe to the ExitGame event
            _messengerService.ExitGame += OnExitGame;
        }

        public override void _ExitTree()
        {
            // Save the application state on exit
            _stateStorage.SaveState();
        }

        #endregion

        #region Private Implementation

        private void InitialiseServices()
        {
            // Initialize and register services in dependency order
            BaseService.Services.AddService(_logService = new LogService());
            BaseService.Services.AddService(_configService = new ConfigService());
            _configService.LoadConfig("Data/config.txt");
            _configService.LoadOptions();

            BaseService.Services.AddService(_stateStorage = new StateStorageService());
            _stateStorage.Initialize();
            _stateStorage.SetState("GameStateFolder", "Assets/GameStates");

            BaseService.Services.AddService(_messengerService = new MessengerService());
            BaseService.Services.AddService(_assetLoaderService = new AssetLoaderService());
            _assetLoaderService.InstantiateSplashImmediately();

            BaseService.Services.AddService(new InputService(new MockInputMap(), MockInput.IsActionPressed));
        }

        private void SetupDefaultBindings()
        {
            // Set up default input bindings
            _inputService.BindKeyToAction("move_up", "W");
            _inputService.BindKeyToAction("move_down", "S");
            _inputService.BindKeyToAction("move_left", "A");
            _inputService.BindKeyToAction("move_right", "D");
            _inputService.BindKeyToAction("jump", "Space");
        }

        private void OnExitGame()
        {
            // Handle the ExitGame event
            _logService.Log("ExitGame message received. Exiting application.");
            GetTree().Quit();
        }

        private StateStorageService _stateStorage;
        private LogService _logService;
        private AssetLoaderService _assetLoaderService;
        private MessengerService _messengerService;
        private ConfigService _configService;
        private InputService _inputService;
        private readonly IServiceProvider _serviceProvider = null; // Removed dependency injection

        #endregion
    }
}