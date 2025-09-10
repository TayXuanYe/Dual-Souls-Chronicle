using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class SelectScenes : VBoxContainer
{
	[Export] private PackedScene _voteBarScene;
	[Export] private PackedScene _cardScene;
	[Export] private Label _votingTimeLabel;
	[Export] private HBoxContainer _cardContainer;
	[Export] private HBoxContainer _voteBarContainer;
	
	private List<(Node node, VoteBar script)> _voteBarList = new List<(Node node, VoteBar script)>();
	private List<(Node node, Card script)> _cardList = new List<(Node node, Card script)>();
	private float _width = 960;
	private int _voteTotalCount;
	private double _voteTimeCountdown;
	private string _parrentGroupName;
	private int _selectAmount;
	string _type;
	private bool _isInit = false;

	public void Init(double voteTime,int selectAmount, string[] voteBarColors, string type, int randomSeed)
	{
		if (_isInit) { return; }
		if(voteBarColors.Length != selectAmount) { return; }

		_voteTimeCountdown = voteTime;
		_voteTotalCount = 0;
		_selectAmount = selectAmount;
		_type = type;

		InitVoteBar(voteBarColors);
		InitCards(_selectAmount, type, randomSeed);

		_isInit = true;
	}
	private void InitCards(int selectAmount, string type, int randomSeed)
	{
		List<CardModel> cardsData = new List<CardModel>();
		switch (type)
		{
			case "buff":
				cardsData = CardsDataManager.Instance.GetBuffCards(selectAmount, randomSeed);
				break;
			case "character":
				cardsData = CardsDataManager.Instance.GetCharacterCards(selectAmount, randomSeed);
				break;
			default:
				break;
		}
		
		foreach (CardModel cardDto in cardsData)
		{
			Node card = _cardScene.Instantiate();
			if (card is Card cardScript)
			{
				cardScript.Init(cardDto, type);
				_cardList.Add((card, cardScript));

				_cardContainer.AddChild(card);
			}
		}
	}
	private void InitVoteBar(string[] voteBarColors)
	{
		for (int i = 0; i < voteBarColors.Length; i++)
		{
			Node voteBarNode = _voteBarScene.Instantiate();
			if (voteBarNode is VoteBar voteBarScript)
			{
				voteBarScript.Init(new Color(voteBarColors[i]), i.ToString());
				_voteBarList.Add((voteBarNode, voteBarScript));
				_voteBarContainer.AddChild(voteBarNode);
			}
		}
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
		EmitSignalByType(_type, id);
	}

	private void EmitSignalByType(string type, string id)
	{
		switch (type)
		{
			case "buff":
				SignalManager.Instance.EmitSelectBuffSignal(id, _parrentGroupName);
				break;
			case "character":
				SignalManager.Instance.EmitSelectCharacterSignal(id, _parrentGroupName);
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
		if(voteCounts.Length != _selectAmount) { return; }

		for (int i = 0; i < voteCounts.Length; i++)
		{
			_voteBarList[i].script.VoteCount = voteCounts[i];
			_voteTotalCount += voteCounts[i];
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
