using System.Collections.Generic;

public class CharacterDto
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
    public List<string> Buff { get; set; }

    public CharacterDto() { }
    public CharacterDto(Role characterRole, int hpLimit, int hp, int attack, int defense)
    {
        CharacterRole = characterRole;
        HpLimit = hpLimit;
        Hp = hp;
        Attack = attack;
        Defense = defense;
    }
    public void Init(CharacterDto characterDto)
    {
        CharacterRole = characterDto.CharacterRole;
        HpLimit = characterDto.HpLimit;
        Hp = characterDto.Hp;
        Attack = characterDto.Attack;
        Defense = characterDto.Defense;
    }
}