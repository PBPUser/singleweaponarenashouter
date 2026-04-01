using System.Text.Json.Serialization;

public class BossActionInfo
{
    [JsonInclude]
    public long Tick;

    [JsonInclude]
    public string ID;

    [JsonInclude]
    public string ScriptClass;

    [JsonInclude]
    public InfoValue[] Values;
}
