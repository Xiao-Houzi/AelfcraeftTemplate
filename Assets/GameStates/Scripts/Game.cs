using Godot;
using System;
using System.Linq; // Ensure LINQ is available for string.Join usage

public partial class Game : BaseGameState
{
    public override void _Ready()
		{
			base._Ready();
			
			_missionLabel = GetNode<Label>("MissionLabel");
			UpdateMissionDisplay();
	
		}

    protected override void OnInitialised()
    {
        // Perform any additional initialization specific to Game
    }

    public override void _Process(double delta)
    {
        base._Process(delta); // Ensure the base method is called
        UpdateMissionDisplay();

        if (InputService.IsActionPressed("move_up"))
        {
            GD.Print("Moving up");
        }
        if (InputService.IsActionPressed("move_down"))
        {
            GD.Print("Moving down");
        }
        if (InputService.IsActionPressed("move_left"))
        {
            GD.Print("Moving left");
        }
        if (InputService.IsActionPressed("move_right"))
        {
            GD.Print("Moving right");
        }
        if (InputService.IsActionPressed("jump"))
        {
            GD.Print("Jumping");
        }
    }
		

		private void UpdateMissionDisplay()
		{
			var currentStage = MissionService.GetCurrentStage();
			if (currentStage != null)
			{
				_missionLabel.Text = $"Mission: {currentStage.Description}\nTasks:\n" +
					string.Join("\n", currentStage.Tasks.Select(t => $"- {t.Description} {(t.IsCompleted ? "(Completed)" : "")}"));
			}
			else
			{
				_missionLabel.Text = "No active mission.";
			}
		}

		
private Label _missionLabel;
}