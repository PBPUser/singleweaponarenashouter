using System.Text.Json.Serialization;

public class PreloadInfoBoss
{
    [JsonInclude]
    public string ID;
    [JsonInclude]
    public string BossClass;
    [JsonInclude]
    public string BossScene;
    [JsonInclude]
    public InfoValue[] ConstructorValues = new InfoValue[0];
}
