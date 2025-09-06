using Godot;
using System;

public partial class Main : Control
{
	[Export] private SubViewport _subViewport1;
	[Export] private SubViewport _subViewport2;
	public string youtube_api_key;

	public override void _Ready()
	{
		// get api key
		string configPath = "res://config.json";
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
        youtube_api_key = config.YoutubeApiKey;
	}
}
