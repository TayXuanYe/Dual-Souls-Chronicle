using System.Text.Json.Serialization;
public class Config
{
	[JsonPropertyName("youtube_api_key")]
	public string YoutubeApiKey { get; set; }
}
