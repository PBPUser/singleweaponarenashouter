using Godot;
using System;
using System.Diagnostics;

public partial class Player : Entity
{
	float SPEED = 5.0f;
	float JUMP_VELOCITY = 4.5f;
	float GRAVITY = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	float pitch = 0f;
	const float MAX_PITCH = 89f;

	[Export]
	PlayerHud hud;

	[Export]
	Camera3D playerCamera;

	[Export]
	PauseMenu pause;

	[Export]
	PlayerDeathUI deathUI;

	[Export]
	float mouseSensitivity = 0.3f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Health = 100;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause") && !IsDied)
		{
			pause.Visible = !pause.Visible;
		}
		hud.Health.Text = Health + "";
		base._Process(delta);
	}

	public override void Died(bool value)
	{
		GetTree().Paused = value;
		Input.MouseMode = value ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		deathUI.Visible = value;
	}

	public override void _Input(Godot.InputEvent ev)
	{
		if (ev is InputEventMouseMotion ievmm)
		{
			RotateY(Mathf.DegToRad(-ievmm.Relative.X * mouseSensitivity));
			pitch = Mathf.Clamp(pitch + -ievmm.Relative.Y * mouseSensitivity, -MAX_PITCH, MAX_PITCH);
			playerCamera.RotationDegrees = new Vector3(pitch, 0, 0);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		float vX = Velocity.X;
		float vY = Velocity.Y;
		float vZ = Velocity.Z;
		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("jump"))
				vY = JUMP_VELOCITY;
			else
				vY = 0f;
		}
		else
			vY += -GRAVITY * (float)delta;
		var inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (inputDir.X == 0 && inputDir.Y == 0)
		{
			vX = Mathf.MoveToward(vX, 0, SPEED);
			vZ = Mathf.MoveToward(vZ, 0, SPEED);
		}
		else
		{
			vX = direction.X * SPEED;
			vZ = direction.Z * SPEED;
		}
		Velocity = new Vector3(vX, vY, vZ);
		MoveAndSlide();

	}
}
