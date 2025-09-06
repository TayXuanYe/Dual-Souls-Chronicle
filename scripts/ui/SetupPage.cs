using Godot;
using System;

public partial class SetupPage : Control
{
	private int _id;
	private string _videoId;
	[Export] private TextEdit _videoIdInput;
	[Export] private Label _urlInputErrorLabel;
	[Export] private Button _submitButton;

	public void Init(int id)
	{
		_id = id;
		_submitButton.Pressed += OnSubmitButtonPressed;
	}

	private void OnSubmitButtonPressed()
	{
		if (string.IsNullOrWhiteSpace(_videoIdInput.Text))
		{
			_urlInputErrorLabel.Text = "Video Id cannot be null or empty.";
			return;
		}
		else
		{
			_urlInputErrorLabel.Text = "";
		}
	}
}
