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
		var mainNode = GetNode<Main>("/root/Loader/Main");
		GD.Print($"Start linking to live room {videoId}, Instance ID: {GetInstanceId()}");
		bool isSuccessInit = false;
		if (_id == 1)
		{
			isSuccessInit = await YoutubeManager.Instance.RegisterYoutubeManager1(videoId);
		}
		else if (_id == 2)
		{
			isSuccessInit = await YoutubeManager.Instance.RegisterYoutubeManager2(videoId);
		}

		if (isSuccessInit)
		{
			GD.Print($"Live linked in sub viewport {_id}");
			//redirect to another scene
			mainNode.RedirectTo(_id, "LoadingPage");
			QueueFree();
		}
		else
		{
			GD.Print($"Live room not found, id:{videoId}");
		}

		isRequestSend = false;
	}
	
	public override void _ExitTree()
	{
		base._ExitTree();
		GD.Print($"SetupPage instance with ID:{GetInstanceId()} is exiting the tree.");
	}
}
