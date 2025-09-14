using Godot;
using System;
using System.Collections.Generic;

public partial class GamaProgressManager : Node
{
	public static GamaProgressManager Instance { get; set; }
	private Dictionary<int, (string type, string resourcesPath)> progressDict = new Dictionary<int, (string type, string resources)>();
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
		progressDict.Add(0, ("SelectCharacter", "res://scenes/ui/select_scenes.tscn"));

		// level 1
		progressDict.Add(1, ("Level", "res://scenes/levels/level1.tscn"));
		// select buff
		progressDict.Add(2, ("SelectBuff", "res://scenes/ui/select_scenes.tscn"));

		// level 2
		progressDict.Add(3, ("Level", "res://scenes/levels/level2.tscn"));
		// select buff
		progressDict.Add(4, ("SelectBuff", "res://scenes/ui/select_scenes.tscn"));

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
		if (!progressDict.ContainsKey(index))
		{
			return (false, null);
		}
		int nextIndex = index + 1;
		if (!progressDict.ContainsKey(nextIndex))
		{
			nextIndex = -1;
		}
		var data = progressDict[index];
		Node node = null;
		switch (data.type)
		{
			case "SelectCharacter":
				node = InitSelectCharacterScene(data.resourcesPath, index, nextIndex);
				break;
			case "SelectBuff":
				node = InitSelectBuffScene(data.resourcesPath, index, nextIndex);
				break;
			case "Level":
				node = InitLevelScene(data.resourcesPath, nextIndex);
				break;
		}

		bool isValid = node != null;
		return (isValid, node);
	}

	private Node InitSelectCharacterScene(string resourcesPath, int seed, int nextIndex)
	{
		Node node = GD.Load<PackedScene>(resourcesPath).Instantiate();
		if (node is SelectScenes script)
		{
			string[] colors = ["#66CCFF", "#FF6666", "#66CC66"];
			script.Init(10, 3, colors, "character", seed, nextIndex);
		}

		return node;
	}

	private Node InitSelectBuffScene(string resourcesPath, int seed, int nextIndex)
	{
		Node node = GD.Load<PackedScene>(resourcesPath).Instantiate();
		if (node is SelectScenes script)
		{
			string[] colors = ["#66CCFF", "#FF6666", "#66CC66"];
			script.Init(20, 3, colors, "buff", seed, nextIndex);
		}

		return node;
	}
	
	private Node InitLevelScene(string resourcesPath, int nextIndex)
	{
		Node node = GD.Load<PackedScene>(resourcesPath).Instantiate();
		if (node is Level script)
		{
			script.Init(nextIndex);
		}
		return node;
	}
}
