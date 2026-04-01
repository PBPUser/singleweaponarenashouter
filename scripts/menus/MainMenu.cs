using Godot;
using System;
using System.Diagnostics;

public partial class MainMenu : Control
{
	PackedScene _gameplayScene = GD.Load<PackedScene>("res:///scenes/gameplay.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void _on_start_btn_pressed()
	{
		GetTree().ChangeSceneToPacked(_gameplayScene);
	}

	void _on_load_btn_pressed()
	{

	}

	void _on_options_btn_pressed()
	{

	}

	void _on_quit_btn_pressed()
	{
		System.Environment.Exit(0);
	}
}
