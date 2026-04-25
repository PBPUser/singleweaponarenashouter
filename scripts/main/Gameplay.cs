using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

public partial class Gameplay : Node3D
{
	public static Gameplay Current;

	[Export]
	PauseMenu pause;
	[Export]
	PlayerHud hud;
	[Export]
	public Player player;
	[Export]
	public WorldEnvironment _Environment;

	LoadedLevel level;
	LoadedStage _stage;
	bool isLevelLoaded;
	double tick;
	int stage = 0;
	long[] activationTicks;
	int activationIndex = 0;
	long nextActivation = 0;
	float totalTicks = 0;

	int bossCount = 0;
	List<Boss> bosses = new();
	LoadedAction[] actions;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Current != null)
			System.Environment.Exit(0);
		Current = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!isLevelLoaded)
			return;
		tick += (delta * 20);
		if (tick > nextActivation)
		{
			bossCount = 0;
			activationIndex++;
			actions = _stage.Actions.Where(x => x.Tick == nextActivation).ToArray();
			foreach (var x in actions)
			{
				x.Boss.ProcessMode = ProcessModeEnum.Inherit;
				if (x.SetPosition)
					x.Boss.Position = x.Position;
			}
			if (activationTicks.Length > activationIndex)
				nextActivation = activationTicks[activationIndex];
		}
		foreach (var x in actions)
			if (!x.Boss.IsDied)
				x.Method.Invoke(x.Boss, x.Values);
		if (_stage.Length < tick)
		{
			if (_stage.TimerType == InfoStage.TimerType.LoopUntilDeath)
			{
				tick -= _stage.Length;
				nextActivation = 0;
				activationIndex = 0;
			}
			else
				MoveToNextStage();
		}
		hud.Tick.Text = _stage.TimerType == InfoStage.TimerType.Limited ? FormatTime(_stage.Length - (long)tick) : tick + "";
		hud.BossStage.Text = _stage.StageName + "\n" + String.Format(", ", level.Stages[stage].ActivationTicks) + $"\nStage: {stage}\nNext tick: {nextActivation}";
	}

	void MoveToNextStage()
	{
		tick = 0;
		activationIndex = 0;
		if (level.Stages.Count > stage + 1)
		{
			stage++;
			activationTicks = level.Stages[stage].ActivationTicks;
		}
		else
			pause.Visible = true;
		nextActivation = activationTicks[0];
		bossCount = 0;
		bool isNew;
		foreach (var x in level.Stages[stage].BossHealthSet)
		{
			isNew = false;
			bossCount++;
			x.Boss.Health = x.Value;
			if (!bosses.Contains(x.Boss))
			{
				isNew = true;
				AddChild(x.Boss);
				bosses.Add(x.Boss);
			}
			x.Boss.Died(false);
			if (x.Position != null)
				x.Boss.Position = new Godot.Vector3(x.Position.Value.X, x.Position.Value.Y, x.Position.Value.Z);
			else if (isNew)
				x.Boss.Position = Vector3.Up;
		}
		_stage = level.Stages[stage];
	}

	string FormatTime(long tick)
	{
		long second = tick / 20;
		long minute = second / 60;
		second %= 60;
		string str = "";
		if (minute != 0)
			str += $"{minute:00}:";
		return $"{str}{second:00}";
	}

	public void SetLevel(LoadedLevel loadedLevel)
	{
		level = loadedLevel;
		isLevelLoaded = true;
		stage = -1;
		MoveToNextStage();
	}

	public void SetWeapon(SceneAttribute weaponSceneAttrib)
	{
		var weaponPS = GD.Load<PackedScene>(weaponSceneAttrib.ScenePath);
		var weapon = weaponPS.Instantiate<Weapon>();
		player.SetWeapon(weapon);
	}

	public void BossDied(string bossId)
	{
		if (!isLevelLoaded)
			return;
		LoadedStage _stage = level.Stages[stage];
		switch (_stage.RequiredBossTypeDie)
		{
			case InfoStage.RequiresToDie.AnyBoss:
				MoveToNextStage();
				return;
			case InfoStage.RequiresToDie.ExactBoss:
				return;
			case InfoStage.RequiresToDie.AllBosses:
				bossCount--;
				if (bossCount == 0)
					MoveToNextStage();
				return;
		}
	}
}
