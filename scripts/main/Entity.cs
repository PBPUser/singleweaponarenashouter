using System.Collections.Generic;
using Godot;

public partial class Entity : CharacterBody3D
{
    bool autoDie = true;
    ///<summary>
    /// Set IsDied to true when Health reaches 0
    /// </summary>
    public bool AutoDie
    {
        get => autoDie;
        set => autoDie = value;
    }

    float health = 20.0f;
    public float Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthChanged(value);
            if (health <= 0)
                if (autoDie)
                    IsDied = true;
        }
    }

    bool isDied = false;
    public bool IsDied
    {
        get => isDied;
        set
        {
            isDied = value;
            Died(value);
        }
    }

    public virtual void OnHealthChanged(float newHealth)
    {

    }

    public virtual void Died(bool isDied)
    {

    }

    public void Damage(float damage, List<DamageModifier> damageModifiers)
    {
        foreach (DamageModifier x in damageModifiers)
        {
            damage = x.ModifyDamage(damage, this);
        }
        Health -= damage;
    }

    float fallingDamage = 0;

    public override void _Process(double delta)
    {
        if (Position.Y < -100)
        {
            Health -= (float)delta * fallingDamage;
            fallingDamage += (float)delta;
        }
    }
}
