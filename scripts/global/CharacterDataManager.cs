using System.Threading.Tasks;
using System;
using Godot;
using System.Collections.Generic;

public partial class CharacterDataManager : Node
{
    public static CharacterDataManager Instance;
    public List<(CharacterDto character, int id)> Characters = new List<(CharacterDto character, int id)>();

    public override void _Ready()
    {
        Instance = this;
    }
}