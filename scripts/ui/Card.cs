using Godot;
using System;
using System.Diagnostics;

public partial class Card : VBoxContainer
{
	[Export] private Label _nameLabel;
	[Export] private TextureRect _imageTextureRect;
	[Export] private Label _describeLabel;
	[Export] private Panel _cardPanel;
	[Export] private Panel _imageContainerPanel;
	[Export] VBoxContainer _container;
	private bool _isInit = false;
	public bool IsSelect = false;
	private StyleBoxFlat _baseStyleBox;
	public string Id { get; set; }

	public void Init(CardDto cardDto, string type)
	{
		if (_isInit) { return; }
		switch (type)
		{
			case "buff":
				break;
			case "character":
				_container.MoveChild(_nameLabel, 1);
				break;
		}

		Id = cardDto.Id;
		_nameLabel.Text = cardDto.Name;
		_imageTextureRect.Texture = cardDto.ImageTexture;
		_describeLabel.Text = cardDto.Describe;

		if (_cardPanel.GetThemeStylebox("panel") is StyleBoxFlat styleBox)
		{
			_baseStyleBox = (StyleBoxFlat)styleBox.Duplicate();
			_cardPanel.AddThemeStyleboxOverride("panel", _baseStyleBox);
		}

		_isInit = true;
	}

	public override void _Ready()
	{
		Visible = true;
	}

	public override void _Process(double delta)
	{
		if (!_isInit) { return; }
		if (IsSelect)
		{
			if (_cardPanel.GetThemeStylebox("panel") is StyleBoxFlat styleBox)
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
			if (_cardPanel.GetThemeStylebox("panel") is StyleBoxFlat styleBox)
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
