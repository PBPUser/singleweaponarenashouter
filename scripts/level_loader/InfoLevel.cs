using System.Text.Json.Serialization;

public class InfoLevel
{
    [JsonInclude]
    public PreloadInfoBoss[] Bosses;

    [JsonInclude]
    public InfoStage[] Stages;
}
