using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Godot;

public class LoadedAction
{
	public LoadedAction(Boss _boss)
	{
		Boss = _boss;
	}

	public void CompletePreload(InfoAction info)
	{
		List<object> objs = new();
		Type classType = Assembly.GetExecutingAssembly().GetType(info.ScriptClass);
		if (classType == null)
			throw new Exception($"Class {info.ScriptClass} not found");
		MethodInfo[] methods = classType.GetMethods().Where(method => method.Name.Contains(info.ScriptMethod)).ToArray();
		if (methods.Count() == 0)
		{
			foreach (var x in classType.GetMethods())
			{
				Debug.WriteLine(x.Name);
			}
			throw new Exception($"Method {info.ScriptMethod} not found in {info.ScriptClass}");
		}
		MethodInfo[] methodsWithExcactNumberOfParameters = methods.Where(method => method.GetParameters().Count() == info.Values.Count()).ToArray();
		if (methodsWithExcactNumberOfParameters.Count() == 0)
			throw new Exception($"Method {info.ScriptMethod} requires inner number of parameters than specified");
		if (methodsWithExcactNumberOfParameters.Count() >= 2)
			throw new Exception($"Two or more methods {info.ScriptMethod} found. Cannot continue.");
		var parameters = methodsWithExcactNumberOfParameters[0].GetParameters();
		for (int i = 0; i < info.Values.Count(); i++)
		{
			var x = info.Values[i];
			var type = parameters[i].ParameterType;
			objs.Add(x.ConvertValueTo(type));
		}
		Tick = info.Tick;
		Method = methodsWithExcactNumberOfParameters[0];
		Values = objs.ToArray();
		SetPosition = info.InitialPosType != InfoAction.InitialPositionType.Previous;
		if (SetPosition)
		{
			float rnd = GD.Randf();
			Position = info.InitialPosType == InfoAction.InitialPositionType.Random ? info.InitialRandomPositionStart * rnd + info.InitialRandomPositionEnd * (1 - rnd) : info.InitialPosition;
		}
		if (info.SetHealth)
		{
			BossHealth = info.HealthToSet;
		}
		SetHealth = info.SetHealth;
	}

	public bool SetHealth;
	public Boss Boss;
	public long Tick;
	public MethodInfo Method;
	public object[] Values;
	public bool SetPosition;
	public Vector3 Position;
	public float BossHealth;
}
