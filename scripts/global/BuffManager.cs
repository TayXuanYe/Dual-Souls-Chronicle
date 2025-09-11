using Godot;
using System;
using System.Collections.Generic;

public partial class BuffManager : Node
{
    public static BuffManager Instance { get; set; }
    public Dictionary<string, BuffModel> Buffs = new Dictionary<string, BuffModel>();
    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }
        Instance = this;
        LoadBuffs();
    }

    private void LoadBuffs()
    {
        CreateBuffChestplate();
        CreateBuffSharpIV();
        CreateBuffSharpV();
        CreateBuffRustBlade();
        CreateBuffGoldenApple();
        CreateBuffHealingPotion();
        CreateBuffKnockoffUndyingTotem();
        CreateBuffShadeCloak();
        CreateBuffUndyingTotem();
    }

    private void CreateBuffChestplate()
    {
        string name = "Chestplate";
        string id = $"buff_{name.ToLower()}";
        string describe =
        @"As hard as steel
Defense +20";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.Defense += 20;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.Defense -= 20;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffSharpIV()
    {
        string name = "Sharpness IV";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        @"Any Sharpness V?
Attack +15";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.Attack += 15;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.Attack -= 15;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffSharpV()
    {
        string name = "Sharpness V";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        @"Yap I am here!
Attack +20";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.Attack += 20;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.Attack -= 20;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffRustBlade()
    {
        string name = "Rust Blade";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        @"A rusted blade with a vicious edge.
Attack -10
CRITRate +20%
CRITDamage +150%";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.Attack -= 10;
            character.CriticalRate += 0.2f;
            character.CriticalDamage += 1.5f;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.Attack += 10;
            character.CriticalRate -= 0.2f;
            character.CriticalDamage -= 1.5f;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffGoldenApple()
    {
        string name = "Golden Apple";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        @"An exquisite apple made of pure gold. 
HP limit +15
HP +15";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.HpLimit += 15;
            character.Hp += 15;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.HpLimit -= 15;
            character.Hp -= 15;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffHealingPotion()
    {
        string name = "Healing potion";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        @"Instantly restores health upon consumption.
HP +50";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            if (character.Hp + 50 > character.HpLimit)
            {
                character.Hp = character.HpLimit;
            }
            else
            {
                character.Hp += 50;
            }
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {

        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffKnockoffUndyingTotem()
    {
        string name = "Knockoff Totem";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        "A cheap imitation of the legendary Undying Totem. It can save you from a fatal blow once.";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.HaveKnockoffUndyingTotem = true;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.HaveKnockoffUndyingTotem = false;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffUndyingTotem()
    {
        string name = "Undying Totem";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        @"Averting death and reborn.
*All buff will be remove.";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.HaveUndyingTotem = true;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.HaveUndyingTotem = false;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
    private void CreateBuffShadeCloak()
    {
        string name = "Shade Cloak";
        string id = $"buff_{name.ToLower().Replace(" ", "_")}";
        string describe =
        @"A cloak woven from pure darkness.
new Dodge rate +20%";
        Action<CharacterModel> onApply = (CharacterModel character) =>
        {
            character.DodgeRate += 0.2f;
        };
        Action<CharacterModel> onRemove = (CharacterModel character) =>
        {
            character.DodgeRate -= 0.2f;
        };
        string imagePath = $"res://assests/textures/buff/buff_{name.ToLower().Replace(" ", "_")}.png";
        var buffModel = new BuffModel(id, name, describe, onApply, onRemove, imagePath);
        Buffs.Add(id, buffModel);
    }
}
