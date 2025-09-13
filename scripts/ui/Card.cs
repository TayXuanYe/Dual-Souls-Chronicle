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
	public string CarryData { get; set; }

	public void Init(CardModel cardModel, string type)
	{
		if (_isInit) { return; }
		type = type.ToLower();
		Id = cardModel.Id;
		CarryData = cardModel.CarryData;
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
		SetCardFillerDetails(cardModel);
		AddChild(_cardFillerInstance.node);
		_isInit = true;
	}

	private void SetCardFillerDetails(CardModel cardDto)
	{
		_cardFillerInstance.script.NameLabel.Text = cardDto.CardName;
		_cardFillerInstance.script.DescribeLabel.Text = cardDto.Describe;
		_cardFillerInstance.script.ImageTextureRect.Texture = cardDto.ImageTexture;
	}
	public override void _Ready()
	{
		if (_cardFillerInstance.script.CardPanel.GetThemeStylebox("panel") is StyleBoxFlat styleBox)
		{
			_baseStyleBox = (StyleBoxFlat)styleBox.Duplicate();
			_cardFillerInstance.script.CardPanel.AddThemeStyleboxOverride("panel", _baseStyleBox);
		}
	}

	public override void _Process(double delta)
	{
		if (!_isInit) { return; }
		if (IsSelect)
		{
			_baseStyleBox.BorderColor = new Color(1, 0, 0);

			_baseStyleBox.BorderWidthLeft = 4;
			_baseStyleBox.BorderWidthRight = 4;
			_baseStyleBox.BorderWidthTop = 4;
			_baseStyleBox.BorderWidthBottom = 4;
		}
		else
		{
			_baseStyleBox.BorderColor = new Color("#FFFFFFFF");

			_baseStyleBox.BorderWidthLeft = 4;
			_baseStyleBox.BorderWidthRight = 4;
			_baseStyleBox.BorderWidthTop = 4;
			_baseStyleBox.BorderWidthBottom = 4;
		}
	}
}
