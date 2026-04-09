using System.Diagnostics;

public partial class Boss : Enemy
{
    string name;
    public string DisplayName
    {
        get => name;
        set => name = value;
    }

    string bossId;
    public string BossID
    {
        get => bossId;
        set => bossId = value;
    }

    bool isSpawned;
    public bool IsSpawned
    {
        get => isSpawned;
        set => isSpawned = value;
    }

    public override void Died(bool isDied)
    {
        if (isDied)
        {
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
            Gameplay.Current.BossDied(bossId);
        }
        else
        {
            Visible = true;
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}
