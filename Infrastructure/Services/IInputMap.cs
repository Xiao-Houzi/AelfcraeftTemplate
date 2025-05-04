using Godot;

public interface IInputMap
{
    void AddAction(string actionName);
    void ActionAddEvent(string actionName, string key);
    bool HasAction(string actionName);
    void ActionEraseEvents(string actionName);
}