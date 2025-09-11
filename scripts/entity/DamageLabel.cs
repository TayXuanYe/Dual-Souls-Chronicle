using Godot;
using System;

public partial class DamageLabel : Node2D
{
	[Export] public Label _damageLabel;
	private float _moveSpeed = 100;
	private float lifetime = 1.0f;

	public override void _Ready()
	{
		var tween = CreateTween();
		tween.TweenProperty(this, "global_position", GlobalPosition + new Vector2(0, -50), 1.0).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(_damageLabel, "modulate:a", 0.0, 1.0).SetEase(Tween.EaseType.Out);
		tween.Finished += QueueFree;
	}
}
