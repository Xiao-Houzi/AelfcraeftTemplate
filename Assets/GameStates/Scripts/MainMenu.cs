using Godot;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.GameStates
{
    public partial class MainMenu : BaseGameState
    {
    

        public override void _Ready()
        {
            
        }

        private void OnNewGamePressed()
        {
            MessengerService.SendMessage(MessengerService.MessageType.StartGame);
        }

        private void OnOptionsPressed()
        {
            MessengerService.SendMessage(MessengerService.MessageType.OpenOptions);
        }

        private void OnExitPressed()
        {
            MessengerService.SendMessage(MessengerService.MessageType.ExitGame);
        }
    }
}