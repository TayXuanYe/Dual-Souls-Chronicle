using Godot;
using System;

public partial class PlayerDataPanel : Panel
{
	[Export] public Label LiveRoomNameLabel;
	[Export] public Label HPLabel;
	[Export] public Label AttackLabel;
	[Export] public Label DefenseLabel;
	[Export] public Label BuffLabel;
	public override void _Ready()
	{
		HPLabel.Text = "???/???";
		AttackLabel.Text = "???";
		DefenseLabel.Text = "???";
	}
}
