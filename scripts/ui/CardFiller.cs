using Godot;
using System;

public partial class CardFiller : Panel
{
	[Export] public Label NameLabel { get; set; }
	[Export] public TextureRect ImageTextureRect { get; set; }
	[Export] public Label DescribeLabel { get; set; }
	[Export] public Panel CardPanel { get; set; }
}
