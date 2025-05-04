using Godot;
using System;
using Survival.Infrastructure.Services; // Add this to reference StateStorageService

public partial class ApplicationManager : Node
{
    #region Public API

   
    public ApplicationManager()
    {
        BaseService.Initialize(new ServiceCollection());

    }

    public override void _Ready()
    {
        base._Ready();

        InitialiseServices();
        
        _inputService = BaseService.Services.GetService<InputService>();
        SetupDefaultBindings();

        var baseScene = GetTree().Root.GetNode("BaseScene");
        _logService.Log($"ApplicationManager: BaseScene node found: {baseScene != null}");
        if (baseScene != null)
        {       
            _stateStorage.SetState("CurrentScene", baseScene);
            _logService.Log("BaseScene set as current scene in state storage.");
        }
        

        var sceneManager = new SceneManager();
        AddChild(sceneManager);
        
        _messengerService.ExitGame += () =>
        {
            _logService.Log("ExitGame message received. Exiting application.");
            GetTree().Quit();
        };
    }

    public override void _ExitTree()
    {
        _stateStorage.SaveState();
    }

    #endregion

    #region Private Implementation

    private void InitialiseServices()
    {
        // Initialize and add SceneManager as a child
        BaseService.Services.AddService(_logService= new LogService());
        BaseService.Services.AddService(_messengerService= new MessengerService());   
        BaseService.Services.AddService(_configService= new ConfigService());
        _configService.LoadConfig("Data/config.txt");
        _configService.LoadOptions();
        BaseService.Services.AddService(_stateStorage = new StateStorageService());
        _stateStorage.LoadState();
        BaseService.Services.AddService(_assetLoaderService= new AssetLoaderService());
        _assetLoaderService.InstantiateSplashImmediately();
        BaseService.Services.AddService(new InputService(new MockInputMap(), MockInput.IsActionPressed));
    }

    private void SetupDefaultBindings()
    {
        _inputService.BindKeyToAction("move_up", "W");
        _inputService.BindKeyToAction("move_down", "S");
        _inputService.BindKeyToAction("move_left", "A");
        _inputService.BindKeyToAction("move_right", "D");
        _inputService.BindKeyToAction("jump", "Space");
    }

    private StateStorageService _stateStorage;
    private LogService _logService;
    private AssetLoaderService _assetLoaderService;
    private MessengerService _messengerService;
    private ConfigService _configService;
    private InputService _inputService;

    #endregion
}