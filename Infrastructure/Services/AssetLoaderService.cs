using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Survival.Infrastructure.Services; // Add this to reference LogService

public class AssetLoaderService : BaseService
{
    // Update to use BaseService for accessing services.
    public AssetLoaderService() : base()
    {
        // No additional initialization needed as services are accessed via BaseService.
    }

    // Add a method to check memory availability.
    public long GetAvailableMemory()
    {
        return System.GC.GetTotalMemory(false);
    }

    // Update LoadAsset to cache assets in memory.
    public T LoadAsset<T>(string path) where T : Resource
    {
        if (_assetCache.TryGetValue(path, out var cachedAsset))
        {
            return cachedAsset as T;
        }

        var asset = ResourceLoader.Load<T>(path);
        if (asset != null)
        {
            _assetCache[path] = asset;
        }
        else
        {
            LogService.LogErr($"Failed to load asset at path: {path}");
        }
        return asset;
    }

    public PackedScene LoadScene(string sceneName)
    {
        var gameStateFolder = StateStorage.GetReferenceType<string>("GameStateFolder");
        var path = System.IO.Path.Combine(gameStateFolder, sceneName + ".tscn");
        LogService.Log($"Loading scene: {path}");
        var scene = GD.Load<PackedScene>(path);
        if (scene == null)
        {
            LogService.LogErr($"Failed to load scene at path: {path}");
        }
        return scene;
    }

    public Texture LoadTexture(string path)
    {
        return LoadAsset<Texture>(path);
    }

    public Material LoadMaterial(string path)
    {
        return LoadAsset<Material>(path);
    }

    public Mesh LoadMesh(string path)
    {
        return LoadAsset<Mesh>(path);
    }

    public async Task PreloadAssets()
    {
        LogService.Log("PreloadAssets: Starting asset preloading.");

        await Task.Run(() =>
        {
            PreloadScenes(new List<string> { "MainMenu", "Options", "Welcome", "Game" });
            PreloadTextures();
            PreloadMaterials();
            PreloadMeshes();

            LogService.Log("PreloadAssets: All assets preloaded. Sending AssetsPreloaded signal.");
            Services.GetService<MessengerService>().SendMessage(MessengerService.MessageType.AssetsPreloaded);
        });

        LogService.Log("PreloadAssets: Asset preloading task completed.");
    }

    private void PreloadTextures()
    {
        var textureFolder = "Assets/Textures";
        var textureFiles = System.IO.Directory.GetFiles(textureFolder, "*.tres", System.IO.SearchOption.TopDirectoryOnly);
        foreach (var textureFile in textureFiles)
        {
            LogService.Log($"Preloading texture: {textureFile}");
            LoadTexture(textureFile);
        }
    }

    private void PreloadMaterials()
    {
        var materialFolder = "Assets/Materials";
        var materialFiles = System.IO.Directory.GetFiles(materialFolder, "*.tres", System.IO.SearchOption.TopDirectoryOnly);
        foreach (var materialFile in materialFiles)
        {
            LogService.Log($"Preloading material: {materialFile}");
            try
            {
                LoadMaterial(materialFile);
            }
            catch (Exception ex)
            {
                LogService.LogErr($"Failed to preload material: {materialFile}. Error: {ex.Message}");
            }
        }
    }

    private void PreloadMeshes()
    {
        var meshFolder = "Assets/Meshes";
        var meshFiles = System.IO.Directory.GetFiles(meshFolder, "*.tres", System.IO.SearchOption.TopDirectoryOnly);
        foreach (var meshFile in meshFiles)
        {
            LogService.Log($"Preloading mesh: {meshFile}");
            LoadMesh(meshFile);
        }
    }

    // Add a method to clear unused assets from memory.
    public void ClearUnusedAssets()
    {
        foreach (var key in new List<string>(_assetCache.Keys))
        {
            if (_assetCache[key] == null)
            {
                _assetCache.Remove(key);
            }
        }
        System.GC.Collect();
    }


