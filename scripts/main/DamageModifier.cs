public class DamageModifier
{
	public string ID;
	public string[] DamageWhiteTags = { };
	public string[] DamageBlackTags = { };

	public virtual float ModifyDamage(float damage, Entity entity)
	{
		return damage;
	}
}
