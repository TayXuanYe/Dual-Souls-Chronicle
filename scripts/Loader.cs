using Godot;

public partial class Loader : Control
{
	[Export] private PackedScene _mainScene;

	public override void _Ready()
	{
		Node main = _mainScene.Instantiate();
		AddChild(main);
	}
}
