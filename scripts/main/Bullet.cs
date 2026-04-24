using Godot;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Bullet : Area3D, IDisposable
{
	static PackedScene bulletPS = GD.Load<PackedScene>("res://scenes/weapons/bullet.tscn");

	Gameplay gameplay;
	MethodInfo methodStart;
	MethodInfo methodUpdate;
	Dictionary<string, object> dict = new();
	object[] oc = new object[1];
	string[] accepts = { "boss" };
	List<DamageModifier> damageModifiers;
	float damage;
	float life;

	public static Bullet Spawn(MethodInfo start, MethodInfo info, Vector3 position, Gameplay gameplay, List<DamageModifier> damageModifiers, float damage, string[] accepts, float life)
	{
		Bullet b = bulletPS.Instantiate<Bullet>();
		gameplay.AddChild(b);
		b.GlobalPosition = position;
		b.methodUpdate = info;
		b.gameplay = gameplay;
		b.damageModifiers = damageModifiers;
		b.damage = damage;
		b.accepts = accepts;
		b.life = life;
		start.Invoke(b, new object[] { });
		return b;
	}

	public void SetValue(string index, object value) => dict[index] = value;

	// Call ed when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		oc[0] = 0d;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		oc[0] = delta;
		methodUpdate.Invoke(this, oc);
		life -= (float)delta;
		if (life < 0)
			QueueFree();
	}

	public void ProcessPlayerBulletMethod(double delta)
	{
		this.Position += Basis * Vector3.Forward * 64f * (float)delta;
	}

	public void PreparePlayerBulletMethod()
	{
		GlobalRotationDegrees = gameplay.player.playerCamera.GlobalRotationDegrees;
		GlobalPosition = gameplay.player.playerCamera.GlobalPosition;
	}

	float ticker = 0;
	Vector3 target;
	Vector3 prevTarget;

	public void PrepareRotatorBulletMethod()
	{
		prevTarget = target = gameplay.player.playerCamera.GlobalPosition;
		ticker = 0;
	}

	public void ProcessRotatorBulletMethod(double delta)
	{
		float speed = 4f;
		float p = Math.Min(1f, ((20f - life) % 2f) * 0.5f);
		if (20f - life > ticker && ticker < 15f)
		{
			ticker += 2;
			prevTarget = target;
			target = gameplay.player.playerCamera.GlobalPosition;
		}
		else if (ticker > 15)
			speed = Mathf.Pow(speed, 1f + (float)delta);
		else if (p < 1f)
			LookAt(prevTarget * (1 - p) + target * p);
		GlobalPosition += GlobalBasis * Vector3.Forward * speed * (float)delta;
	}

	public void _on_body_entered(Node body)
	{
		foreach (var x in accepts)
		{
			if (body.IsInGroup(x))
			{
				((Entity)body).Damage(damage, damageModifiers, GlobalBasis, GlobalPosition);
				QueueFree();
				return;
			}
		}
	}
}
