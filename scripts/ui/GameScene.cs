using Godot;
using System;

public partial class GameScene : Control
{
	[Export] private Node _selectScene;
	public override void _Ready()
	{
		if (_selectScene is SelectScenes selectSceneScript)
		{
			// selectSceneScript.Init(10);
			selectSceneScript.SetPosition(new Vector2(0, selectSceneScript.Position.Y));
		}
		
	}
	
	public override void _Process(double delta)
	{
		if (_selectScene is SelectScenes selectSceneScript)
		{
			if(Math.Floor(delta) % 2 == 0)
			{
				// selectSceneScript.UpdateVoteCount(GD.RandRange(0,10),GD.RandRange(0,10),GD.RandRange(0,10));
			}
		}
	}
}
