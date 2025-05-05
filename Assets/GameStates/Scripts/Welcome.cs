using Godot;
using System;
using Aelfcraeft.Infrastructure.Services;

namespace Aelfcraeft.GameStates
{
    public partial class Welcome : BaseGameState
    {
        public Welcome()
        {
            freeable = true;
        }
        
        public override void _Ready()
        {
            
            if (StateStorageService.GetValueType<bool>("HasSeenWelcome"))
            {
                MessengerService.SendMessage(MessengerService.MessageType.WelcomeKeyPress);
                return;
            }

            StateStorageService.SetState("HasSeenWelcome", true);
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey)
            {
                MessengerService.SendMessage(MessengerService.MessageType.WelcomeKeyPress);
            }
        }
    }
}