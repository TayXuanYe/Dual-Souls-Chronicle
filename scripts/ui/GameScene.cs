using Godot;
using System;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public partial class GameScene : Control
{
	[Export] private PackedScene _selectScene;
	[Export] private PackedScene _dialogue;
	[Export] private VBoxContainer _player1DialogDisplayVBox;
	[Export] private VBoxContainer _player2DialogDisplayVBox;
	[Export] private Panel _player1DataPanel;
	[Export] private Panel _player2DataPanel;

	private string _parentGroupName;
	private bool _isInit = false;
	private long _pollingIntervalMillis = 0;
	private (Node node, SelectScenes script) _selectSceneInstant;
	private (Node node, SelectScenes script) _battleSceneInstant;
	private Queue<(Node node, Dialogue script)> _dialogueInstants1 = new Queue<(Node node, Dialogue script)>();
	private Queue<(Node node, Dialogue script)> _dialogueInstants2 = new Queue<(Node node, Dialogue script)>();
	private int _selectionAmount = 3;

	private async Task StartGetChartMessageAsync()
	{
		while (true)
		{
			try
			{
				await Task.Delay(TimeSpan.FromMilliseconds(_pollingIntervalMillis));
				YoutubeServices service = YoutubeManager.YoutubeServicesMap[_parentGroupName];

				var (response, newNextPageToken) = await service.GetChatMessagesAsync();

				if (response?.Items != null)
				{
					_pollingIntervalMillis = response.PollingIntervalMillis ?? 5000;
					ProcessChartResponse(response);
				}
			}
			catch (Exception ex)
			{
				GD.PrintErr($"Error: {ex.Message}");
				break;
			}
		}
	}

	private void ProcessChartResponse(LiveChatMessageListResponse response)
	{
		Dictionary<int, int> votingData = new Dictionary<int, int>();
		foreach (var message in response.Items)
		{
			var messageText = message.Snippet?.DisplayMessage;
			if (messageText.All(c => char.IsDigit(c) || char.IsWhiteSpace(c)))
			{
				var result = JustifyAndConvertVoteMessageValid(messageText);
				if (result.isValid)
				{
					votingData.Add(result.data, 1);
				}
			}
			else
			{
				SignalManager.Instance.EmitChatSignal(messageText, _parentGroupName);
			}
		}

		// is select page found if found update vote
		if (_selectSceneInstant.script != null)
		{
			var votesData = votingData.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToArray();
			_selectSceneInstant.script.UpdateVoteCount(votesData);
		}
	}

	private (bool isValid, int data) JustifyAndConvertVoteMessageValid(string message)
	{
		message.Replace(" ", "");
		char firstChar = message[0];
		if (firstChar - '0' != 0 || firstChar - '0' < _selectionAmount)
		{
			bool foundNotSame = false;
			for (int i = 1; i < message.Count(); i++)
			{
				if (message[i] != firstChar)
				{
					foundNotSame = true;
					break;
				}
			}
			if (!foundNotSame)
			{
				return (true, firstChar - '0');
			}
		}
		return (false, -1);
	}

	private int maxDialogAmount = 5;
	public void OnDisplayDialog(string message, string parentGroupName)
	{
		if (parentGroupName == "IsInViewport1")
		{
			Node dialog = _dialogue.Instantiate();
			if (dialog is Dialogue script)
			{
				script.Init(message);
				_dialogueInstants1.Enqueue((dialog, script));

				_player1DialogDisplayVBox.AddChild(dialog);
			}

			if (_dialogueInstants1.Count > maxDialogAmount)
			{
				Node node = _dialogueInstants1.Dequeue().node;
				node.QueueFree();
			}
		}
		if (parentGroupName == "IsInViewport2")
		{
			Node dialog = _dialogue.Instantiate();
			if (dialog is Dialogue script)
			{
				script.Init(message);
				_dialogueInstants2.Enqueue((dialog, script));

				_player2DialogDisplayVBox.AddChild(dialog);
			}

			if (_dialogueInstants2.Count > maxDialogAmount)
			{
				Node node = _dialogueInstants2.Dequeue().node;
				node.QueueFree();
			}
		}
	}
	public override void _Ready()
	{
		Size = new Vector2(960, 720);

		SignalManager.Instance.DisplayDialog += OnDisplayDialog;
		SignalManager.Instance.UpdateAllPlayerData += OnUpdateAllPlayerDataSignalReceipt;
		_parentGroupName = NodeUtility.GetParentNodeGroup(this, "IsInViewport1", "IsInViewport2");

		Node selectScene = _selectScene.Instantiate();
		if (selectScene is SelectScenes selectSceneScript)
		{
			string[] colors = ["#66CCFF", "#FFEED0", "#eeff00ff"];
			selectSceneScript.Init(10, 3, colors, "buff", 1);
			selectSceneScript.SetPosition(new Vector2(0, selectSceneScript.Position.Y));
		}
		AddChild(selectScene);

		SignalManager.Instance.EmitUpdateAllPlayerDataSignal();		

		_ = StartGetChartMessageAsync();

		_isInit = true;
	}

	private void OnUpdateAllPlayerDataSignalReceipt()
	{
		if (_player1DataPanel is PlayerDataPanel player1DataPanelScript)
		{
			player1DataPanelScript.LiveRoomNameLabel.Text = CharacterDataManager.Instance.Characters["IsInViewport1"].CharacterName;
			if (CharacterDataManager.Instance.Characters["IsInViewport1"].IsAssignRole())
			{
				string hpAmount = CharacterDataManager.Instance.Characters["IsInViewport1"].Hp.ToString();
				string hpLimit = CharacterDataManager.Instance.Characters["IsInViewport1"].HpLimit.ToString();
				player1DataPanelScript.HPLabel.Text = $"{hpAmount}/{hpLimit}";
				player1DataPanelScript.AttackLabel.Text = CharacterDataManager.Instance.Characters["IsInViewport1"].Attack.ToString();
				player1DataPanelScript.DefenseLabel.Text = CharacterDataManager.Instance.Characters["IsInViewport1"].Defense.ToString();

				var buffs = CharacterDataManager.Instance.Characters["IsInViewport1"].Buff;
				player1DataPanelScript.BuffLabel.Text = string.Join(", ", buffs.Select(b => b.Name));
			}
		}

		if (_player2DataPanel is PlayerDataPanel player2DataPanelScript)
		{
			player2DataPanelScript.LiveRoomNameLabel.Text = CharacterDataManager.Instance.Characters["IsInViewport2"].CharacterName;
			if (CharacterDataManager.Instance.Characters["IsInViewport2"].IsAssignRole())
			{
				string hpAmount = CharacterDataManager.Instance.Characters["IsInViewport2"].Hp.ToString();
				string hpLimit = CharacterDataManager.Instance.Characters["IsInViewport2"].HpLimit.ToString();
				player2DataPanelScript.HPLabel.Text = $"{hpAmount}/{hpLimit}";
				player2DataPanelScript.AttackLabel.Text = CharacterDataManager.Instance.Characters["IsInViewport2"].Attack.ToString();
				player2DataPanelScript.DefenseLabel.Text = CharacterDataManager.Instance.Characters["IsInViewport2"].Defense.ToString();

				var buffs = CharacterDataManager.Instance.Characters["IsInViewport2"].Buff;
				player2DataPanelScript.BuffLabel.Text = string.Join(", ", buffs.Select(b => b.Name));
			}
		}
	}
}
