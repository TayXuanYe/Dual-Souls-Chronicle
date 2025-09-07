using Godot;
using System.IO;
using System.Text.Json;

public partial class Main : Control
{
	[Export] private SubViewport _subViewport1;
	[Export] private SubViewport _subViewport2;

	[Export] private PackedScene _setupScene;
	[Export] private PackedScene _loadingScene;

	public override void _Ready()
	{
		// get api key
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
			return;
		}
		catch (JsonException e)
		{
			GD.PrintErr($"Error: Failed to parse the configuration file. Please check if the format of {configPath} is correct. Details: {e.Message}");
			GetTree().Quit();
			return;
		}

		if (config == null || string.IsNullOrWhiteSpace(config.YoutubeApiKey))
		{
			GD.PrintErr("Error: The configuration file content is invalid, API Key is missing.");
			GetTree().Quit();
			return;
		}
		GD.Print("Configuration read successfully, API Key loaded.");
		YoutubeManager.Instance.YoutubeApiKey = config.YoutubeApiKey;

		// create setup page and add to sub viewpoint
		Node setupScene1 = _setupScene.Instantiate();
		_subViewport1.AddChild(setupScene1);
		if (setupScene1 is SetupPage setupPageScript1)
			setupPageScript1.Init(1);


		Node setupScene2 = _setupScene.Instantiate();
		_subViewport2.AddChild(setupScene2);
		if (setupScene2 is SetupPage setupPageScript2)
			setupPageScript2.Init(2);
	}

	public void RedirectTo(int viewportId, string pageName)
	{
		GD.Print("REDIRECT PROGRESS");
		Node page = null;
		switch (pageName)
		{
			case "LoadingPage":
				page = _loadingScene.Instantiate();
				break;
		}
		if (page == null) { return; }
		GD.Print($"REDIRECT TO {page.Name}");
		GD.Print(viewportId);

		Node targetViewport;
		if (viewportId == 1)
		{
			targetViewport = _subViewport1;
		}
		else if (viewportId == 2)
		{
			targetViewport = _subViewport2;
		}
		else
		{
			return;
		}

		var childrenToFree = new Godot.Collections.Array<Node>(targetViewport.GetChildren());
		
		foreach (Node child in childrenToFree)
		{
			GD.Print($"Removing child: {child.Name}");
			targetViewport.RemoveChild(child);
			child.QueueFree();
		}
		
		targetViewport.CallDeferred(Node.MethodName.AddChild, page);

		GD.Print("REDIRECT COMPLETED");
	}
}
