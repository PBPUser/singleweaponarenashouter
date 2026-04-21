public class DamageModifier
{
    public string ID;

    public virtual float ModifyDamage(float damage, Entity entity) => damage;
}
