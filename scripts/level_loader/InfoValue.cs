using System.Text.Json.Serialization;

public class InfoValue
{
    [JsonInclude]
    public string Field;
    [JsonInclude]
    public string Value;
}
