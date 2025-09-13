using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;

public partial class Level : Node
{
	private int _pointer = 0;
	private List<Entity> _character = new List<Entity>();
	private List<Entity> _enemy = new List<Entity>();
	[Export] private PackedScene _warriorScene = GD.Load<PackedScene>("res://scenes/entity/character/warrior.tscn");
	[Export] private PackedScene _shieldGuardScene= GD.Load<PackedScene>("res://scenes/entity/character/shield_guard.tscn");
	[Export] private PackedScene _mageScene = GD.Load<PackedScene>("res://scenes/entity/character/mage.tscn");
	[Export] private PackedScene _slimeScene = GD.Load<PackedScene>("res://scenes/entity/character/mage.tscn");
	private async void Process()
	{
		while (true)
		{
			foreach (Entity character in _character)
			{
				if (character.Hp != 0)
				{
					Entity enemy = GetAttackEnemy();
					character.AttackEntity(enemy);
					await ToSignal(GetTree().CreateTimer(2.0), Timer.SignalName.Timeout);
				}
			}

			foreach (Entity enemy in _enemy)
			{
				if (enemy.Hp != 0)
				{
					Entity character = GetAttackCharacter();
					enemy.AttackEntity(character);
					await ToSignal(GetTree().CreateTimer(2.0), Timer.SignalName.Timeout);
					SignalManager.Instance.EmitUpdateAllPlayerDataSignal();
				}
			}
		}
	}

	public override void _Process(double delta)
	{
		if (CharacterDataManager.Instance.Characters["IsInViewport1"].Hp == 0 && CharacterDataManager.Instance.Characters["IsInViewport2"].Hp == 0)
		{
			SignalManager.Instance.EmitNextProgressSignal(NodeUtility.GetParentNodeGroup(this, "IsInViewport1", "IsInViewport2"),1);
		}

		bool allEnemyKill = true;
		foreach (Entity enemy in _enemy)
		{
			if (enemy.Hp > 0)
			{
				allEnemyKill = false;
			}
		}
		if (allEnemyKill)
		{
			SignalManager.Instance.EmitNextProgressSignal(NodeUtility.GetParentNodeGroup(this, "IsInViewport1", "IsInViewport2"),-1);


		}

	}

	private Entity GetAttackEnemy()
	{
		foreach (Entity enemy in _enemy)
		{
			if (enemy.Hp != 0)
			{
				return enemy;
			}
		}
		return null;
	}

	private Entity GetAttackCharacter()
	{
		foreach (Entity character in _character)
		{
			if (character.Hp != 0)
			{
				return character;
			}
		}
		return null;
	}

	public override void _Ready()
	{
		// spawn entity
		SpawnCharacterByName(
			CharacterDataManager.Instance.Characters["IsInViewport1"].CharacterRole.ToString(),
			CharacterDataManager.Instance.Characters["IsInViewport1"].Id,
			new Vector2(135, 160));
		SpawnCharacterByName(
			CharacterDataManager.Instance.Characters["IsInViewport2"].CharacterRole.ToString(),
			CharacterDataManager.Instance.Characters["IsInViewport2"].Id,
			new Vector2(405, 160));
		Node slime = _slimeScene.Instantiate();
		if (slime is Slime slimeScript)
		{
			_enemy.Add(slimeScript);
			slimeScript.Init("slime1", new Vector2(1000, 270));
		}
		Process();
	}

	public void SpawnCharacterByName(string name, string id, Vector2 position)
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

}
