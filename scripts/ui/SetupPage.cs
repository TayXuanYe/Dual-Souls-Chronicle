using Godot;
using System;

public partial class SetupPage : Control
{
	private int _id;
	private string _videoId;
	[Export] private TextEdit _videoIdInput;
	[Export] private Label _urlInputErrorLabel;
	[Export] private Button _submitButton;

	public override void _Ready()
	{
		_submitButton.Pressed += OnSubmitButtonPressed;
	} 
	
	public void Init(int id)
	{
		_id = id;
		GD.Print($"Setup page init success id:{_id}; Instance ID: {GetInstanceId()}");
	}
	private bool isRequestSend = false;
	private void OnSubmitButtonPressed()
	{
		if (isRequestSend)
		{
			return;
		}
		
		if (string.IsNullOrWhiteSpace(_videoIdInput.Text))
		{
			_urlInputErrorLabel.Text = "Video Id cannot be null or empty.";
			return;
		}
		else
		{
			_urlInputErrorLabel.Text = "";
		}
		string videoId = _videoIdInput.Text.Trim();
		OnLinkToLiveRoom(videoId);
	}
	
	private async void OnLinkToLiveRoom(string videoId)
	{
		var mainNode = GetNode<Main>("/root/Main");
		GD.Print($"Start linking to live room {videoId}, Instance ID: {GetInstanceId()}");
		var youtubeService = new YoutubeServices(mainNode.YoutubeApiKey, videoId);
		bool isSuccessInit = await youtubeService.InitializeAsync();

		if (isSuccessInit)
		{
			if(_id == 1)
				mainNode.YoutubeServices1 = youtubeService;
			else if(_id == 2)
				mainNode.YoutubeServices2 = youtubeService;
			GD.Print($"Live linked in sub viewport {_id}");
			//redirect to another scene
		}
		else
		{
			_urlInputErrorLabel.Text = "Live room not found.";
		}
		
		isRequestSend = false;
	}
}
