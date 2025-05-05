using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.Infrastructure.Management
{
    public partial class SceneManager : Node
    {
        #region Public API

        public SceneManager() : base()
        {
            SetProcess(true); // Enable processing for the SceneManager
        }

        public override void _Ready()
        {
            LogService = BaseService.Services.GetService<LogService>();
            AssetLoaderService = BaseService.Services.GetService<AssetLoaderService>();
            MessengerService = BaseService.Services.GetService<MessengerService>();

            LogService.Log("SceneManager initialized.");

            MessengerService.SplashLoadingComplete += OnSplashLoadingComplete;
            MessengerService.StartupDelayComplete += OnStartupDelayComplete;
            MessengerService.AssetsPreloaded += OnAssetsPreloaded;
            MessengerService.WelcomeKeyPress += OnWelcomeKeyPress;
            MessengerService.SplashInstantiated += Runsplash;
            MessengerService.StartGame += OnStartGame;
            MessengerService.OpenOptions += OnOpenOptions;

            LoadSceneConfiguration();
        }

        public override void _Process(double delta)
        {
            MessengerService.ProcessMessages();
        }

        #endregion

        #region Private Implementation

        private LogService LogService;
        private AssetLoaderService AssetLoaderService;
        private MessengerService MessengerService;

        private readonly string gameStateFolder = BaseService.StateStorage.GetReferenceType<string>("GameStateFolder");

        private bool isLoadingComplete = false;
        private bool isStartupDelayComplete = false;

        private Dictionary<string, string> _scenePaths;
        private Dictionary<string, bool> _freeableStates;

        private void LoadSceneConfiguration()
        {
            _scenePaths = new Dictionary<string, string>();
            _freeableStates = new Dictionary<string, bool>();

            var configKeys = BaseService.ConfigService.GetAllConfigKeys();
            foreach (var key in configKeys)
            {
                if (key.StartsWith("State."))
                {
                    var value = BaseService.ConfigService.GetConfigValue(key);
                    var stateParts = value.Split(",");
                    if (stateParts.Length == 2)
                    {
                        var scenePath = stateParts[0].Trim();
                        var freeable = stateParts[1].Trim().ToLower() == "true";

                        var stateName = key.Substring("State.".Length);
                        _scenePaths[stateName] = scenePath;
                        _freeableStates[stateName] = freeable;
                    }
                }
            }
        }

        public void PreloadScene(string sceneName)
        {
            if (_scenePaths.ContainsKey(sceneName))
            {
                var scenePath = _scenePaths[sceneName];
                var scene = GD.Load<PackedScene>(scenePath);
                if (scene != null)
                {
                    GD.Print($"Preloaded scene: {sceneName} from {scenePath}");
                }
                else
                {
                    GD.PrintErr($"Failed to preload scene: {sceneName} from {scenePath}");
                }
            }
            else
            {
                GD.PrintErr($"Scene name not found in configuration: {sceneName}");
            }
        }

        private async void Runsplash()
        {
            LogService.Log("Splash state active. Requesting asset preloading.");
            ChangeScene("Splash");

            await AssetLoaderService.PreloadAssets();

            // Send startup delay message
            await Task.Delay(1000); // Wait for 1 second
            MessengerService.SendMessage(MessengerService.MessageType.StartupDelayComplete);
        }

        private void CheckAndLoadWelcomeScene()
        {
            LogService.Log($"CheckAndLoadWelcomeScene: isLoadingComplete={isLoadingComplete}, isStartupDelayComplete={isStartupDelayComplete}");
            if (isLoadingComplete && isStartupDelayComplete)
            {
                LogService.Log("CheckAndLoadWelcomeScene: Both conditions met. Loading Welcome scene...");
                ChangeScene("Welcome");
            }
        }

        
        private void ChangeScene(string sceneName)
        {
            LogService.Log($"ChangeScene: Changing scene to {sceneName}");
            if (GetTree() == null)
            {
                LogService.LogErr("SceneManager is not yet part of the scene tree. Cannot change scene.");
                return;
            }

            var currentScene = BaseService.StateStorage.GetReferenceType<Node>("CurrentScene");
            var sceneInstance = AssetLoaderService.GetInstantiatedScene(sceneName);

            // Unload the current scene	
            if (currentScene.GetParent() == GetTree().Root)
            {
                // Use call_deferred to safely remove the child
                GetTree().Root.CallDeferred("remove_child", currentScene);
                LogService.Log($"Scene unloaded: {currentScene}");
            }
            else
            {
                LogService.LogErr($"Scene instance parent mismatch or null for: {currentScene}");
            }
            
            // Load the new scene
            if (sceneInstance != null)
            {
                // Use call_deferred to safely add the child
                GetTree().Root.CallDeferred("add_child", sceneInstance);
                LogService.Log($"Scene loaded and added to tree: {sceneInstance}");
                BaseService.StateStorage.SetState("CurrentScene", sceneInstance);
            }
            else
            {
                LogService.LogErr($"Failed to load scene: {sceneInstance}");
            }
        }

        private void OnLoadingComplete()
        {
            LogService.Log("OnLoadingComplete: Loading complete event received.");
            isLoadingComplete = true; // Set the flag to true
            CheckAndLoadWelcomeScene();
        }

        private void OnStartupDelayComplete()
        {
            LogService.Log("OnStartupDelayComplete: Startup delay complete message received.");
            isStartupDelayComplete = true; // Set the flag to true
            CheckAndLoadWelcomeScene();
        }

        private void OnWelcomeKeyPress()
        {
            ChangeScene("MainMenu");
        }
        private void OnAssetsPreloaded()
        {
            LogService.Log("OnAssetsPreloaded: Assets preloaded event received.");
            isLoadingComplete = true; // Set the flag to true
            CheckAndLoadWelcomeScene();
        }

        private void OnSplashLoadingComplete()
        {
            LogService.Log("Splash loading complete.");
            
        }	

        private void OnStartGame()
        {
            ChangeScene("GamePreload");
        }

        private void OnOpenOptions()
        {
            ChangeScene("Options");
        }

        private void OnGamePreloadComplete()
        {
            ChangeScene("Game");
        }

        #endregion
    }
}