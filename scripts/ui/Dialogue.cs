using Godot;
using System;

public partial class Dialogue : Control
{
	[Export] private Panel _backgroundPanel;
	[Export] private Label _textLabel;
	
	public void Init(string text)
	{
		_textLabel.Text = text;
		_backgroundPanel.Size = new Vector2(_textLabel.Size.X + 2, _textLabel.Size.Y);
	}
	
	public void Init(string text, Vector2 size)
	{
		_textLabel.Text = text;
		_backgroundPanel.Size = size;
	}
}
