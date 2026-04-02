using Godot;
using System;
using System.Diagnostics;

public partial class Player : Entity
{
    float SPEED = 5.0f;
    float SPEED_RUNNING = 7.5f;
    float SPEED_DELTA = 20f;
    float JUMP_VELOCITY = 4.5f;
    float GRAVITY = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    float pitch = 0f;
    const float MAX_PITCH = 89f;
    const float SHAKE_RANGE = .4f;
    const float RUNNING_SHAKE = 1.4f;
    float cameraShake = 0f;
    bool isRunning = false;
    float currentSpeed = 0f;
    Vector3 directionInput = new Vector3();
    float runStamina = 5f;
    float staminaRegenerationDelay = 0f;
    bool canRun = true;
    Vector2 cameraParalax = new Vector2();

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

    [Export]
    Node3D boxWeapon;

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
            pause.Visible = !pause.Visible;
        hud.HealthBar.Value = Health;
        hud.StaminaBar.Value = RunStamina;
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
            cameraParalax.X -= pitch;
            pitch = Mathf.Clamp(pitch + -ievmm.Relative.Y * mouseSensitivity, -MAX_PITCH, MAX_PITCH);
            playerCamera.RotationDegrees = new Vector3(pitch, 0, 0);
            cameraParalax.X += pitch;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaF = (float)delta;
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
        var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (inputDir.X == 0 && inputDir.Y == 0)
        {
            currentSpeed = Mathf.MoveToward(currentSpeed, 0, SPEED_DELTA * (float)delta);
            cameraShake = Mathf.MoveToward(cameraShake, 0, SPEED_DELTA * (float)delta);
            isRunning = false;
        }
        else
        {
            currentSpeed = Mathf.MoveToward(currentSpeed, isRunning ? SPEED_RUNNING : SPEED, SPEED_DELTA * (float)delta);
            cameraShake = Mathf.MoveToward(cameraShake, 1, SPEED_DELTA * (float)delta);
            directionInput = direction;
        }
        cameraParalax.X = Mathf.MoveToward(cameraParalax.X, 0, 4f * deltaF);
        cameraParalax.Y = Mathf.MoveToward(cameraParalax.Y, 0, 4f * deltaF);
        vX = directionInput.X * currentSpeed;
        vZ = directionInput.Z * currentSpeed;
        var rotation = boxWeapon.Rotation;
        rotation.X = MathF.Sin((float)Time.GetTicksMsec() * .01f) * 0.1f * cameraShake;
        rotation.X -= cameraParalax.X;
        rotation.X = Math.Clamp(rotation.X, -1f, 1f);
        boxWeapon.Rotation = rotation;
        Velocity = new Vector3(vX, vY, vZ);
        MoveAndSlide();


    }
}
