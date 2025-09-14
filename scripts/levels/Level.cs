using Godot;
using System.Collections.Generic;
using System;

public partial class Level : Node
{
    protected int _nextIndex;
    protected int _failSceneIndex;
    protected List<Entity> _character = new List<Entity>();
    protected List<Entity> _enemy = new List<Entity>();
    [Export] protected PackedScene _warriorScene = GD.Load<PackedScene>("res://scenes/entity/character/warrior.tscn");
    [Export] protected PackedScene _shieldGuardScene = GD.Load<PackedScene>("res://scenes/entity/character/shield_guard.tscn");
    [Export] protected PackedScene _mageScene = GD.Load<PackedScene>("res://scenes/entity/character/mage.tscn");

    protected void SpawnCharacterByName(string name, string id, Vector2 position)
    {
        switch (name)
        {
            case "Warrior":
                Node warriorNode = _warriorScene.Instantiate();
                if (warriorNode is Warrior scriptWarrior)
                {
                    _character.Add(scriptWarrior);
                    scriptWarrior.Init(id, position);
                }
                AddChild(warriorNode);
                break;
            case "Mega":
                Node mageNode = _mageScene.Instantiate();
                if (mageNode is Mage scriptMage)
                {
                    _character.Add(scriptMage);
                    scriptMage.Init(id, position);
                }
                AddChild(mageNode);
                break;
            case "ShieldGuard":
                Node shieldGuardNode = _shieldGuardScene.Instantiate();
                if (shieldGuardNode is ShieldGuard scriptShieldGuard)
                {
                    _character.Add(scriptShieldGuard);
                    scriptShieldGuard.Init(id, position);
                }
                AddChild(shieldGuardNode);
                break;
        }
    }

    public void Init(int nextIndex)
    {
        _nextIndex = nextIndex;
    }
}
