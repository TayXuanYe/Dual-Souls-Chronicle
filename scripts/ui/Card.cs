using Godot;
using System;

public partial class Card : VBoxContainer
{
	[Export] private Label _buffNameLabel;
	[Export] private TextureRect _imageTextureRect;
	[Export] private Label _describeLabel;
	[Export] CollisionPolygon2D _markerCollisionPolygon2D;
	[Export] Panel _cardPanel;
	private bool _isInit = true;
	public bool IsSelect = false;
	public void Init(string buffName, TextureRect imageTextureRect, string describe)
	{
		if(!_isInit) { return; }
		_buffNameLabel.Text = buffName;
		_imageTextureRect = imageTextureRect;
		_describeLabel.Text = describe;
		Visible = true;
	}
	
	public override void _Ready()
	{
		Visible = false;
	}
	
	public override void _Process(double delta)
	{
		if(!_isInit) { return; }
		if (IsSelect)
		{
			_markerCollisionPolygon2D.Visible = true;
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
			_markerCollisionPolygon2D.Visible = false;
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
