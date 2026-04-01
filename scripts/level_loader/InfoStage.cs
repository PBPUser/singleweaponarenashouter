using System.Text.Json.Serialization;

public class InfoStage
{
    [JsonInclude]
    public string StageCaption;

    [JsonInclude]
    public InfoAction[] Actions;

    public enum TimerType
    {
        Limited = 0,
        LoopUntilDeath = 1
    }

    [JsonInclude]
    public TimerType Type;

    [JsonInclude]
    public long Length;
}
