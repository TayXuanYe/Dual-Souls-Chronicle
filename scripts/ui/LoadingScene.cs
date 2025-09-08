using Godot;
using System;

public partial class LoadingScene : Control
{
	[Export] private Label _viewport1ReadyLabel;
	[Export] private Label _viewport2ReadyLabel;
	private int _id;

	public override void _Process(double delta)
	{
		if (YoutubeManager.IsYoutubeServices1Ready)
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Ready";
		}
		else
		{
			_viewport1ReadyLabel.Text = "Viewport 1: Not ready";
		}

		if (YoutubeManager.IsYoutubeServices2Ready)
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
		Node current = this;
		SubViewport parentViewport = null;
		int count = 0;
		while (current != null)
		{
			if (current is SubViewport viewport)
			{
				parentViewport = viewport;
				break;
			}
			current = current.GetParent();
			GD.Print($"Finding {current.Name}, count {count++}");
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

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode != Key.P) { return; }
			GD.Print("KEY P PRESS");
			var mainNode = GetNode<Main>("/root/Loader/Main");
			if (YoutubeManager.IsYoutubeServices1Ready && YoutubeManager.IsYoutubeServices2Ready)
			{
				mainNode.RedirectTo(_id, "GamePage");
			}

		}
	}

}
