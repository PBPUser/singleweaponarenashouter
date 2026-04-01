using System.Collections.Generic;
using System.Linq;

public class LoadedStage
{
	public LoadedStage(InfoStage info, Dictionary<string, Boss> bosses)
	{
		StageName = info.StageCaption;
		List<LoadedAction> actions = new();
		foreach (var x in info.Actions)
		{
			if (!bosses.ContainsKey(x.ID))
				throw new System.Exception($"Boss with ID {x.ID} not found.");
			LoadedAction action = new(bosses[x.ID]);
			action.CompletePreload(x);
			actions.Add(action);
		}
		Actions = actions.ToArray();
		TimerType = info.Type;
		Length = info.Length;
		List<long> aTicks = new();
		foreach (var x in info.Actions)
			if (!aTicks.Contains(x.Tick))
				aTicks.Add(x.Tick);
		ActivationTicks = aTicks.OrderBy(x => x).ToArray();
	}

	public string StageName;
	public LoadedAction[] Actions;
	public InfoStage.TimerType TimerType;
	public long Length;
	public long[] ActivationTicks;
}
