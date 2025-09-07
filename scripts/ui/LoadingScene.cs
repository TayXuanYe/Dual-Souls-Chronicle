using Godot;
using System;

public partial class LoadingScene : Control
{
	[Export] private Label _viewport1ReadyLabel;
	[Export] private Label _viewport2ReadyLabel;
	private int _id;

	public override void _Process(double delta)
	{
		GD.Print("Ready");
		var mainNode = GetNode<Main>("/root/Main");
		if (mainNode.IsViewport1Ready)
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Ready";
		}
		else
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Not ready";
		}

		if (mainNode.IsViewport2Ready)
		{
			_viewport2ReadyLabel.Text = "Viewport 2: Ready";
		}
		else
		{
			_viewport2ReadyLabel.Text = "Viewport 2: Not ready";
		}
	}

	public void Init(int id)
	{
		_id = id;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode != Key.P) { return; }

			var mainNode = GetNode<Main>("/root/Main");
			if (mainNode.IsViewport1Ready && mainNode.IsViewport2Ready)
			{
				mainNode.RedirectTo(_id, "GamePage");
			}

		}
	}

}
