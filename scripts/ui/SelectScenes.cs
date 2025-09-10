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
	private int _id;
	private int _cardAmount = 0;
	string _type;

	public void Init(int id, double voteTime, string[] voteBarColors, int cardAmount, string type, int randomSeed)
	{
		if (_isInit) { return; }
		_id = id;
		GD.Print("select scene init id" + id);
		_voteTimeCountdown = voteTime;
		_voteTotalCount = 0;
		_cardAmount = cardAmount;
		_type = type;
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
		List<CardDto> cardsData = new List<CardDto>();
		switch (type)
		{
			case "buff":
				cardsData = CardsDataManager.Instance.GetBuffCards(cardAmount, randomSeed);
				break;
			case "character":
				cardsData = CardsDataManager.Instance.GetCharacterCards();
				break;
			default:
				break;
		}
		foreach (CardDto cardDto in cardsData)
		{
			Node card = _cardScene.Instantiate();
			if (card is Card cardScript)
			{
				cardScript.Init(cardDto, type);
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
		if (isOnVoteTimeCountdownTrigger) { return; }

		string id = "1";
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
		isOnVoteTimeCountdownTrigger = true;
		switch (_type)
		{
			case "buff":
				break;
			case "character":
				SignalManager.Instance.EmitSelectCharacterSignal(id, _id);
				break;
		}
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
