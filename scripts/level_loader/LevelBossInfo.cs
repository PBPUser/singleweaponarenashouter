using System.Text.Json.Serialization;

public class LevelBossInfo
{
    [JsonInclude]
    public string ID;

    [JsonInclude]
    public float BossMinHealthMp;

    [JsonInclude]
    public float BossMaxHealthMp;

    [JsonInclude]
    public DamageModifier[] DamageModifiers;
}
