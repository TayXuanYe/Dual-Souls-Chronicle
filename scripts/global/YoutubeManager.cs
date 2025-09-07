// YoutubeManager.cs
using System.Threading.Tasks;
using Godot;

public partial class YoutubeManager : Node
{
	private static YoutubeManager _instance;
	public static YoutubeManager Instance => _instance;

	public static YoutubeServices _youtubeServices1;
	public static YoutubeServices _youtubeServices2;

	public static bool IsYoutubeServices1Ready { get; private set; } = false;
	public static bool IsYoutubeServices2Ready { get; private set; } = false;

	public string YoutubeApiKey { get; set; }

	public override void _Ready()
	{
		_instance = this;
	}

	public override void _Process(double delta)
	{
		if(IsYoutubeServices1Ready && IsYoutubeServices2Ready) {return;}
		IsYoutubeServices1Ready = _youtubeServices1 != null;
		IsYoutubeServices2Ready = _youtubeServices2 != null;
		// GD.Print($"v1:{IsYoutubeServices2Ready},v2:{IsYoutubeServices2Ready}");
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
