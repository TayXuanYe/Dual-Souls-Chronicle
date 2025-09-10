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
	
	private int _id;
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
				YoutubeServices service;
				if (_id == 1)
				{
					service = YoutubeManager.YoutubeServices1;
				}
				else if (_id == 2)
				{
					service = YoutubeManager.YoutubeServices2;
				}
				else
				{
					return;
				}

				var (response, newNextPageToken) = await service.GetChatMessagesAsync();

				if (response?.Items != null)
				{
					_pollingIntervalMillis = response.PollingIntervalMillis ?? 5000;
					Dictionary<int, int> votingData = new Dictionary<int, int>();
					foreach (var message in response.Items)
					{
						var messageText = message.Snippet?.DisplayMessage;
						if (messageText.All(c => char.IsDigit(c) || char.IsWhiteSpace(c)))
						{
							messageText.Replace(" ", "");
							char firstChar = messageText[0];
							if (firstChar - '0' != 0 || firstChar - '0' < _selectionAmount)
							{
								bool foundNotSame = false;
								for (int i = 1; i < messageText.Count(); i++)
								{
									if (messageText[i] != firstChar)
									{
										foundNotSame = true;
										break;
									}
								}

								if (!foundNotSame)
								{
									votingData.Add(firstChar - '0', 1);
								}
							}
						}
						else
						{
							SignalManager.Instance.EmitChatSignal(messageText, _id);
							GD.Print($"EmitSignal: {messageText},{_id}");
						}
					}

					if (_selectSceneInstant.script != null)
					{
						var votesData = votingData.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToArray();
						_selectSceneInstant.script.UpdateVoteCount(votesData);
					}
				}
			}
			catch (Exception ex)
			{
				GD.PrintErr($"Error: {ex.Message}");
				break;
			}
		}
	}

	private int maxDialogAmount = 5;
	public void OnDisplayDialog(string message, int id)
	{
		GD.Print($"Receive signal in {_id}, {message},{id}");
		if (id == 1)
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
		if (id == 2)
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
		SignalManager.Instance.DisplayDialog  += OnDisplayDialog;

		Size = new Vector2(960, 720);
		Node current = this;
		SubViewport parentViewport = null;
		while (current != null)
		{
			if (current is SubViewport viewport)
			{
				parentViewport = viewport;
				break;
			}
			current = current.GetParent();
		}

		if (parentViewport != null)
		{
			ViewportId dataNode = parentViewport.GetNode<ViewportId>("Data");
			if (dataNode != null)
			{
				_id = dataNode.Id;
				GD.Print($"Init id:{_id}");
			}
			else
			{
				GD.Print($"Init id: fail");
			}
		}
		else
		{
			GD.Print($"Owner not found");
		}

		Node selectScene = _selectScene.Instantiate();
		if (selectScene is SelectScenes selectSceneScript)
		{
			string[] colors = ["#66CCFF", "#FFEED0", "#eeff00ff"];
			selectSceneScript.Init(_id, 10, colors, 3, "character", 1);
			selectSceneScript.SetPosition(new Vector2(0, selectSceneScript.Position.Y));
		}

		AddChild(selectScene);

		if (_player1DataPanel is PlayerDataPanel player1DataPanelScript)
		{
			player1DataPanelScript.LiveRoomNameLabel.Text = CharacterDataManager.Instance.Characters[1].CharacterName;
		}

		if (_player2DataPanel is PlayerDataPanel player2DataPanelScript)
		{
			player2DataPanelScript.LiveRoomNameLabel.Text = CharacterDataManager.Instance.Characters[2].CharacterName;
		}

		_ = StartGetChartMessageAsync();

		_isInit = true;
	}
}
