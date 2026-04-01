using System.Text.Json.Serialization;

public class InfoDamageModifier
{
    [JsonInclude]
    public float Chance;

    [JsonInclude]
    public InfoValue[] Variables;
}
