using Godot;
using System;
using System.Reflection;

[Name("weapon.pistol")]
[Scene("res://scenes/weapons/pistol.tscn")]
public partial class Pistol : Weapon
{
	static MethodInfo pistolMethodInfoProcess = Assembly.GetExecutingAssembly().GetType(typeof(Bullet).FullName).GetMethod("ProcessPlayerBulletMethod");
	static MethodInfo pistolMethodInfoPrepare = Assembly.GetExecutingAssembly().GetType(typeof(Bullet).FullName).GetMethod("PreparePlayerBulletMethod");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}



	public override void Attack(Gameplay gameplay, Player player)
	{
		Bullet.Spawn(pistolMethodInfoPrepare, pistolMethodInfoProcess, player.Position, gameplay);
	}
}
