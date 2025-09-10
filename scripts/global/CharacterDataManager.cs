using System.Threading.Tasks;
using System;
using Godot;
using System.Collections.Generic;

public partial class CharacterDataManager : Node
{
    public static CharacterDataManager Instance;
    public Dictionary<string, CharacterModel> Characters = new Dictionary<string, CharacterModel>();

    public readonly static CharacterModel Warrior = new CharacterModel
    (
        "Warrior",
        CharacterModel.Role.Warrior,
        100,
        100,
        100,
        100,
        "Average, attack, defense and health are balanced",
        "res://assests/textures/character/warrior/warrior_normal.png"
    );
    public readonly static CharacterModel Mage = new CharacterModel
    (
        "Mega",
        CharacterModel.Role.Mage,
        50,
        50,
        200,
        50,
        "High damage but super squishy",
        "res://assests/textures/character/mega/mega_normal.png"
    );
    public readonly static CharacterModel ShieldGuard = new CharacterModel
    (
        "ShieldGuard",
        CharacterModel.Role.ShieldGuard,
        200,
        200,
        30,
        200,
        "Low damage but super tanky",
        "res://assests/textures/character/shieldguard/shieldguard_normal.png"
    );

    public override void _Ready()
    {
        Instance = this;
        SignalManager.Instance.SelectCharacter += OnSelectCharacterSignalReceipt;
        SignalManager.Instance.AddBuffCharacter += OnAddBuffCharacterSignalReceipt;
    }

    private void OnSelectCharacterSignalReceipt(string name, string groupName)
    {
        if (!Characters.ContainsKey(groupName)) { return; }
        string role = name.Replace(" ","");
        var characterDto = Characters[groupName];
        CharacterModel.Role roleEnum;
        if (Enum.TryParse(role, true, out roleEnum))
        {
            InitCharacter(characterDto, roleEnum);
        }
    }

    private void InitCharacter(CharacterModel characterDto, CharacterModel.Role role)
    {
        switch (role)
        {
            case CharacterModel.Role.Warrior:
                characterDto.Init(Warrior);
                break;
            case CharacterModel.Role.Mage:
                characterDto.Init(Mage);
                break;
            case CharacterModel.Role.ShieldGuard:
                characterDto.Init(ShieldGuard);
                break;
            default:
                break;
        }
    }

    private void OnAddBuffCharacterSignalReceipt(string buffId, string parentGroupName)
    {
        if (Characters.TryGetValue(parentGroupName, out var character))
        {
            if (BuffManager.Instance.Buffs.TryGetValue(buffId, out var buff))
            {
            character.Buff.Add(buff);
            }
            else
            {
            GD.PrintErr($"Buff with ID '{buffId}' not found.");
            }
        }
        else
        {
            GD.PrintErr($"Character group '{parentGroupName}' not found.");
        }
    }
}