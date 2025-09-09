using Godot;
using System;

public partial class SignalManager : Node
{
	public static SignalManager Instance { get; private set; }
	[Signal]
	public delegate void DisplayDialogEventHandler(string message, int id);

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
}
