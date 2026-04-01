using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Godot;


///<summary>
/// Loads Level
/// </summary>
public class LevelLoader
{
    static LevelLoader()
    {
        var infos = DirAccess.GetFilesAt("res:///levels/").Select<string, InfoLevel>(x => JsonSerializer.Deserialize<InfoLevel>(FileAccess.GetFileAsString($"res://levels/{x}"))).ToArray();
        foreach (var x in infos)
            if (x == null)
                throw new System.Exception($"Failed to read {x}");
        Infos = infos;
        Files = DirAccess.GetFilesAt("res:///levels/");
        foreach (var x in Files)
            Debug.WriteLine(x);
    }

    public static string[] Files;
    public static InfoLevel[] Infos;
}
