using System.Numerics;
using System.Text.Json.Serialization;

public class InfoStageBoss
{
    [JsonInclude]
    public float Value;
    [JsonInclude]
    public string ID;
    [JsonInclude]
    public Vector3? Position = null;
}
