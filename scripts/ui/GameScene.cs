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
	private int _id;
	private bool _isInit = false;
	private long _pollingIntervalMillis = 0;
	private (Node node, SelectScenes script) selectScene;
	private (Node node, SelectScenes script) battleScene;
	private int selectionAmount = 3;
	// private (Node node, BattleScene script) battleScene;
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
					List<string> messageData = new List<string>();
					foreach (var message in response.Items)
					{
						var messageText = message.Snippet?.DisplayMessage;
						if (messageText.All(c => char.IsDigit(c) || char.IsWhiteSpace(c)))
						{
							messageText.Replace(" ", "");
							char firstChar = messageText[0];
							if (firstChar - '0' != 0 || firstChar - '0' < selectionAmount)
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
							messageData.Add(messageText);
						}
					}

					if (selectScene.script != null)
					{
						var votesData = votingData.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToArray();
						selectScene.script.UpdateVoteCount(votesData);
					}

					if (battleScene.script != null)
					{
						// display text
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

	public override void _Ready()
	{
		Size = new Vector2(960,540);
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
			ViewportData dataNode = parentViewport.GetNode<ViewportData>("Data");
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
			selectSceneScript.Init(_id, 10, colors, 3, "buff", 1);
			selectSceneScript.SetPosition(new Vector2(0, selectSceneScript.Position.Y));
		}

		AddChild(selectScene);
		_ = StartGetChartMessageAsync();

		_isInit = true;
	}
}
