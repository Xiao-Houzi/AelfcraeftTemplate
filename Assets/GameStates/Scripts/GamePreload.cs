using Godot;
using System.Threading.Tasks;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.GameStates
{
    public partial class GamePreload : Node3D
    {
        private AssetLoaderService _assetLoaderService;
        private MessengerService _messengerService;

        public override void _Ready()
        {
            _assetLoaderService = BaseService.Services.GetService<AssetLoaderService>();
            _messengerService = BaseService.Services.GetService<MessengerService>();

            PreloadAssets();
        }

        private async void PreloadAssets()
        {
            GD.Print("Starting asset preloading...");
            await _assetLoaderService.PreloadAssets();
            GD.Print("Asset preloading complete. Moving to Game scene.");

            _messengerService.SendMessage(MessengerService.MessageType.GamePreloadComplete);
        }
    }
}