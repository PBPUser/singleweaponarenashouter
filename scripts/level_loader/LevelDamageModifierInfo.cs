using System.Text.Json.Serialization;

public class LevelDamageModifierInfo
{
    [JsonInclude]
    public float Chance;

    [JsonInclude]
    public InfoValue[] Variables;
}
