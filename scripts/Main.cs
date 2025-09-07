using Godot;
using System.IO;
using System.Text.Json;

public partial class Main : Control
{
	[Export] private SubViewport _subViewport1;
	[Export] private SubViewport _subViewport2;

	[Export] private PackedScene _setupScene;

	public YoutubeServices YoutubeServices1 { get; set; }
	public YoutubeServices YoutubeServices2 { get; set; }
	public string YoutubeApiKey{ get; private set; }

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
		YoutubeApiKey = config.YoutubeApiKey;

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
}
