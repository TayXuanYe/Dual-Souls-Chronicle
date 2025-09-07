// YoutubeManager.cs
using System.Threading.Tasks;
using Godot;

public partial class YoutubeManager : Node
{
	private static YoutubeManager _instance;
	public static YoutubeManager Instance => _instance;

	public static YoutubeServices _youtubeServices1;
	public static YoutubeServices _youtubeServices2;

	public string YoutubeApiKey { get; set; }

	public override void _Ready()
	{
		_instance = this;
	}

	public async Task<bool> RegisterYoutubeManager1(string videoId)
	{
		var youtubeService = new YoutubeServices(YoutubeApiKey, videoId);
		bool isSuccessInit = await youtubeService.InitializeAsync();
		if (isSuccessInit)
		{
			_youtubeServices1 = youtubeService;
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
			_youtubeServices2 = youtubeService;
			return true;
		}
		return false;
	}
	
}
