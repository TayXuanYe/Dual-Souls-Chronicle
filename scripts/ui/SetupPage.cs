using Godot;
using System;

public partial class SetupPage : Control
{
	private string _parentGroupName;
	private string _videoId;
	[Export] private TextEdit _videoIdInput;
	[Export] private Label _urlInputErrorLabel;
	[Export] private Button _submitButton;
	[Export] private TextEdit _nameInput;
	[Export] private Label _nameErrorLabel;

	public override void _Ready()
	{
		_submitButton.Pressed += OnSubmitButtonPressed;
		_parentGroupName = NodeUtility.GetParentNodeGroup(this, "IsInViewport1", "IsInViewport2");
		if (string.IsNullOrEmpty(_parentGroupName))
		{
			GD.Print("Parent group name is null or empty");
			return;
		}
	}

	private bool _isRequestSend = false;
	private void OnSubmitButtonPressed()
	{
		if (_isRequestSend)
		{
			return;
		}

		if(string.IsNullOrWhiteSpace(_nameInput.Text))
		{
			_nameErrorLabel.Text = "Name cannot be null or empty.";
			return;
		}
		else
		{
			_nameErrorLabel.Text = "";
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
		_isRequestSend = true;
		var mainNode = GetNode<Main>("/root/Loader/Main");
		bool isSuccessInit = false;
		isSuccessInit = await YoutubeManager.Instance.RegisterYoutubeManager(videoId, _parentGroupName);

		if (isSuccessInit)
		{
			CharacterModel characterModel = new CharacterModel();
			characterModel.CharacterName = _nameInput.Text.Trim();
			characterModel.Id = $"character_{_parentGroupName}";
			CharacterDataManager.Instance.Characters.Add(_parentGroupName, characterModel);
			//redirect to another scene
			mainNode.RedirectTo(_parentGroupName, "LoadingPage");
		}
		else
		{
			GD.Print($"Live room not found, id:{videoId}");
		}

		_isRequestSend = false;
	}
}
