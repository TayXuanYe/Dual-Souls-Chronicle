// YoutubeManager.cs
using System.Threading.Tasks;
using System;
using Godot;

public partial class YoutubeManager : Node
{
	private static YoutubeManager _instance;
	public static YoutubeManager Instance => _instance;

	public static YoutubeServices YoutubeServices1 { get; private set; }
	public static YoutubeServices YoutubeServices2 { get; private set; }

	public static bool IsYoutubeServices1Ready { get; private set; } = false;
	public static bool IsYoutubeServices2Ready { get; private set; } = false;

	public string YoutubeApiKey { get; set; }
	public override void _Ready()
	{
		_instance = this;
	}

	public override void _Process(double delta)
	{
		if (IsYoutubeServices1Ready && IsYoutubeServices2Ready) { return; }
		IsYoutubeServices1Ready = YoutubeServices1 != null;
		IsYoutubeServices2Ready = YoutubeServices2 != null;
	}


	public async Task<bool> RegisterYoutubeManager1(string videoId)
	{
		var youtubeService = new YoutubeServices(YoutubeApiKey, videoId);
		bool isSuccessInit = await youtubeService.InitializeAsync();
		if (isSuccessInit)
		{
			YoutubeServices1 = youtubeService;
			return true;
		}
		return false;
	}

	public async Task<bool> RegisterYoutubeManager2(string videoId)
	{
		var youtubeService = new YoutubeServices(YoutubeApiKey, videoId);
		bool isSuccessInit = await youtubeService.InitializeAsync();
		if (isSuccessInit)
		{
			YoutubeServices2 = youtubeService;
			return true;
		}
		return false;
	}
}
