using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

public partial class LevelSelectionMenu : Control
{
	PackedScene _gameplayScene = GD.Load<PackedScene>("res:///scenes/gameplay.tscn");

	[Export]
	GridContainer grid;

	[Export]
	GridContainer weaponContainer;

	[Export]
	Label selectedWeaponLabel;

	SceneAttribute attributeSelectedWeapon;

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
		foreach (var x in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAssignableTo(typeof(Weapon)) && x != typeof(Weapon)))
		{
			var attribs = x.GetCustomAttributes();
			var nameAttrib = x.GetCustomAttribute<NameAttribute>();
			if (nameAttrib == null)
				continue;
			var sceneAttrib = x.GetCustomAttribute<SceneAttribute>();
			if (sceneAttrib == null)
				continue;
			Button btn = new();
			btn.Text = nameAttrib.Name;
			btn.Pressed += () =>
			{
				selectedWeaponLabel.Text = nameAttrib.Name;
				attributeSelectedWeapon = sceneAttrib;
			};
			weaponContainer.AddChild(btn);

			if (attributeSelectedWeapon == null)
			{
				attributeSelectedWeapon = sceneAttrib;
				selectedWeaponLabel.Text = nameAttrib.Name;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void openLevel(InfoLevel info)
	{
		if (attributeSelectedWeapon == null)
			return;
		Visible = false;
		LoadedLevel loaded = new LoadedLevel(info);
		var g = _gameplayScene.Instantiate() as Gameplay;
		AddChild(g);
		g.SetLevel(loaded);
		g.SetWeapon(attributeSelectedWeapon);
	}
}
