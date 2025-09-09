using Godot;
using System;

public partial class Dialogue : Control
{
	[Export] private Panel _backgroundPanel;
	[Export] private Label _textLabel;

	public void Init(string text)
	{
		_textLabel.Text = text;
	}

	public void Init(string text, Vector2 size)
	{
		_textLabel.Text = text;
		_backgroundPanel.Size = size;
	}	
}
