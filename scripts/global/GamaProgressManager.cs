using Godot;
using System;
using System.Collections.Generic;

public partial class GamaProgressManager : Node
{
	public static GamaProgressManager Instance { get; set; }
	private Dictionary<int, (string type, string resourcesPath)> progressList = new Dictionary<int, (string type, string resources)>();
	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;
		LoadGameProgress();
	}

	private void LoadGameProgress()
	{
		// select character
		progressList.Add(0, ("SelectCharacter", "res://scenes/ui/select_scenes.tscn"));

		// level 1
		progressList.Add(1, ("Level", "res://scenes/levels/level1.tscn"));
		// select buff
		progressList.Add(2, ("SelectBuff", "res://scenes/ui/select_scenes.tscn"));

		// level 2

		// select buff
		progressList.Add(2, ("SelectBuff", "res://scenes/ui/select_scenes.tscn"));

		// level 3 (boss)

		// select boss buff

		// level 4 

		// select buff

		// level 5
		// select buff

		// level 6 (final boss)

		// fail scene
		// success scene
	}

	// this for select scene
	public (bool IsValid, Node node) GetProgress(int index)
	{
		if (index >= progressList.Count)
		{
			return (false, null);
		}

		var data = progressList[index];
		Node node = null;
		switch (data.type)
		{
			case "SelectCharacter":
				node = InitSelectCharacterScene(data.resourcesPath, index);
				break;
			case "SelectBuff":
				node = InitSelectBuffScene(data.resourcesPath, index);
				break;
			case "Level":
				node = InitLevelScene(data.resourcesPath);
				break;
		}

		bool isValid = node != null;
		return (isValid, node);
	}

	private Node InitSelectCharacterScene(string resourcesPath, int seed)
	{
		Node node = GD.Load<PackedScene>(resourcesPath).Instantiate();
		if (node is SelectScenes script)
		{
			string[] colors = ["#66CCFF", "#FF6666", "#66CC66"];
			script.Init(60, 3, colors, "character", seed);
		}

		return node;
	}

	private Node InitSelectBuffScene(string resourcesPath, int seed)
	{
		Node node = GD.Load<PackedScene>(resourcesPath).Instantiate();
		if (node is SelectScenes script)
		{
			string[] colors = ["#66CCFF", "#FF6666", "#66CC66"];
			script.Init(20, 3, colors, "buff", seed);
		}

		return node;
	}
	
	private Node InitLevelScene(string resourcesPath)
	{
		Node node = GD.Load<PackedScene>(resourcesPath).Instantiate();
		return node;
	}
}
