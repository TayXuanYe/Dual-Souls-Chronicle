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
		SignalManager.Instance.RequestForInti += OnRequestForInitSignalReceipt;
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

	public (bool IsValid, PackedScene packedScene, string type, int nextIndex) GetProgress(int index)
	{
		if (!progressDict.ContainsKey(index))
		{
			return (false, null, null, -1);
		}

		int nextIndex = index + 1;
		if (!progressDict.ContainsKey(nextIndex))
		{
			nextIndex = -1;
		}

		var data = progressDict[index];
		PackedScene packedScene = null;

		switch (data.type)
		{
			case "SelectCharacter":
				packedScene = GD.Load<PackedScene>(data.resourcesPath);
				break;
			case "SelectBuff":
				packedScene = GD.Load<PackedScene>(data.resourcesPath);
				break;
			case "Level":
				packedScene = GD.Load<PackedScene>(data.resourcesPath);
				break;
		}

		bool isValid = packedScene != null;
		return (isValid, packedScene, data.type, nextIndex);
	}
	
	private void OnRequestForInitSignalReceipt(int index, Node node)
	{
		var nodeData = GetProgress(index);
		if (nodeData.IsValid)
		{
			switch (nodeData.type)
			{
				case "SelectCharacter":
					if (node is SelectScenes selectScenesScript1)
					{
						string[] colors = ["#66CCFF", "#FF6666", "#66CC66"];
						selectScenesScript1.Init(60, 3, colors, "character", index, nodeData.nextIndex);
					}
					break;
				case "SelectBuff":
					if (node is SelectScenes selectScenesScript2)
					{
						string[] colors = ["#66CCFF", "#FF6666", "#66CC66"];
						selectScenesScript2.Init(20, 3, colors, "buff", index, nodeData.nextIndex);
					}
					break;
				case "Level":
					if (node is Level levelScript)
					{
						levelScript.Init(nodeData.nextIndex);
					}
					break;
			}
		}
	}
}
