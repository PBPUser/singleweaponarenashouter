using System.Text.Json.Serialization;
using Godot;

public class InfoBoss
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
