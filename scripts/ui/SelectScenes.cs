using Godot;
using System;
using System.Collections.Generic;

public partial class SelectScenes : VBoxContainer
{
	[Export] private PackedScene _voteBarScene;
	[Export] private PackedScene _cardScene;

	[Export] private Label _votingTimeLabel;
	[Export] private HBoxContainer _cardContainer;
	private float _width = 960;
	private int _voteTotalCount;
	private List<int> _voteCountList;
	private List<(Node,VoteBar)> _voteBarList;
	private List<(Node,Card)> _cardList;
	private double _voteTimeCountdown;
	private bool _isInit = false;
	private Random _random = new Random();
	private int _id;

	public void Init(int id, double voteTime, string[] voteBarColors, int cardAmount, string type)
	{
		if (_isInit) { return; }
		_id = id;
		_voteTimeCountdown = voteTime;
		_voteTotalCount = 0;

		foreach (string voteBarColor in voteBarColors)
		{
			
		}

		_isInit = true;

	}
	// 	if (_card1 is Card card1Script)
	// 	{
	// 		_card1Script = card1Script;
	// 		var randomNum = _random.Next(CardsDataManager.Instance.BuffCards.Count);
	// 		_card1Script.Init(CardsDataManager.Instance.BuffCards[randomNum]);
	// 		CardsDataManager.Instance.BuffCards.RemoveAt(randomNum);
	// 	}

	// 	if (_card2 is Card card2Script)
	// 	{
	// 		_card2Script = card2Script;
	// 		var randomNum = _random.Next(CardsDataManager.Instance.BuffCards.Count);
	// 		_card2Script.Init(CardsDataManager.Instance.BuffCards[randomNum]);
	// 		CardsDataManager.Instance.BuffCards.RemoveAt(randomNum);
	// 	}

	// 	if (_card3 is Card card3Script)
	// 	{
	// 		_card3Script = card3Script;
	// 		var randomNum = _random.Next(CardsDataManager.Instance.BuffCards.Count);
	// 		_card3Script.Init(CardsDataManager.Instance.BuffCards[randomNum]);
	// 		CardsDataManager.Instance.BuffCards.RemoveAt(randomNum);
	// 	}
	// 	_card3Script.IsSelect = true;
	// }

	// public override void _Process(double delta)
	// {
	// 	if (!_isInit) { return; }
	// 	if (_voteTimeCountdown <= 0)
	// 	{
	// 		OnVoteTimeCountdown();
	// 		return;
	// 	}
	// 	DisplayVotingTime(delta);
	// 	UpdateVoteBar();
	// 	UpdateSelectCard();
	// }
	// bool isOnVoteTimeCountdownTrigger = false;
	// public void OnVoteTimeCountdown()
	// {
	// 	if(isOnVoteTimeCountdownTrigger) { return; }
		
	// 	int id = -1;
	// 	if (_vote1Count > _vote2Count && _vote1Count > _vote3Count)
	// 	{
	// 		id = _card1Script.Id;
	// 	}
	// 	else
	// 	{
	// 		if (_vote2Count > _vote3Count)
	// 		{
	// 			id = _card2Script.Id;
	// 		}
	// 		else
	// 		{
	// 			id = _card3Script.Id;
	// 		}
	// 	}

	// 	//signal id temp use print
	// 	GD.Print(id);
	// }

	// public void UpdateSelectCard()
	// {
	// 	if (_vote1Count > _vote2Count && _vote1Count > _vote3Count)
	// 	{
	// 		_card1Script.IsSelect = true;
	// 		_card2Script.IsSelect = false;
	// 		_card3Script.IsSelect = false;
	// 	}
	// 	else
	// 	{
	// 		if (_vote2Count > _vote3Count)
	// 		{
	// 			_card1Script.IsSelect = false;
	// 			_card2Script.IsSelect = true;
	// 			_card3Script.IsSelect = false;
	// 		}
	// 		else
	// 		{
	// 			_card1Script.IsSelect = false;
	// 			_card2Script.IsSelect = false;
	// 			_card3Script.IsSelect = true;
	// 		}
	// 	}
	// }

