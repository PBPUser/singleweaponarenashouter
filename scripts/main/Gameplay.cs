using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

public partial class Gameplay : Node3D
{
	[Export]
	PauseMenu pause;
	[Export]
	PlayerHud hud;

	LoadedLevel level;
	bool isLevelLoaded;
	double tick;
	int stage = 0;
	long[] activationTicks;
	int activationIndex = 0;
	long nextActivation = 0;

	List<Boss> bosses = new();
	LoadedAction[] actions;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isLevelLoaded)
		{
			tick += (delta * 20);
			LoadedStage _stage = level.Stages[stage];
			if (tick > nextActivation)
			{
				activationIndex++;
				actions = _stage.Actions.Where(x => x.Tick == nextActivation).ToArray();
				foreach (var x in actions)
					if (!bosses.Contains(x.Boss))
					{
						bosses.Add(x.Boss);
						AddChild(x.Boss);
						if (!x.SetPosition)
							x.Boss.Position = new Vector3(0, 2, 0);
					}
				if (activationTicks.Length > activationIndex)
					nextActivation = activationTicks[activationIndex];
				foreach (var x in actions)
					if (x.SetPosition)
						x.Boss.Position = x.Position;
			}
			foreach (var x in actions)
				x.Method.Invoke(x.Boss, x.Values);
			if (_stage.Length < tick)
			{
				if (_stage.TimerType == InfoStage.TimerType.LoopUntilDeath)
				{
					tick -= _stage.Length;
					nextActivation = activationTicks[0];
					activationIndex = 0;
				}
				else
				{
					tick = 0;
					nextActivation = activationTicks[0];
					activationIndex = 0;
					if (level.Stages.Count > stage + 1)
					{
						stage++;
						activationTicks = level.Stages[stage].ActivationTicks;
					}
					else
						pause.Visible = true;
				}
			}
			hud.Tick.Text = _stage.TimerType == InfoStage.TimerType.Limited ? formatTime(_stage.Length - (long)tick) : "";
			hud.BossStage.Text = _stage.StageName;
		}
	}

	string formatTime(long tick)
	{
		long second = tick / 20;
		long _tick = tick % 20;
		long minute = second / 60;
		second %= 60;
		return $"{minute:00}:{second:00}.{(_tick * 5):00}";
	}

	public void SetLevel(LoadedLevel level)
	{
		this.level = level;
		isLevelLoaded = true;
		tick = 0;
		stage = 0;
		activationTicks = level.Stages[0].ActivationTicks;
		nextActivation = activationTicks[0];
	}
}
