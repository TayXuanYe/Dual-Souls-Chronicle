using Godot;
using System;

public partial class GameScene : Control
{
	[Export] private Node _selectScene;
	public override void _Ready()
	{
		if (_selectScene is SelectScenes selectSceneScript)
		{
			string[] colors = ["#66CCFF", "#FFEED0", "#eeff00ff"];
			selectSceneScript.Init(0, 10, colors, 3, "buff");
			selectSceneScript.SetPosition(new Vector2(0, selectSceneScript.Position.Y));
		}

	}

	private double _timeSinceLastUpdate = 0;

	public override void _Process(double delta)
	{
		// 累加时间
		_timeSinceLastUpdate += delta;

		// 当累积时间超过或等于 1.0 秒时
		if (_selectScene is SelectScenes selectSceneScript)
		{
			if (_timeSinceLastUpdate >= 1.0)
			{
				// 重置计时器，并保留超出的时间以确保精确
				_timeSinceLastUpdate -= 1.0;

				// 在这里放置你想要每秒执行一次的代码
				int[] voteCount = new int[] { GD.RandRange(0, 10), GD.RandRange(0, 10), GD.RandRange(0, 10) };
				selectSceneScript.UpdateVoteCount(voteCount);
			}
		}
	}
}
