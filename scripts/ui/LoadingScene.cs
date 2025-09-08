using Godot;
using System;

public partial class LoadingScene : Control
{
	[Export] private Label _viewport1ReadyLabel;
	[Export] private Label _viewport2ReadyLabel;
	private int _id;
	private bool _isViewport1Ready = false;
	private bool _isViewport2Ready = false;

	public override void _Process(double delta)
	{
		_isViewport1Ready = YoutubeManager.IsYoutubeServices1Ready;
		_isViewport2Ready = YoutubeManager.IsYoutubeServices2Ready;

		if (_isViewport1Ready)
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Ready";
		}
		else
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Not ready";
		}

		if (_isViewport2Ready)
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
		SubViewport parentViewport = GetParent<SubViewport>();
		if (parentViewport != null)
		{
			ViewportData dataNode = parentViewport.GetNode<ViewportData>("Data");
			if (dataNode != null)
			{
				_id = dataNode.Id;
			}
		}
    }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode != Key.P) { return; }

			var mainNode = GetNode<Main>("/root/Loader/Main");
			if (_isViewport1Ready && _isViewport2Ready)
			{
				mainNode.RedirectTo(_id, "GamePage");
			}

		}
	}

}
