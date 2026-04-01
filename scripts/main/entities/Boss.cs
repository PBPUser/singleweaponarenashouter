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
}
