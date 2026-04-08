using Godot;
using System;
using System.Reflection;
using System.Collections.Generic;

public partial class Bullet : RigidBody3D, IDisposable
{
    static PackedScene bulletPS = GD.Load<PackedScene>("res://scenes/weapons/bullet.tscn");

    Gameplay gameplay;
    MethodInfo methodStart;
    MethodInfo methodUpdate;
    Dictionary<string, object> dict = new();
    object[] oc = new object[1];

    public static void Spawn(MethodInfo start, MethodInfo info, Vector3 position, Gameplay gameplay)
    {
        Bullet b = bulletPS.Instantiate<Bullet>();
        b.GlobalPosition = position;
        b.methodUpdate = info;
        b.gameplay = gameplay;
        start.Invoke(b, new object[] { });
    }

    public void SetValue(string index, object value)
    {
        dict[index] = value;
    }

    // Call ed when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        oc[0] = 0d;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        oc[0] = delta;
        methodUpdate.Invoke(this, oc);
    }

    public void ProcessPlayerBulletMethod(double delta)
    {
    }

    public void PreparePlayerBulletMethod()
    {
        GlobalTransform = gameplay.player.GlobalTransform;

    }
}
