using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class LevelSelectionMenu : Control
{
	PackedScene _gameplayScene = GD.Load<PackedScene>("res:///scenes/gameplay.tscn");

	[Export]
	GridContainer grid;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Debug.WriteLine("Ready");
		for (int i = 0; i < LevelLoader.Infos.Count(); i++)
		{
			var x = LevelLoader.Infos[i];
			Button btn = new();
			btn.Text = LevelLoader.Files[i];
			btn.Pressed += () => openLevel(x);
			grid.AddChild(btn);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void openLevel(InfoLevel info)
	{
		LoadedLevel loaded = new LoadedLevel(info);
		var g = _gameplayScene.Instantiate() as Gameplay;
		AddChild(g);
		g.SetLevel(loaded);
		grid.Visible = false;
	}
}
