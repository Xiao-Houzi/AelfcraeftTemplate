using System.Collections.Generic;
using Godot;

public class MockInputMap : IInputMap
{
    private readonly Dictionary<string, List<object>> _actions = new();

    public void AddAction(string actionName)
    {
        if (!_actions.ContainsKey(actionName))
        {
            _actions[actionName] = new List<object>();
        }
    }

    public void ActionAddEvent(string actionName, string key)
    {
        if (_actions.ContainsKey(actionName))
        {
            _actions[actionName].Add(key);
        }
    }

    public bool HasAction(string actionName)
    {
        return _actions.ContainsKey(actionName);
    }

    public void ActionEraseEvents(string actionName)
    {
        if (_actions.ContainsKey(actionName))
        {
            _actions[actionName].Clear();
        }
    }
}