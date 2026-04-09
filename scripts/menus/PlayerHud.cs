using Godot;
using System;

public partial class PlayerHud : Control
{
	[Export]
	public Label Health;

	[Export]
	public Label BossName;

	[Export]
	public Label BossStage;

	[Export]
	public Label Tick;

	[Export]
	public ProgressBar HealthBar;

	[Export]
	public ProgressBar StaminaBar;

	[Export]
	public Label PositionLabel;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
