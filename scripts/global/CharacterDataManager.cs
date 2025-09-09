using System.Threading.Tasks;
using System;
using Godot;
using System.Collections.Generic;

public partial class CharacterDataManager : Node
{
    public static CharacterDataManager Instance;
    public Dictionary<int, CharacterDto> Characters = new Dictionary<int, CharacterDto>();

    public override void _Ready()
    {
        Instance = this;
    }
}