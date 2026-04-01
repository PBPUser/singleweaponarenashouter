using System.Text.Json.Serialization;

public class BossStageInfo
{
    [JsonInclude]
    public string Name;

    [JsonInclude]
    public BossActionInfo[] Actions;
}
