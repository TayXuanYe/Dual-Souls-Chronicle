using Godot;
using System.IO;
using System.Text.Json;

public partial class Main : Control
{
	[Export] private SubViewport _subViewport1;
	[Export] private SubViewport _subViewport2;

	[Export] private PackedScene _setupScene;
	[Export] private PackedScene _loadingScene;
	[Export] private PackedScene _gameScene;

	public override void _Ready()
	{
		// get api key
		if (!GetApiKey())
		{
			return;
		}

		// create setup page and add to sub viewpoint
		Node setupScene1 = _setupScene.Instantiate();
		_subViewport1.AddChild(setupScene1);

		Node setupScene2 = _setupScene.Instantiate();
		_subViewport2.AddChild(setupScene2);
	}

	public bool GetApiKey()
	{
		string configPath = "res://config.json";
		Config config = null;
		try
		{
			string jsonString = File.ReadAllText(ProjectSettings.GlobalizePath(configPath));
			config = JsonSerializer.Deserialize<Config>(jsonString, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}
		catch (FileNotFoundException)
		{
			GD.PrintErr($"Error: Config not found: {configPath}");
			GetTree().Quit();
			return false;
		}
		catch (JsonException e)
		{
			GD.PrintErr($"Error: Failed to parse the configuration file. Please check if the format of {configPath} is correct. Details: {e.Message}");
			GetTree().Quit();
			return false;
		}

		if (config == null || string.IsNullOrWhiteSpace(config.YoutubeApiKey))
		{
			GD.PrintErr("Error: The configuration file content is invalid, API Key is missing.");
			GetTree().Quit();
			return false;
		}
		GD.Print("Configuration read successfully, API Key loaded.");
		YoutubeManager.Instance.YoutubeApiKey = config.YoutubeApiKey;

		return true;
	}

	public void RedirectTo(string groupName, string pageName)
	{
		GD.Print("REDIRECT PROGRESS");
		Node page = null;
		switch (pageName)
		{
			case "LoadingPage":
				page = _loadingScene.Instantiate();
				break;
			case "GamePage":
				page = _gameScene.Instantiate();
				break;
		}
		if (page == null) { return; }
		GD.Print($"REDIRECT TO {page.Name}");

		Node targetViewport = new Node();
		switch (groupName)
		{
			case "IsInViewport1":
				CharacterDataManager.CharacterId1 = "IsInViewport1";
				targetViewport = _subViewport1;
				break;
			case "IsInViewport2":
				CharacterDataManager.CharacterId1 = "IsInViewport2";
				targetViewport = _subViewport2;
				break;
			default:
				break;
				
		}
		GD.Print($"REMOVING NODE {groupName}");
		var childrenToFree = new Godot.Collections.Array<Node>(targetViewport.GetChildren());

		foreach (Node child in childrenToFree)
		{
			if (child.Name != "Data")
			{
				GD.Print($"Removing child: {child.Name}");
				targetViewport.RemoveChild(child);
				child.QueueFree();
			}
		}

		targetViewport.CallDeferred(Node.MethodName.AddChild, page);

		GD.Print("REDIRECT COMPLETED");
	}
}
