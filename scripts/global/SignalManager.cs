using Godot;
using System;

public partial class SignalManager : Node
{
	public static SignalManager Instance { get; private set; }
	[Signal]
	public delegate void DisplayDialogEventHandler(string message, int id);
	[Signal]
	public delegate void SelectCharacterEventHandler(string roleData, string parentGroupName);
	[Signal]
	public delegate void SelectBuffEventHandler(string buffId, int id);

	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;
	}

	public void EmitChatSignal(string message, int id)
	{
		EmitSignal(SignalName.DisplayDialog, message, id);
	}

	public void EmitSelectCharacterSignal(string roleData, string parentGroupName)
	{
		EmitSignal(SignalName.SelectCharacter, roleData, parentGroupName);
	}

	public void EmitSelectBuffSignal(string buffId, int id)
	{
		EmitSignal(SignalName.SelectBuff, buffId, id);
	}
}
