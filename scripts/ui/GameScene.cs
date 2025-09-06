using Godot;
using System;

public partial class GameScene : Control
{
	[Export] private Node _selectScene;
	public override void _Ready()
	{
		if (_selectScene is SelectScenes selectSceneScript)
			selectSceneScript.Init(10);
	}
	
	public override void _Process(double delta)
	{
		
	}
}
