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

	[Export] private Node _card1;
	[Export] private Node _card2;
	[Export] private Node _card3;
	[Export] private HBoxContainer _cardContainer;
	private float _width = 960;
	private int _voteTotalCount;
	private int _vote1Count = 0;
	private int _vote2Count = 0;
	private int _vote3Count = 0;
	private double _voteTimeCountdown;
	private bool _isInit = false;
	private Random _random = new Random();
	private Card _card1Script;
	private Card _card2Script;
	private Card _card3Script;

	public override void _Ready()
	{
		this.Visible = true;

		if (_card1 is Card card1Script)
		{
			_card1Script = card1Script;
			var randomNum = _random.Next(CardsDataManager.Instance.BuffCards.Count);
			_card1Script.Init(CardsDataManager.Instance.BuffCards[randomNum]);
			CardsDataManager.Instance.BuffCards.RemoveAt(randomNum);
		}

		if (_card2 is Card card2Script)
		{
			_card2Script = card2Script;
			var randomNum = _random.Next(CardsDataManager.Instance.BuffCards.Count);
			_card2Script.Init(CardsDataManager.Instance.BuffCards[randomNum]);
			CardsDataManager.Instance.BuffCards.RemoveAt(randomNum);
		}

		if (_card3 is Card card3Script)
		{
			_card3Script = card3Script;
			var randomNum = _random.Next(CardsDataManager.Instance.BuffCards.Count);
			_card3Script.Init(CardsDataManager.Instance.BuffCards[randomNum]);
			CardsDataManager.Instance.BuffCards.RemoveAt(randomNum);
		}
		_card3Script.IsSelect = true;
	}

	public override void _Process(double delta)
	{
		if (!_isInit) { return; }
		DisplayVotingTime(delta);
		UpdateVoteBar();
	}

	public void Init(double voteTime)
	{
		if (_isInit) { return; }
		this.Visible = true;
		_voteTimeCountdown = voteTime;
		_vote1Count = 0;
		_vote2Count = 0;
		_vote3Count = 0;
		_voteTotalCount = 0;
		_isInit = true;
	}

	public void UpdateVoteCount(int vote1Count, int vote2Count, int vote3Count)
	{
		_vote1Count += vote1Count;
		_vote2Count += vote2Count;
		_vote3Count += vote3Count;
		_voteTotalCount += vote1Count + vote2Count + vote3Count;
	}

	private void UpdateVoteBar()
	{
		if (_voteTotalCount == 0)
		{
			// set width
			_voteBarPanel1.SetSize(new Vector2(_width / 3, _voteBarPanel1.Size.Y));
			_voteBarPanel2.SetSize(new Vector2(_width / 3, _voteBarPanel2.Size.Y));
			_voteBarPanel3.SetSize(new Vector2(_width / 3, _voteBarPanel3.Size.Y));

			// set position
			_voteBarPanel1.SetPosition(new Vector2(0, _voteBarPanel1.Position.Y));
			_voteBarPanel2.SetPosition(new Vector2(_width / 3, _voteBarPanel2.Position.Y));
			_voteBarPanel3.SetPosition(new Vector2(_width / 3 * 2, _voteBarPanel3.Position.Y));
			return;
		}
		float vote1Width = (float)_vote1Count / (float)_voteTotalCount * _width;
		float vote2Width = (float)_vote2Count / (float)_voteTotalCount * _width;
		float vote3Width = (float)_vote3Count / (float)_voteTotalCount * _width;
		// set width
		_voteBarPanel1.SetSize(new Vector2(vote1Width + 3, _voteBarPanel1.Size.Y));
		_voteBarPanel2.SetSize(new Vector2(vote2Width + 3, _voteBarPanel2.Size.Y));
		_voteBarPanel3.SetSize(new Vector2(vote3Width + 3, _voteBarPanel3.Size.Y));

		// set position
		_voteBarPanel1.SetPosition(new Vector2(0, _voteBarPanel1.Position.Y));
		_voteBarPanel2.SetPosition(new Vector2(vote1Width, _voteBarPanel2.Position.Y));
		_voteBarPanel3.SetPosition(new Vector2(vote1Width + vote2Width, _voteBarPanel3.Position.Y));

		if (vote1Width < 50f)
		{
			_voteCountLabel1.Visible = false;
		}
		else
		{
			_voteCountLabel1.Visible = true;
		}

		if (vote2Width < 50f)
		{
			_voteCountLabel2.Visible = false;
		}
		else
		{
			_voteCountLabel2.Visible = true;
		}

		if (vote3Width < 50f)
		{
			_voteCountLabel3.Visible = false;
		}
		else
		{
			_voteCountLabel3.Visible = true;
		}
	}

	private void DisplayVotingTime(double delta)
	{
		if (_voteTimeCountdown < 0)
		{
			_votingTimeLabel.Text = $"Voting Time: 0s";
			return;
		}
		_votingTimeLabel.Text = $"Voting Time: {_voteTimeCountdown.ToString("F0")}s";

		if (_voteTimeCountdown <= 5f)
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
		_isInit = false;
	}
}
