using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;
using System.Threading.Tasks;
using System;

public class YoutubeServices
{
	private readonly YouTubeService _youtubeService;
	private readonly string _videoId;
	private string _liveChatId;
	private string _nextPageToken = null;

	public YoutubeServices(string apiKey, string videoId)
	{
		_videoId = videoId;
		_youtubeService = new YouTubeService(new BaseClientService.Initializer()
		{
			ApiKey = apiKey,
			ApplicationName = this.GetType().ToString()
		});
	}

	public async Task<bool> InitializeAsync()
	{
		try
		{
			var videoRequest = _youtubeService.Videos.List("liveStreamingDetails");
			videoRequest.Id = _videoId;
			var videoResponse = await videoRequest.ExecuteAsync();

			if (videoResponse.Items.Count > 0 && videoResponse.Items[0].LiveStreamingDetails != null)
			{
				_liveChatId = videoResponse.Items[0].LiveStreamingDetails.ActiveLiveChatId;
				if (!string.IsNullOrEmpty(_liveChatId))
				{
					Console.WriteLine($"Successfully connected to live stream {_videoId}, Live chat ID is {_liveChatId}.");
					return true;
				}
			}
			Console.WriteLine($"Error: Unable to connect to live stream {_videoId}. Please check if the video ID is correct or if the live stream is currently active.");
			return false;
		}
		catch (Google.GoogleApiException ex)
		{
			Console.WriteLine($"API Error: {ex.Message}");
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An unexpected error occurred: {ex.Message}");
			return false;
		}
	}

	public async Task<(LiveChatMessageListResponse response, string nextPageToken)> GetChatMessagesAsync()
	{
		if (string.IsNullOrEmpty(_liveChatId))
		{
			throw new InvalidOperationException("Init not completed, please inti by using InitializeAsync().");
		}

		var chatRequest = _youtubeService.LiveChatMessages.List(_liveChatId, "snippet");
		chatRequest.PageToken = _nextPageToken;

		var chatResponse = await chatRequest.ExecuteAsync();
		_nextPageToken = chatResponse.NextPageToken;
		return (chatResponse, chatResponse.NextPageToken);
	}
	public async Task<(LiveChatMessageListResponse response, string nextPageToken)> GetChatMessagesAsync(string pageToken)
	{
		if (string.IsNullOrEmpty(_liveChatId))
		{
			throw new InvalidOperationException("Init not completed, please inti by using InitializeAsync().");
		}

		var chatRequest = _youtubeService.LiveChatMessages.List(_liveChatId, "snippet");
		chatRequest.PageToken = pageToken;
		
		var chatResponse = await chatRequest.ExecuteAsync();
		_nextPageToken = chatResponse.NextPageToken;
		return (chatResponse, chatResponse.NextPageToken);
	}
}
