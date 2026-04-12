using System;
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

	Basis damageBasis;

	public void Damage(float damage, List<DamageModifier> damageModifiers, Basis damageBasis, Vector3 damagePoint)
	{
		damageAnim = 0;
		foreach (DamageModifier x in damageModifiers)
			damage = x.ModifyDamage(damage, this);
		Health -= damage;
		this.damageBasis = damageBasis;
		if (Mesh == null)
			return;
		damageMaterial.SetShaderParameter("damagePoint", (damagePoint - GlobalPosition) * GlobalBasis);
		damageMaterial.SetShaderParameter("basis", damageBasis
		//.Rotated(Vector3.Up, -GlobalRotation[0])
		//.Rotated(Vector3.Right, -GlobalRotation[1])
		//.Rotated(Vector3.Back, -GlobalRotation[1])
		);
		Debug.WriteLine($"Damaged {damage}");
		Velocity -= dmgVelocity;
	}

	float fallingDamage = 0;
	float damageAnim = 2;

	public void Paralize()
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
		if (Mesh == null)
			return;
		damageMaterial.SetShaderParameter("damageStrengh", 1 - Math.Abs(damageAnim - 1));
		Debug.WriteLine($"Damage Strength: {1 - Math.Abs(damageAnim - 1)}");
	}

	public virtual void __Process(float delta)
	{

	}

	public virtual void __Physics(float delta)
	{

	}

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	Vector3 dmgVelocity = Vector3.Zero;

	public sealed override void _PhysicsProcess(double delta)
	{
		__delta = (float)delta;
		float deltaF = (float)delta;
		Vector3 velocity = Velocity;
		if (!IsOnFloor())
			velocity += GetGravity() * deltaF;
		Velocity = velocity;
		__Physics(deltaF);
		Velocity -= dmgVelocity;
		if (damageAnim < 2)
		{
			damageAnim = Mathf.Min(2.0f, damageAnim + deltaF);
			dmgVelocity = (Vector3.Forward * damageBasis + Vector3.Up) * (1 - Math.Abs(damageAnim - 1));
		}
		Velocity += dmgVelocity;
		MoveAndSlide();
	}

}
