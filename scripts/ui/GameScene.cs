using Godot;
using System;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;

public partial class GameScene : Control
{
	[Export] private PackedScene _selectScene;
	private int _id;
	private bool _isInit = false;
	private long _pollingIntervalMillis = 0;
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
					foreach (var message in response.Items)
					{
						var messageText = message.Snippet?.DisplayMessage;
						GD.Print($"[{DateTime.Now}] : {messageText}");
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
		// _ = StartGetChartMessageAsync();

		_isInit = true;
	}
}
