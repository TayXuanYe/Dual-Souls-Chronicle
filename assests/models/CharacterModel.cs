using System.Collections.Generic;

public class CharacterModel
{
    public string CharacterName { get; set; }
    public enum Role
    {
        Warrior,
        Mage,
        ShieldGuard
    }
    public Role CharacterRole { get; set; }
    public int HpLimit { get; set; }
    public int Hp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public float CriticalRate { get; set; } = 0f; 
    public float CriticalDamage { get; set; } = 1f;
    public float DodgeRate { get; set; } = 0f;
    public bool HaveKnockoffUndyingTotem { get; set; } = false;
    public bool HaveUndyingTotem { get; set; } = false;
    public List<BuffModel> Buff { get; set; }

    public CharacterModel() { }
    public CharacterModel(Role characterRole, int hpLimit, int hp, int attack, int defense)
    {
        CharacterRole = characterRole;
        HpLimit = hpLimit;
        Hp = hp;
        this.Attack = attack;
        Defense = defense;
    }
    public void Init(CharacterModel characterDto)
    {
        CharacterRole = characterDto.CharacterRole;
        HpLimit = characterDto.HpLimit;
        Hp = characterDto.Hp;
        Attack = characterDto.Attack;
        Defense = characterDto.Defense;
    }
}