using System;
using System.Collections.Generic;

namespace Survival.Infrastructure.Services
{
    public class InputService : BaseService
    {
        private readonly IInputMap _inputMap;
        private readonly Func<string, bool> _isActionPressed;
        private Dictionary<string, string> _actionBindings;
        private Dictionary<string, bool> _inputStates;

        public InputService(IInputMap inputMap, Func<string, bool> isActionPressed)
        {
            _inputMap = inputMap;
            _isActionPressed = isActionPressed;
            _actionBindings = new Dictionary<string, string>();
            _inputStates = new Dictionary<string, bool>();
        }

        public void BindKeyToAction(string actionName, string key)
        {
            if (_inputMap.HasAction(actionName))
            {
                _inputMap.ActionEraseEvents(actionName);
            }
            else
            {
                _inputMap.AddAction(actionName);
            }

            _inputMap.ActionAddEvent(actionName, key);
            _actionBindings[actionName] = key;
        }

        public string GetKeyForAction(string actionName)
        {
            return _actionBindings.ContainsKey(actionName) ? _actionBindings[actionName] : null;
        }

        public bool IsActionPressed(string actionName)
        {
            return _isActionPressed(actionName);
        }

        public void ProcessInput(string inputName, bool isPressed)
        {
            _inputStates[inputName] = isPressed;
        }

        public bool GetInputState(string inputName)
        {
            return _inputStates.ContainsKey(inputName) && _inputStates[inputName];
        }
    }
}