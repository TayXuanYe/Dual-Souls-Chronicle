using Godot;
using System;

public abstract partial class Entity : Node2D
{
    protected abstract void PlayAnimation(string animName, Vector2 position);
    protected abstract void PlayAnimation(string animName);
    public abstract void AttackEntity(Entity entity);
    public abstract void Attacked(int damage);
    protected abstract void OnAnimationFinished();
    protected int CalculateDamage(float attack, float enemyDefense, float criticalDamage, float criticalRate)
    {
        if (criticalRate < 0.0f) criticalRate = 0.0f;
        if (criticalRate > 1.0f) criticalRate = 1.0f;
        bool triggerCritical = GD.Randf() <= criticalRate;
        float damage;
        if (triggerCritical)
            damage = attack * (1 - (enemyDefense / (enemyDefense + 300))) * criticalDamage;
        else
            damage = attack * (1 - (enemyDefense / (enemyDefense + 300)));
        return (int)damage;
    }

    public int HpLimit { get; set; }
    public int Hp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public float CriticalRate { get; set; } = 0f;
    public float CriticalDamage { get; set; } = 1f;
    [Export] protected AnimatedSprite2D _animatedSprite;
    [Export] protected PackedScene _damageLabelScene = GD.Load<PackedScene>("res://scenes/entity/DamageLabel.tscn");
    public string Id { get; protected set; }
    public Vector2 OriginGlobalPosition { get; set; }
}
