using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Survival.Infrastructure.Services;

public partial class SceneManager : Node
{
	#region Public API

	// Update to use BaseService for accessing services.
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

		//Runsplash();

		MessengerService.SplashLoadingComplete += OnSplashLoadingComplete;
		MessengerService.StartupDelayComplete += OnStartupDelayComplete;
		MessengerService.AssetsPreloaded += OnAssetsPreloaded;
		MessengerService.WelcomeKeyPress += OnWelcomeKeyPress;
		MessengerService.SplashInstantiated += Runsplash;
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

	private async void Runsplash()
	{
		LogService.Log("Splash state active. Requesting asset preloading.");
		ChangeScene("Splash");

		await AssetLoaderService.PreloadAssets();

		// Send startup delay message
		await Task.Delay(1000); // Wait for 1 second
		MessengerService.SendMessage(MessengerService.MessageType.StartupDelayComplete);
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

	private void CheckAndLoadWelcomeScene()
	{
		LogService.Log($"CheckAndLoadWelcomeScene: isLoadingComplete={isLoadingComplete}, isStartupDelayComplete={isStartupDelayComplete}");
		if (isLoadingComplete && isStartupDelayComplete)
		{
			LogService.Log("CheckAndLoadWelcomeScene: Both conditions met. Loading Welcome scene...");
			ChangeScene("Welcome");
		}
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

	#endregion
}