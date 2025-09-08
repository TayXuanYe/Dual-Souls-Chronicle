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
		Node current = this;
		SubViewport parentViewport = null;
		while (current != null)
		{
			if (current is SubViewport viewport)
			{
				parentViewport = viewport;
				break;
			}
			current = current.GetParent(); 
		}

		if (parentViewport != null)
		{
			ViewportData dataNode = parentViewport.GetNode<ViewportData>("Data");
			if (dataNode != null)
			{
				_id = dataNode.Id;
				GD.Print($"Init id:{_id}");
			}
			else
			{
				GD.Print($"Init id: fail");
			}
		}
		else
		{
			GD.Print($"Owner not found");
		}
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
		GD.Print($"Start linking to live room {videoId}, Instance ID: {GetInstanceId()},_id{_id}");
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
		}
		else
		{
			GD.Print($"Live room not found, id:{videoId}");
		}

		isRequestSend = false;
	}
}
