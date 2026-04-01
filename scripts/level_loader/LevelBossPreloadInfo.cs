using System.Text.Json.Serialization;

public class LevelBossPreloadInfo
{
    [JsonInclude]
    public string ID;
    [JsonInclude]
    public string BossClass;
    [JsonInclude]
    public string BossAssetModel;
}
