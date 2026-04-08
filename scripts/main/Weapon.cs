using Godot;

public partial class Weapon : StaticBody3D
{
    public virtual void Attack(Gameplay gameplay, Player player)
    {

    }

    public virtual float ComputeAttackDelay(Gameplay gameplay)
    {
        return .2f;
    }
}
