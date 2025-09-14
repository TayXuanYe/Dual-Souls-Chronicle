using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class SelectScenes : VBoxContainer
{
	[Export] private PackedScene _voteBarScene;
	[Export] private PackedScene _cardScene;
	[Export] private Label _votingTimeLabel;
	[Export] private HBoxContainer _cardContainer;
	[Export] private HBoxContainer _voteBarContainer;
	[Export] private AnimatedSprite2D _animationSprite2D;
	private Timer _voteTimer;

	private List<(Node node, VoteBar script)> _voteBarList = new List<(Node node, VoteBar script)>();
	private List<(Node node, Card script)> _cardList = new List<(Node node, Card script)>();
	private float _width = 960;
	private int _voteTotalCount;
	private string _parentGroupName;
	private int _selectAmount;
	string _type;
	private bool _isInit = false;
	private int _nextProgressIndex;

	public override void _Ready()
	{
		_parentGroupName = NodeUtility.GetParentNodeGroup(this, "IsInViewport1", "IsInViewport2");
		SignalManager.Instance.UpdateVote += OnUpdateVoteSignalReceipt;
		if (_voteTimer != null)
		{
			_voteTimer.Timeout += OnVoteTimerTimeout;
		}
		else
		{
			_voteTimer = new Timer();
			AddChild(_voteTimer);
		}
	}

	public void Init(double voteTime, int selectAmount, string[] voteBarColors, string type, int randomSeed, int nextProgressIndex)
	{
		if (voteBarColors.Length != selectAmount) { return; }

		if (_voteTimer != null)
		{
			_voteTimer.Timeout += OnVoteTimerTimeout;
		}
		else
		{
			_voteTimer = new Timer();
			AddChild(_voteTimer);
		}
		_voteTimer.WaitTime = voteTime;
		_voteTimer.Start();

		_voteTotalCount = 0;
		_selectAmount = selectAmount;
		_type = type;
		_nextProgressIndex = nextProgressIndex;

		InitVoteBar(voteBarColors);
		InitCards(_selectAmount, type, randomSeed);

		_isInit = true;
	}

	private void InitCards(int selectAmount, string type, int randomSeed)
	{
		GD.Print($"Init cards, type:{type}, selectAmount:{selectAmount}, randomSeed:{randomSeed}");
		List<CardModel> cardsData = new List<CardModel>();
		switch (type)
		{
			case "buff":
				cardsData = CardsDataManager.Instance.GetBuffCards(selectAmount, randomSeed);
				break;
			case "character":
				cardsData = CardsDataManager.Instance.GetCharacterCards(selectAmount);
				break;
			default:
				break;
		}

		foreach (CardModel cardModel in cardsData)
		{
			Node card = _cardScene.Instantiate();
			if (card is Card cardScript)
			{
				cardScript.Init(cardModel, type);
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
				voteBarScript.Init(new Color(voteBarColors[i]), (i+1).ToString());
				_voteBarList.Add((voteBarNode, voteBarScript));
				_voteBarContainer.AddChild(voteBarNode);
			}
		}
	}

	public override void _Process(double delta)
	{
		Size = new Vector2(960, 720);
		Position = new Vector2(0, 0);
		if (!_isInit) { return; }
		UpdateVotingTimeLabel(); 
		UpdateVoteBarSize();	
		UpdateSelectCard();
	}
	public void OnVoteTimerTimeout()
	{
		_voteTimer.Stop();

		string carryData = null;
		int maxVoteCount = -1;
		int count = 0;
		foreach ((Node, VoteBar script) voteBar in _voteBarList)
		{
			VoteBar voteScript = voteBar.script;
			if (voteScript.VoteCount > maxVoteCount)
			{
				maxVoteCount = voteScript.VoteCount;
				carryData = _cardList[count].script.CarryData;
			}
			count++;
		}

		EmitSignalByType(_type, carryData);
	}

	private void EmitSignalByType(string type, string carryData)
	{
		GD.Print($"Emit signal,{type}?");
		if(string.IsNullOrEmpty(carryData)) { return; }
		GD.Print($"Emit signal,{type}");
		
		switch (type)
		{
			case "buff":
				SignalManager.Instance.EmitSelectBuffSignal(carryData, _parentGroupName);
				break;
			case "character":
				SignalManager.Instance.EmitSelectCharacterSignal(carryData, _parentGroupName);
				break;
		}

		// if didn't have next Index will be -1
		if (_nextProgressIndex != -1)
		{
			SignalManager.Instance.EmitNextProgressSignal(_parentGroupName, _nextProgressIndex);
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

	public void OnUpdateVoteSignalReceipt(string id, int voteId)
	{
		if(id == _parentGroupName)
		{
			if (voteId < 1 || voteId > _selectAmount) { return; }
			_voteTotalCount++;
			_voteBarList[voteId - 1].script.VoteCount++;
		}
	}

	private void UpdateVoteBarSize()
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

	private void UpdateVotingTimeLabel()
	{
		double timeLeft = _voteTimer.TimeLeft;
		_votingTimeLabel.Text = $"Voting Time: {Math.Floor(timeLeft)}s";

		if (timeLeft <= 5f)
		{
			_votingTimeLabel.Modulate = Math.Floor(timeLeft) % 2 == 0 ? new Color("#FF0000") : new Color("#FFFFFF");
		}
		else
		{
			_votingTimeLabel.Modulate = new Color("#FFFFFF");
		}
	}
}
