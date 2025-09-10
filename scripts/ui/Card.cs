using Godot;
using System;
using System.Diagnostics;

public partial class Card : VBoxContainer
{
	[Export] private PackedScene _buffFiller;
	[Export] private PackedScene _characterFiller;


	private bool _isInit = false;
	public bool IsSelect = false;
	private StyleBoxFlat _baseStyleBox;
	private (Node node, CardFiller script) _cardFillerInstance;
	public string Id { get; set; }

	public void Init(CardModel cardDto, string type)
	{
		if (_isInit) { return; }
		type = type.ToLower();
		switch (type)
		{
			case "buff":
				var buffNode = _buffFiller.Instantiate();
				if (buffNode is CardFiller buffscript)
				{
					_cardFillerInstance = (buffNode, buffscript);
				}
				break;
			case "character":
				var characterBuff = _characterFiller.Instantiate();
				if (characterBuff is CardFiller characterScript)
				{
					_cardFillerInstance = (characterBuff, characterScript);
				}
				break;
		}
		SetCardFillerDetails(cardDto);
		AddChild(_cardFillerInstance.node);
		_isInit = true;
	}

	private void SetCardFillerDetails(CardModel cardDto)
	{
		_cardFillerInstance.script.NameLabel.Text = cardDto.CardName;
		_cardFillerInstance.script.DescribeLabel.Text = cardDto.Describe;
		_cardFillerInstance.script.ImageTextureRect.Texture = cardDto.ImageTexture;
	}

	public override void _Process(double delta)
	{
		if (!_isInit) { return; }
		if (IsSelect)
		{
			if (_cardFillerInstance.script.CardPanel.GetThemeStylebox("panel") is StyleBoxFlat styleBox)
			{
				styleBox.BorderColor = new Color(1, 0, 0);

				styleBox.BorderWidthLeft = 4;
				styleBox.BorderWidthRight = 4;
				styleBox.BorderWidthTop = 4;
				styleBox.BorderWidthBottom = 4;
			}
		}
		else
		{
			if (_cardFillerInstance.script.CardPanel.GetThemeStylebox("panel") is StyleBoxFlat styleBox)
			{
				styleBox.BorderColor = new Color("#FFFFFFFF");

				styleBox.BorderWidthLeft = 4;
				styleBox.BorderWidthRight = 4;
				styleBox.BorderWidthTop = 4;
				styleBox.BorderWidthBottom = 4;
			}
		}
	}
}
