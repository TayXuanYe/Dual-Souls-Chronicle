using Godot;
using System;

public partial class SignalManager : Node
{
	public static SignalManager Instance { get; private set; }
	[Signal]
	public delegate void DisplayDialogEventHandler(string message, string parentGroupName);
	[Signal]
	public delegate void SelectCharacterEventHandler(string role, string parentGroupName);
	[Signal]
	public delegate void SelectBuffEventHandler(string buffId, string parentGroupName);
	[Signal]
	public delegate void ShowSelectAnimationEventHandler(string animation);
	[Signal]
	public delegate void AddBuffCharacterEventHandler(string buffId, string parentGroupName);
	[Signal]
	public delegate void RemoveBuffCharacterEventHandler(string buffId, string parentGroupName);
	[Signal]
	public delegate void UpdateAllPlayerDataEventHandler();
	[Signal]
	public delegate void SluggishnessCharacterEventHandler(string characterId);
	[Signal]
	public delegate void NextProgressEventHandler(string id, int index);

	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;
	}

	public void EmitChatSignal(string message, string parentGroupName)
	{
		EmitSignal(SignalName.DisplayDialog, message, parentGroupName);
	}

	public void EmitSelectCharacterSignal(string role, string parentGroupName)
	{
		EmitSignal(SignalName.SelectCharacter, role, parentGroupName);
	}

	public void EmitSelectBuffSignal(string buffId, string parentGroupName)
	{
		EmitSignal(SignalName.SelectBuff, buffId, parentGroupName);
	}

	public void EmitShowSelectAnimationSignal(string animation)
	{
		EmitSignal(SignalName.ShowSelectAnimation, animation);
	}

	public void EmitAddBuffCharacterSignal(string buffId, string parentGroupName)
	{
		EmitSignal(SignalName.AddBuffCharacter, buffId, parentGroupName);
	}

	public void EmitRemoveBuffCharacterSignal(string buffId, string parentGroupName)
	{
		EmitSignal(SignalName.RemoveBuffCharacter, buffId, parentGroupName);
	}

	public void EmitUpdateAllPlayerDataSignal()
	{
		EmitSignal(SignalName.UpdateAllPlayerData);
	}

	public void EmitSluggishnessCharacterSignal(string characterId)
	{
		EmitSignal(SignalName.SluggishnessCharacter, characterId);
	}

	public void EmitNextProgressSignal(string id, int index)
	{
		EmitSignal(SignalName.NextProgress, id, index);
	}
}
