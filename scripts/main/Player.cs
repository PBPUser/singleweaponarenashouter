using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Player : Entity
{
	float SPEED = 5.0f;
	float SPEED_CROUCHING = 2.5f;
	float SPEED_RUNNING = 7.5f;
	float SPEED_DELTA = 20f;
	float JUMP_VELOCITY = 4.5f;
	float GRAVITY = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	float pitch = 0f;
	const float MAX_PITCH = 89f;
	const float SHAKE_RANGE = .4f;
	const float RUNNING_SHAKE = 1.4f;
	const float CROUCHING_SPEED = 4f;
	const float CROUCHING_LENGTH = 1f;
	const float CROUCHING_STRENGTH = 4f;

	static Vector3
		COLLISION_POSITION_DEFAULT = new Vector3(0, 1, 0),
		COLLISION_TRANSFORM_DEFAULT = Vector3.One,
		CAMERA_POSITION_DEFAULT = new Vector3(0, 1.5f, -.25f),
		COLLISION_POSITION_CROUCH = new Vector3(0, .25f, 0),
		COLLISION_TRANSFORM_CROUCH = new Vector3(1, .5f, 1),
		CAMERA_POSITION_CROUCH = new Vector3(0, .5f, -.25f);

	float cameraShake = 0f;
	bool isRunning = false;
	float currentSpeed = 0f;
	Vector3 directionInput = new Vector3();
	float runStamina = 5f;
	float staminaRegenerationDelay = 0f;
	bool canRun = true;
	Vector2 cameraParalax = new Vector2();
	bool isSneaking = false;
	float sneaking = 0f;

	bool isCrouching = false;
	float crouching = 0f;
	Vector3 crouchingTo = new Vector3(0, 0, 0);

	public float DamageValue = 0.5f;
	public List<DamageModifier> DamageModifiers = new();

	bool IsCrouching
	{
		get => isCrouching;
		set
		{
			if (value)
				crouching = CROUCHING_LENGTH;
			isCrouching = value;
		}
	}

	bool IsSneaking
	{
		get => isSneaking;
		set
		{
			isSneaking = value;
			if (isRunning)
				isRunning = false;
		}
	}

	float RunStamina
	{
		get => runStamina;
		set
		{
			if (value < 0 && canRun)
			{
				runStamina = 0;
				canRun = false;
				staminaRegenerationDelay = STAMINA_REGENERATION_DELAY;
			}
			else if (value >= MAX_STAMINA && !canRun)
			{
				runStamina = MAX_STAMINA;
				canRun = true;
			}
			else
				runStamina = Math.Clamp(value, 0, MAX_STAMINA);
		}
	}

	const float STAMINA_REGENERATION_SPEED = 1.2f;
	const float STAMINA_REGENERATION_DELAY = 5f;
	const float MAX_STAMINA = 5f;

	float shakingTime = 0;
	float time = 0;
	float timeShaking = 0f;

	[Export]
	PlayerHud hud;

	[Export]
	public Camera3D playerCamera;

	[Export]
	PauseMenu pause;

	[Export]
	PlayerDeathUI deathUI;

	[Export]
	float mouseSensitivity = 0.3f;

	[Export]
	Weapon weapon;

	[Export]
	CollisionShape3D collision;

	[Export]
	Node3D cameraShakeNode;

	[Export]
	Node3D boxWeapon;

	[Export]
	Gameplay gameplay;

	// Called when the node enters the scene tree for the first time.
	public override void ObjectReady()
	{
		Health = 100;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void Process(float delta)
	{
		if (Input.IsActionJustPressed("pause") && !IsDied)
			pause.Visible = !pause.Visible;
		hud.HealthBar.Value = Health;
		hud.StaminaBar.Value = RunStamina;
		hud.Health.Text = "" + IsCrouching;
		base.Process(delta);
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
			cameraParalax.X -= pitch;
			pitch = Mathf.Clamp(pitch + -ievmm.Relative.Y * mouseSensitivity, -MAX_PITCH, MAX_PITCH);
			playerCamera.RotationDegrees = new Vector3(pitch, 0, 0);
			cameraParalax.X += pitch;
		}
		hud.PositionLabel.Text = $"Position: {GlobalPosition}\nRotation: {GlobalRotationDegrees}\nCamera rotation: {playerCamera.GlobalRotationDegrees}";
	}

	public override void Physics(float deltaF)
	{
		time += deltaF;
		timeShaking += deltaF;
		float vX = Velocity.X;
		float vY = Velocity.Y;
		float vZ = Velocity.Z;
		isSneaking = Input.IsActionPressed("sneak") && !IsCrouching;
		if (Input.IsActionJustPressed("crouch") && IsOnFloor() && !isCrouching && canRun)
		{
			IsCrouching = true;
			crouchingTo = (Transform.Basis * new Vector3(0, 0, -1)).Normalized();
		}
		else if (IsCrouching)
		{
			RunStamina -= deltaF * 4;
			crouching = Mathf.MoveToward(crouching, 0, deltaF);
			if (crouching <= 0)
				IsCrouching = false;
		}
		if (IsOnFloor() && !isCrouching)
		{
			if (Input.IsActionJustPressed("jump") && !isSneaking)
				vY = JUMP_VELOCITY;
			else
				vY = 0f;
		}
		else
			vY += -GRAVITY * deltaF;
		sneaking = Mathf.MoveToward(sneaking, isSneaking ? 1 : 0, CROUCHING_SPEED * deltaF);
		if (Input.IsActionPressed("run") && canRun)
			isRunning = true;
		if (!canRun)
			isRunning = false;
		if (isRunning)
			RunStamina -= deltaF;
		else if (staminaRegenerationDelay > 0)
			staminaRegenerationDelay -= deltaF;
		else
			RunStamina += deltaF * STAMINA_REGENERATION_SPEED;
		var inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		inputDir.Y *= 1 - (sneaking / 2);
		var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (inputDir.X == 0 && inputDir.Y == 0)
		{
			currentSpeed = Mathf.MoveToward(currentSpeed, 0, SPEED_DELTA * deltaF);
			cameraShake = Mathf.MoveToward(cameraShake, 0, SPEED_DELTA * deltaF);
			isRunning = false;
		}
		else
		{
			currentSpeed = Mathf.MoveToward(currentSpeed, isRunning ? SPEED_RUNNING : isSneaking ? SPEED_CROUCHING : SPEED, SPEED_DELTA * deltaF);
			cameraShake = Mathf.MoveToward(cameraShake, isRunning ? 2 : 1, SPEED_DELTA * deltaF);
			directionInput = direction;
			shakingTime += deltaF * 0.007f * cameraShake;
			timeShaking += deltaF * 0.007f * cameraShake;
		}
		var rotationCamera = playerCamera.RotationDegrees;
		rotationCamera.Z = Mathf.Sin(shakingTime) * cameraShake + ((2 - this.damageAnim) * 16f);
		playerCamera.RotationDegrees = rotationCamera;
		var shakeNodePos = cameraShakeNode.Position;
		shakeNodePos.Y = Mathf.Cos(shakingTime) * cameraShake * 0.1f;
		cameraShakeNode.Position = shakeNodePos;
		playerCamera.Position = CAMERA_POSITION_DEFAULT * (1f - sneaking) + (CAMERA_POSITION_CROUCH * sneaking);
		if (isCrouching)
		{
			var crouchAnim = Mathf.Pow(crouching / CROUCHING_LENGTH * 2 - 1, 4);
			playerCamera.Position = playerCamera.Position * (1f - crouchAnim) + (CAMERA_POSITION_CROUCH * crouchAnim);
		}
		collision.Position = COLLISION_POSITION_DEFAULT * (1f - sneaking) + (COLLISION_POSITION_CROUCH * sneaking);
		collision.Scale = COLLISION_TRANSFORM_DEFAULT * (1f - sneaking) + (COLLISION_TRANSFORM_CROUCH * sneaking);
		cameraParalax.X = Mathf.MoveToward(cameraParalax.X, 0, 4f * deltaF);
		cameraParalax.Y = Mathf.MoveToward(cameraParalax.Y, 0, 4f * deltaF);
		if (IsCrouching)
		{
			vX = crouchingTo.X * crouching * CROUCHING_STRENGTH;
			vZ = crouchingTo.Z * crouching * CROUCHING_STRENGTH;
		}
		else
		{
			vX = directionInput.X * currentSpeed;
			vZ = directionInput.Z * currentSpeed;
		}
		var posWeapon = boxWeapon.Position;
		posWeapon = new Vector3(0.02f, 0.02f, 0.02f) * new Vector3(Mathf.Sin(timeShaking * 2f),
		Mathf.Cos(timeShaking * 2f), 0) * (1f + cameraShake) * 5f;
		boxWeapon.Position = posWeapon;
		Velocity = new Vector3(vX, vY, vZ);
		delayWeapon -= deltaF;
		if (Input.IsActionPressed("attack"))
		{
			if (delayWeapon < 0)
			{
				weapon.Attack(gameplay, this);
				delayWeapon = weapon.ComputeAttackDelay(gameplay);
			}
		}
	}

	float delayWeapon = 0f;

	public void SetWeapon(Weapon w)
	{
		weapon = w;
		boxWeapon.AddChild(w);
		w.Position = Vector3.Zero;
	}
}
