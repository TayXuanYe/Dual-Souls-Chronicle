// YoutubeManager.cs
using System.Threading.Tasks;
using System;
using Godot;
using System.Collections.Generic;

public partial class YoutubeManager : Node
{
	private static YoutubeManager _instance;
	public static YoutubeManager Instance => _instance;

	public static Dictionary<string, YoutubeServices> YoutubeServicesMap { get; private set; } = new Dictionary<string, YoutubeServices>();

	public string YoutubeApiKey { get; set; }
	public override void _Ready()
	{
		_instance = this;
	}

	public async Task<bool> RegisterYoutubeManager(string videoId)
	{
		if (YoutubeServicesMap.ContainsKey(videoId))
		{
			return true;
		}
		var youtubeService = new YoutubeServices(YoutubeApiKey, videoId);
		bool isSuccessInit = await youtubeService.InitializeAsync();
		if (isSuccessInit)
		{
			YoutubeServicesMap[videoId] = youtubeService;
			return true;
		}
		return false;
	}

	public bool IsYoutubeManagerRegistered(string videoId)
	{
		return YoutubeServicesMap.ContainsKey(videoId);
	}
}
