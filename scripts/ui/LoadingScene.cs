using Godot;
using System;

public partial class LoadingScene : Control
{
	[Export] private Label _viewport1ReadyLabel;
	[Export] private Label _viewport2ReadyLabel;
	private string _parentGroupName;

	public override void _Process(double delta)
	{
		if (YoutubeManager.Instance.IsYoutubeManagerRegistered("IsInViewport1"))
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Ready";
		}
		else
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Not ready";
		}

		if (YoutubeManager.Instance.IsYoutubeManagerRegistered("IsInViewport2"))
		{
			_viewport2ReadyLabel.Text = "Viewport 2: Ready";
		}
		else
		{
			_viewport2ReadyLabel.Text = "Viewport 2: Not ready";
		}
		
	}

	public override void _Ready()
	{
		_parentGroupName = NodeUtility.GetParentNodeGroup(this, "IsInViewport1", "IsInViewport2");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode != Key.P) { return; }
			GD.Print("KEY P PRESS");
			var mainNode = GetNode<Main>("/root/Loader/Main");
			if (YoutubeManager.Instance.IsYoutubeManagerRegistered("IsInViewport1") && YoutubeManager.Instance.IsYoutubeManagerRegistered("IsInViewport2"))
			{
				mainNode.RedirectTo(_parentGroupName, "GamePage");
			}
		}
	}

}
