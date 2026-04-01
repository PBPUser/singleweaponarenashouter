using Godot;
using System;
using System.Diagnostics;

public partial class MainMenu : Control
{
	[Export]
	GridContainer container;

	PackedScene levelSelectionPackedScene = GD.Load<PackedScene>("res:///menus/level_selection_menu.tscn");
	LevelSelectionMenu selectionMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		selectionMenu = levelSelectionPackedScene.Instantiate<LevelSelectionMenu>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void _on_start_btn_pressed()
	{
		AddChild(selectionMenu);
		container.Visible = false;
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
