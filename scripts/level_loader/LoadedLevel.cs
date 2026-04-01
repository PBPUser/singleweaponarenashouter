using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Godot;

public class LoadedLevel
{
	static LoadedLevel()
	{
		foreach (var x in DirAccess.GetFilesAt("res:///scenes/entities/bosses/"))
			bossPackedScenes[x.Remove(x.Length - 5)] = GD.Load<PackedScene>($"res:///scenes/entities/bosses/{x}");
		foreach (var x in bossPackedScenes.Keys)
			Debug.WriteLine(x);
	}

	static Dictionary<string, PackedScene> bossPackedScenes = new();

	public LoadedLevel(InfoLevel info)
	{
		foreach (var x in info.Bosses)
		{
			Type classType = Assembly.GetExecutingAssembly().GetType(x.BossClass);
			if (classType == null)
				throw new Exception($"Class {x.BossClass} not found");
			MethodInfo[] infos = classType.GetMethods().Where(c => c.Name.Equals("SetValues")).ToArray();
			if (infos.Count() == 0)
				throw new Exception($"Class {x.BossClass} must be to have SetValues method to continue");
			infos = infos.Where(c => c.GetParameters().Count() == x.ConstructorValues.Count()).ToArray();
			if (infos.Count() == 0)
				throw new Exception($"Constructor {x.BossClass} requires inner number of parameters than specified");
			if (infos.Count() >= 2)
				throw new Exception($"Two or more constructors {x.BossClass} found. Cannot continue.");
			PreloadedBosses[x.ID] = bossPackedScenes[x.BossScene].Instantiate() as Boss;
		}
		foreach (var x in info.Stages)
			Stages.Add(new LoadedStage(x, PreloadedBosses));
	}

	public Dictionary<string, Boss> PreloadedBosses = new();
	public List<LoadedStage> Stages = new();
}
