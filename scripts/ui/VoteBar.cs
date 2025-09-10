using Godot;
using System;

public partial class VoteBar : Panel
{
	[Export] private Color _color;
	[Export] public Label VoteNumberLabel;
	[Export] public Label VoteCountLabel;
	[Export] public Panel VoteBarPanel;
	public int VoteCount { get; set; } = 0;
	private bool _isInit;
	public void Init(Color color, string voteNumberText)
	{
		_color = color;
		VoteBarPanel.Modulate = _color;
		VoteNumberLabel.SelfModulate = new Color("#FFFFFF");
		VoteNumberLabel.Text = voteNumberText;
		VoteCountLabel.Text = "0";    
	}

	public override void _Process(double delta)
	{
		VoteCountLabel.Text = VoteCount.ToString();    
	}
}
