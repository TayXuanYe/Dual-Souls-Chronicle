using Godot;
using System;
using System.Collections.Generic;

public partial class SelectScenes : VBoxContainer
{
	[Export] private PackedScene _voteBarScene;
	[Export] private PackedScene _cardScene;

	[Export] private Label _votingTimeLabel;
	[Export] private HBoxContainer _cardContainer;
	[Export] private HBoxContainer _voteBarContainer;
	private float _width = 960;
	private int _voteTotalCount;
	private List<(Node node, VoteBar script)> _voteBarList = new List<(Node node, VoteBar script)>();
	private List<(Node node, Card script)> _cardList = new List<(Node node, Card script)>();
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
			Node voteBar = _voteBarScene.Instantiate();
			if (voteBar is VoteBar voteBarScript)
			{
				voteBarScript.Init(new Color(voteBarColor));
				_voteBarList.Add((voteBar, voteBarScript));
				_voteBarContainer.AddChild(voteBar);
			}
		}

		for (int i = 0; i < cardAmount; i++)
		{
			Node card = _cardScene.Instantiate();
			if (card is Card cardScript)
			{
				var randomNum = _random.Next(CardsDataManager.Instance.BuffCards.Count);
				cardScript.Init(CardsDataManager.Instance.BuffCards[randomNum]);
				CardsDataManager.Instance.BuffCards.RemoveAt(randomNum);

				_cardList.Add((card, cardScript));
				_cardContainer.AddChild(card);
			}
		}

		_isInit = true;
	}
	
	public override void _Process(double delta)
	{
		if (!_isInit) { return; }
		if (_voteTimeCountdown <= 0)
		{
			OnVoteTimeCountdown();
			return;
		}
		DisplayVotingTime(delta);
		UpdateVoteBar();
		UpdateSelectCard();
	}

	bool isOnVoteTimeCountdownTrigger = false;
	public void OnVoteTimeCountdown()
	{
		if(isOnVoteTimeCountdownTrigger) { return; }

		int id = -1;
		int maxVoteCount = -1;
		int count = 0;
		foreach ((Node, VoteBar script) voteBar in _voteBarList)
		{
			VoteBar voteScript = voteBar.script;
			if (voteScript.VoteCount > maxVoteCount)
			{
				maxVoteCount = voteScript.VoteCount;
				id = _cardList[count].script.Id;
			}
			count++;
		}

		//signal id temp use print
		GD.Print(id);
	}

	public void UpdateSelectCard()
	{
		int maxCountIndex = -1;
		int maxVoteCount = -1;
		int count = 0;
		foreach ((Node, VoteBar script) voteBar in _voteBarList)
		{
			VoteBar voteScript = voteBar.script;
			if (voteScript.VoteCount > maxVoteCount)
			{
				maxVoteCount = voteScript.VoteCount;
				maxCountIndex = count;
			}
			_cardList[count].script.IsSelect = false;
			count++;
		}

		_cardList[maxCountIndex].script.IsSelect = true;
	}

	public void UpdateVoteCount(int[] voteCounts)
	{
		if(!_isInit) { return; }
		int count = 0;
		// GD.Print(voteCounts.Length + "::" + _voteBarList.Count);
		foreach (int voteCount in voteCounts)
		{
			_voteBarList[count].script.VoteCount += voteCount;
			_voteTotalCount += voteCount;
			count++;
		}
	}

	private void UpdateVoteBar()
	{
		float continuousPosition = 0;
		if (_voteTotalCount == 0)
		{
			// set width
			foreach ((Node node, VoteBar script) voteData in _voteBarList)
			{
				Panel voteBar = voteData.script.VoteBarPanel;
				float barWidth = _width / (float)_voteBarList.Count;
				voteBar.Size = new Vector2(barWidth, voteBar.Size.Y);
				voteBar.SetPosition(new Vector2(continuousPosition, voteBar.Position.Y));
				continuousPosition += barWidth;
			}
			return;
		}

		foreach ((Node node, VoteBar script) voteData in _voteBarList)
		{
			Panel voteBar = voteData.script.VoteBarPanel;
			float barWidth = (float)voteData.script.VoteCount / (float)_voteTotalCount * _width;
			voteBar.Size = new Vector2(barWidth + 3, voteBar.Size.Y);
			voteBar.SetPosition(new Vector2(continuousPosition, voteBar.Position.Y));
			continuousPosition += barWidth;
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
}
