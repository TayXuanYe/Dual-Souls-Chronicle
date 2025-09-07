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
        GD.Print("Ready");
        if (_isViewport1Ready)
        {
            _viewport1ReadyLabel.Text = "Viewport 1: Ready";
        }
        else
        {
            _viewport1ReadyLabel.Text = "Viewport 1: Not ready";
            _isViewport1Ready = YoutubeManager.Instance.RegisterYoutubeManager1 != null;
        }

        if (_isViewport2Ready)
        {
            _viewport2ReadyLabel.Text = "Viewport 2: Ready";
        }
        else
        {
            _viewport2ReadyLabel.Text = "Viewport 2: Not ready";
            _isViewport2Ready = YoutubeManager.Instance.RegisterYoutubeManager2 != null;
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
			if (_isViewport1Ready && _isViewport2Ready)
			{
				mainNode.RedirectTo(_id, "GamePage");
			}

		}
	}

}
