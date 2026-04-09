using System.Text.Json.Serialization;

public class InfoStage
{
    [JsonInclude]
    public string StageCaption;

    [JsonInclude]
    public InfoAction[] Actions;

    public enum RequiresToDie
    {
        AnyBoss = 0,
        ExactBoss = 1,
        AllBosses = 2
    }

    public enum TimerType
    {
        Limited = 0,
        LoopUntilDeath = 1
    }

    [JsonInclude]
    public TimerType Type;

    [JsonInclude]
    public RequiresToDie BossRequiredToDie = RequiresToDie.AnyBoss;

    [JsonInclude]
    public string ExactDieBossID;

    [JsonInclude]
    public long Length;

    [JsonInclude]
    public InfoStageBoss[] SetBosses;
}
