using Godot;
using System;
using System.Diagnostics;

public partial class BossRotator : Boss
{
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

	public void rotate_and_shoot()
	{
		RotateY((float)__delta);
	}
}
