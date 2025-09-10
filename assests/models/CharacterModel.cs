using System.Collections.Generic;

public class CharacterModel
{
    public string Id { get; set; }
    public string CharacterName { get; set; }
    public string Describe { get; set; }
    public string ImagePath { get; set; }
    public enum Role
    {
        None,
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
    public CharacterModel(Role characterRole, int hpLimit, int hp, int attack, int defense, string describe, string imagePath)
    {
        CharacterRole = characterRole;
        HpLimit = hpLimit;
        Hp = hp;
        Attack = attack;
        Defense = defense;
        Describe = describe;
        ImagePath = imagePath;
    }
    public void Init(CharacterModel characterDto)
    {
        
        CharacterRole = characterDto.CharacterRole;
        HpLimit = characterDto.HpLimit;
        Hp = characterDto.Hp;
        Attack = characterDto.Attack;
        Defense = characterDto.Defense;
        Describe = characterDto.Describe;
        ImagePath = characterDto.ImagePath;
    }
    public bool IsAssignRole()
    {
        return CharacterRole != Role.None;
    }
}