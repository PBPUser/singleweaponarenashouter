using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public partial class Entity : CharacterBody3D
{
	[Export]
	public MeshInstance3D Mesh;

	ShaderMaterial damageMaterial;

	bool autoDie = true;
	///<summary>
	/// Set IsDied to true when Health reaches 0
	/// </summary>
	public bool AutoDie
	{
		get => autoDie;
		set => autoDie = value;
	}

	protected float __delta;

	float health = 20.0f;
	public float Health
	{
		get => health;
		set
		{
			health = value;
			OnHealthChanged(value);
			if (health <= 0)
				if (autoDie)
					IsDied = true;
		}
	}

	bool isDied = false;
	public bool IsDied
	{
		get => isDied;
		set
		{
			isDied = value;
			Died(value);
		}
	}

	public virtual void OnHealthChanged(float newHealth)
	{

	}

	public virtual void Died(bool isDied)
	{

	}



	public sealed override void _Ready()
	{
		__Ready();
		if (this.Mesh == null)
			return;
		damageMaterial = new ShaderMaterial();
		damageMaterial.Shader = GD.Load<Shader>("res://assets/shaders/damage.gdshader");
		Mesh.MaterialOverlay = damageMaterial;
	}

	public virtual void __Ready()
	{

	}

	public void Damage(float damage, List<DamageModifier> damageModifiers, Basis damageBasis, Vector3 damagePoint)
	{
		foreach (DamageModifier x in damageModifiers)
		{
			damage = x.ModifyDamage(damage, this);
		}
		Health -= damage;
		if (Mesh == null)
			return;
		damageMaterial.SetShaderParameter("damagePoint", (damagePoint - GlobalPosition) * GlobalBasis);
		damageMaterial.SetShaderParameter("basis", damageBasis
		//.Rotated(Vector3.Up, -GlobalRotation[0])
		//.Rotated(Vector3.Right, -GlobalRotation[1])
		//.Rotated(Vector3.Back, -GlobalRotation[1])
		);
		damageMaterial.SetShaderParameter("damageStrengh", 1.0f);
	}

	float fallingDamage = 0;

	public void Paralizate()
	{

	}

	public sealed override void _Process(double delta)
	{
		float deltaF = (float)delta;
		if (Position.Y < -100)
		{
			Health -= deltaF * fallingDamage * 5;
			fallingDamage += deltaF;
		}
		__Process((float)delta);
	}

	public virtual void __Process(float delta)
	{

	}

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public override void _PhysicsProcess(double delta)
	{
		__delta = (float)delta;
		Vector3 velocity = Velocity;
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
