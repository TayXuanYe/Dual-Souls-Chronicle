using Godot;
using System;

public partial class Container : Node
{
	public void ChangeScene(PackedScene scene, int index)
	{
		// remove all existing children
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}
		// instance and add the new scene
		Node newScene = scene.Instantiate();
		SignalManager.Instance.EmitRequestForInitSignal(index, newScene);
		AddChild(newScene);
	}
}
