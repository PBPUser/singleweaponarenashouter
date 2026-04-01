using System.Text.Json.Serialization;
using Godot;

public class InfoAction
{
    [JsonInclude]
    public long Tick;

    [JsonInclude]
    public string ID;

    [JsonInclude]
    public string ScriptClass;

    [JsonInclude]
    public string ScriptMethod;

    [JsonInclude]
    public InfoValue[] Values = new InfoValue[0];

    [JsonInclude]
    public Vector3 InitialPosition;

    [JsonInclude]
    public Vector3 InitialRandomPositionStart;

    [JsonInclude]
    public Vector3 InitialRandomPositionEnd;

    [JsonInclude]
    public InitialPositionType InitialPosType = InitialPositionType.Previous;

    public enum InitialPositionType
    {
        Random = 0,
        Static = 1,
        Previous = 2
    }
}
