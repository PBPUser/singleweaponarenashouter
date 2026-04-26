using Godot;
using System;
using System.Diagnostics;

public partial class PauseMenu : Control
{
	long DateTime = 0;
	[Export]
	PlayerHud hud;
	const int Delay = 5000;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
			if (DateTime + Delay < System.DateTime.UtcNow.ToBinary())
				Visible = false;
	}

	void _on_visibility_changed()
	{
		DateTime = System.DateTime.UtcNow.ToBinary();
		GetTree().Paused = Visible;
		hud.Visible = !Visible;
		if (Visible)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else
			Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
