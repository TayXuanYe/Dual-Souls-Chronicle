using Godot;
using System;

public partial class SetupPage : Control
{
	private int _id;
	[Export] private TextEdit _videoIdInput;
	[Export] private Label _urlInputErrorLabel;
	[Export] private Button _submitButton;

	private string _videoId;
	public void Init(int id)
	{
		_id = id;
		_submitButton.Pressed += OnSubmitButtonPressed;
	}
	
	private void OnSubmitButtonPressed()
	{
		
	}
}
