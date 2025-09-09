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
}