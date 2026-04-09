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
		if (info.BossRequiredToDie == InfoStage.RequiresToDie.ExactBoss)
		{
			if (!bosses.ContainsKey(info.ExactDieBossID))
				throw new System.Exception($"Can't find boss with id {info.ExactDieBossID}");
			BossRequiredToDie = info.ExactDieBossID;
		}
		RequiredBossTypeDie = info.BossRequiredToDie;
		ActivationTicks = aTicks.OrderBy(x => x).ToArray();
		BossHealthSet = new LoadedBossStageInfo[info.SetBosses.Length];
		for (int i = 0; i < BossHealthSet.Length; i++)
		{
			BossHealthSet[i] = new LoadedBossStageInfo(info.SetBosses[i], bosses);
		}
	}

	public string StageName;
	public LoadedAction[] Actions;
	public InfoStage.TimerType TimerType;
	public long Length;
	public long[] ActivationTicks;
	public string BossRequiredToDie;
	public InfoStage.RequiresToDie RequiredBossTypeDie;
	public LoadedBossStageInfo[] BossHealthSet;
}