	// 	public void UpdateVoteCount(int vote1Count, int vote2Count, int vote3Count)
	// {
	// 	_vote1Count += vote1Count;
	// 	_vote2Count += vote2Count;
	// 	_vote3Count += vote3Count;
	// 	_voteTotalCount += vote1Count + vote2Count + vote3Count;
	// }

	// private void UpdateVoteBar()
	// {
	// 	if (_voteTotalCount == 0)
	// 	{
	// 		// set width
	// 		_voteBarPanel1.SetSize(new Vector2(_width / 3, _voteBarPanel1.Size.Y));
	// 		_voteBarPanel2.SetSize(new Vector2(_width / 3, _voteBarPanel2.Size.Y));
	// 		_voteBarPanel3.SetSize(new Vector2(_width / 3, _voteBarPanel3.Size.Y));

	// 		// set position
	// 		_voteBarPanel1.SetPosition(new Vector2(0, _voteBarPanel1.Position.Y));
	// 		_voteBarPanel2.SetPosition(new Vector2(_width / 3, _voteBarPanel2.Position.Y));
	// 		_voteBarPanel3.SetPosition(new Vector2(_width / 3 * 2, _voteBarPanel3.Position.Y));

	// 		_voteCountLabel1.Text = _vote1Count.ToString();
	// 		_voteCountLabel2.Text = _vote2Count.ToString();
	// 		_voteCountLabel3.Text = _vote3Count.ToString();
	// 		return;
	// 	}
	// 	float vote1Width = (float)_vote1Count / (float)_voteTotalCount * _width;
	// 	float vote2Width = (float)_vote2Count / (float)_voteTotalCount * _width;
	// 	float vote3Width = (float)_vote3Count / (float)_voteTotalCount * _width;
	// 	// set width
	// 	_voteBarPanel1.SetSize(new Vector2(vote1Width + 3, _voteBarPanel1.Size.Y));
	// 	_voteBarPanel2.SetSize(new Vector2(vote2Width + 3, _voteBarPanel2.Size.Y));
	// 	_voteBarPanel3.SetSize(new Vector2(vote3Width + 3, _voteBarPanel3.Size.Y));

	// 	// set position
	// 	_voteBarPanel1.SetPosition(new Vector2(0, _voteBarPanel1.Position.Y));
	// 	_voteBarPanel2.SetPosition(new Vector2(vote1Width, _voteBarPanel2.Position.Y));
	// 	_voteBarPanel3.SetPosition(new Vector2(vote1Width + vote2Width, _voteBarPanel3.Position.Y));

	// 	_voteCountLabel1.Text = _vote1Count.ToString();
	// 	_voteCountLabel2.Text = _vote2Count.ToString();
	// 	_voteCountLabel3.Text = _vote3Count.ToString();
	// 	if (vote1Width < 50f)
	// 	{
	// 		_voteCountLabel1.Visible = false;
	// 	}
	// 	else
	// 	{
	// 		_voteCountLabel1.Visible = true;
	// 	}

	// 	if (vote2Width < 50f)
	// 	{
	// 		_voteCountLabel2.Visible = false;
	// 	}
	// 	else
	// 	{
	// 		_voteCountLabel2.Visible = true;
	// 	}

	// 	if (vote3Width < 50f)
	// 	{
	// 		_voteCountLabel3.Visible = false;
	// 	}
	// 	else
	// 	{
	// 		_voteCountLabel3.Visible = true;
	// 	}
	// }

	// private void DisplayVotingTime(double delta)
	// {
	// 	if (_voteTimeCountdown < 0)
	// 	{
	// 		_votingTimeLabel.Text = $"Voting Time: 0s";
	// 		return;
	// 	}
	// 	_votingTimeLabel.Text = $"Voting Time: {_voteTimeCountdown.ToString("F0")}s";

	// 	if (_voteTimeCountdown <= 5f)
	// 	{
	// 		if (Math.Floor(_voteTimeCountdown) % 2 == 0)
	// 			_votingTimeLabel.Modulate = new Color("#FF0000");
	// 		else
	// 			_votingTimeLabel.Modulate = new Color("#FFFFFF");
	// 	}

	// 	_voteTimeCountdown -= delta;
	// }
}
