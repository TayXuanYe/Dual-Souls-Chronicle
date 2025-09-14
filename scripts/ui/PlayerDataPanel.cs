using Godot;
using System.Linq;

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

	public void UpdatePlayerDataDisplay(CharacterModel characterModel)
	{
		LiveRoomNameLabel.Text = characterModel.CharacterName;
		if (characterModel.IsAssignRole())
		{
			HPLabel.Text = $"{characterModel.Hp}/{characterModel.HpLimit}";
			AttackLabel.Text = characterModel.Attack.ToString();
			DefenseLabel.Text = characterModel.Defense.ToString();
			BuffLabel.Text = string.Join(", ", characterModel.Buff.Select(b => b.Name));
		}
	}
}
