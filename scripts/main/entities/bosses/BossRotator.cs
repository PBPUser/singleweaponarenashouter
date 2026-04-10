using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

public partial class BossRotator : Boss
{
	static MethodInfo pistolMethodInfoProcess = Assembly.GetExecutingAssembly().GetType(typeof(Bullet).FullName).GetMethod("ProcessRotatorBulletMethod");
	static MethodInfo pistolMethodInfoPrepare = Assembly.GetExecutingAssembly().GetType(typeof(Bullet).FullName).GetMethod("PrepareRotatorBulletMethod");
	[Export]
	NavigationAgent3D navAgent3D;

	public void SetValues()
	{

	}

	public void jump()
	{
		var velocity = Velocity;
		if (IsOnFloor())
			velocity.Y = JumpVelocity;
		Velocity = velocity;
	}

	float shotDelay = 0f;

	public void rotate_and_shoot()
	{
		RotateY(__delta);
		shotDelay -= __delta;
		if (shotDelay < 0)
		{
			shotDelay = 4.0f;
			//var bullet = Bullet.Spawn(pistolMethodInfoPrepare, pistolMethodInfoProcess, GlobalPosition, Gameplay.Current,
			//	new List<DamageModifier>(), 10f, new string[] { "player" }, 20f);
			//bullet.GlobalBasis = GlobalBasis;
		}
	}
}
