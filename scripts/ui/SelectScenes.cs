using Godot;
using System;

public partial class SelectScenes : VBoxContainer
{
	[Export] private Label _votingTimeLabel;
	[Export] private Label _voteCountLabel1;
	[Export] private Label _voteCountLabel2;
	[Export] private Label _voteCountLabel3;
	[Export] private Panel _voteBarPanel1;
	[Export] private Panel _voteBarPanel2;
	[Export] private Panel _voteBarPanel3;

	private int _totalVoteCount;
	private int _vote1Count;
	private int _vote2Count;
	private int _vote3Count;
	private double _voteTimeCountdown;
	private double _updateTime;
	bool isInit = false;
	public override void _Ready()
	{
		this.Visible = false;
	}

	public override void _Process(double delta)
	{
		if (!isInit) { return; }
		DisplayVotingTime(delta);

	}

	public void Init(double voteTime)
	{
		this.Visible = true;
		_voteTimeCountdown = voteTime;
		_totalVoteCount = 0;
		_vote1Count = 0;
		_vote2Count = 0;
		_vote3Count = 0;
		_updateTime = 0;
		isInit = true;
	}

	public void UpdateVoteCount(int vote1Count, int vote2Count, int vote3Count, double updateTime)
	{

	}

	private void DisplayVotingTime(double delta)
	{
		if (_voteTimeCountdown < 0)
		{
			_votingTimeLabel.Text = $"Voting Time: 0s";
			return;
		}
		_votingTimeLabel.Text = $"Voting Time: {_voteTimeCountdown}s";

		if (_voteTimeCountdown <= 10f)
		{
			if (Math.Floor(_voteTimeCountdown) % 2 == 0)
				_votingTimeLabel.Modulate = new Color("#FF0000");
			else
				_votingTimeLabel.Modulate = new Color("#FFFFFF");
		}

		_voteTimeCountdown -= delta;
	}
	
	public void HiddenScene()
	{
		this.Visible = false;
		isInit = false;
	}
}
