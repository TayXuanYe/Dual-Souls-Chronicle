using Godot;
using System;

public partial class VoteBar : Panel
{
	[Export] private Color _color;
	[Export] public Label VoteNumberLabel;
	[Export] public Panel VoteBarPanel;
	private bool _isInit;
	public void Init(Color color)
	{
		_color = color;
		VoteBarPanel.Modulate = _color;
	}

	public override void _Process(double delta)
	{
		if (VoteBarPanel.Size.X < VoteNumberLabel.Size.X)
		{
			VoteNumberLabel.Visible = false;
		}
		else
		{
			VoteNumberLabel.Visible = true;
		}        
	}
}