    public void PreloadScenes(List<string> sceneNames)
    {
        var gameStateFolder = BaseService.StateStorage.GetReferenceType<string>("GameStateFolder");
        foreach (var name in sceneNames)
        {
            if (!_instantiatedScenes.ContainsKey(name))
            {
                var path = System.IO.Path.Combine(gameStateFolder, name + ".tscn");
                LogService.Log($"Preloading scene: {path}");
                var packedScene = GD.Load<PackedScene>(path);
                if (packedScene != null)
                {
                    var instance = packedScene.Instantiate();
                    if (instance is BaseGameState baseGameState)
                    {
                        _sceneCache[name] = packedScene;
                        AddToInstantiatedScenes(name, baseGameState);
                    }
                    
                    LogService.Log($"Scene instantiated and preloaded: {name}");
                }
                else
                {
                    LogService.LogErr($"Failed to preload scene: {name}");
                }
            }
        }
    }

    public Node GetInstantiatedScene(string sceneName)
    {
        if (_instantiatedScenes.TryGetValue(sceneName, out var scene))
        {
            return scene;
        }
        LogService.LogErr($"Scene not found in instantiated cache: {sceneName}");
        return null;
    }

    public PackedScene GetPreloadedScene(string path)
    {
        if (_sceneCache.TryGetValue(path, out var scene))
        {
            return scene;
        }
        LogService.LogErr($"Scene not found in cache: {path}");
        return null;
    }

    // Add a dictionary to store loaded assets in memory.
    private readonly Dictionary<string, Resource> _assetCache = new Dictionary<string, Resource>();
    private readonly Dictionary<string, PackedScene> _sceneCache = new Dictionary<string, PackedScene>();
    private readonly Dictionary<string, BaseGameState> _instantiatedScenes = new Dictionary<string, BaseGameState>(StringComparer.OrdinalIgnoreCase);

    public void InstantiateSplashImmediately()
    {
        LogService.Log("Instantiating splash scene immeediately.");
        var splashScene = LoadScene("Splash");
        if (splashScene != null)
        {
            var splashInstance = (BaseGameState)splashScene.Instantiate();
            if (splashInstance is BaseGameState baseGameState)
            {
                BaseGameState.Initialise(Services);
            }
            AddToInstantiatedScenes("Splash", splashInstance);
            LogService.Log("Splash scene instantiated and stored in the scene cache.");
        }
        else
        {
            LogService.LogErr("Failed to load splash scene.");
        }

        if (_instantiatedScenes.TryGetValue("Splash", out var cachedSplashInstance))
        {
            if (cachedSplashInstance is BaseGameState baseGameState)
            {
                LogService.Log("Splash scene instantiated and message sent to SceneManager.");
                Services.GetService<MessengerService>().SendMessage(MessengerService.MessageType.SplashInstantiated);

            }
        }
    }

    public List<string> ListGameStates()
    {
        var gameStateFolder = "Assets/GameStates";
        var gameStateFiles = System.IO.Directory.GetFiles(gameStateFolder, "*.tscn", System.IO.SearchOption.TopDirectoryOnly);
        var gameStateNames = new List<string>();
        foreach (var file in gameStateFiles)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(file);
            if (name != "Splash") // Exclude Splash from deferred states
            {
                gameStateNames.Add(name);
            }
        }
        return gameStateNames;
    }

    public void AddToInstantiatedScenes(string sceneName, BaseGameState sceneInstance)
    {
        if (!_instantiatedScenes.ContainsKey(sceneName))
        {
            _instantiatedScenes[sceneName] = sceneInstance;
            sceneInstance.Initialise();
            LogService.Log($"Scene added to instantiated scenes: {sceneName}");
        }
        else
        {
            LogService.LogErr($"Scene already exists in instantiated scenes: {sceneName}");
        }
    }
}