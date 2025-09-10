using System.Threading.Tasks;
using System;
using Godot;
using System.Collections.Generic;

public partial class CharacterDataManager : Node
{
    public static CharacterDataManager Instance;
    public Dictionary<int, CharacterDto> Characters = new Dictionary<int, CharacterDto>();

    private readonly static CharacterDto _warrior = new CharacterDto
    (
        CharacterDto.Role.Warrior,
        100,
        100,
        100,
        100
    );
    private readonly static CharacterDto _mage = new CharacterDto
    (
        CharacterDto.Role.Mage,
        50,
        50,
        200,
        50
    );
    private readonly static CharacterDto _shieldGuard = new CharacterDto
    (
        CharacterDto.Role.ShieldGuard,
        200,
        200,
        30,
        200
    );

    public override void _Ready()
    {
        Instance = this;
        SignalManager.Instance.SelectCharacter += OnSelectCharacterSignalReceipt;
    }

    private void OnSelectCharacterSignalReceipt(string roleData, int id)
    {
        if (!Characters.ContainsKey(id)) { return; }
        string role = roleData.Split('_')[1];
        var characterDto = Characters[id];
        CharacterDto.Role roleEnum;
        if (Enum.TryParse(role, true, out roleEnum))
        {
            InitCharacter(characterDto, roleEnum);
        }
    }

    private void InitCharacter(CharacterDto characterDto, CharacterDto.Role role)
    {
        switch (role)
        {
            case CharacterDto.Role.Warrior:
                characterDto.Init(_warrior);
                break;
            case CharacterDto.Role.Mage:
                characterDto.Init(_mage);
                break;
            case CharacterDto.Role.ShieldGuard:
                characterDto.Init(_shieldGuard);
                break;
            default:
                break;
        }
    }
}