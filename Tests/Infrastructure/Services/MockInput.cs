using System.Collections.Generic;

// Mocked Input class to isolate tests from Godot's runtime
public static class MockInput
{
    private static readonly HashSet<string> PressedActions = new();

    public static void SimulateActionPress(string actionName)
    {
        PressedActions.Add(actionName);
    }

    public static void SimulateActionRelease(string actionName)
    {
        PressedActions.Remove(actionName);
    }

    public static bool IsActionPressed(string actionName)
    {
        return PressedActions.Contains(actionName);
    }
}