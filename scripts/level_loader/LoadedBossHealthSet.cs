using System.Collections.Generic;
using System.Numerics;

public class LoadedBossStageInfo
{
    public LoadedBossStageInfo(InfoStageBoss ibh, Dictionary<string, Boss> bosses)
    {
        Boss = bosses[ibh.ID];
        Value = ibh.Value;
        Position = ibh.Position;
    }

    public float Value;
    public Boss Boss;
    public Vector3? Position;
}
