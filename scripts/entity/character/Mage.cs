using Godot;
using System;

public partial class Mage : Entity
{
	[Export] protected AnimatedSprite2D _boom;

	public override void _Ready()
	{
		_animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	public void Init(string id, Vector2 globalPosition)
	{
		_id = id;
		OriginGlobalPosition = globalPosition;
		GlobalPosition = GlobalPosition;
	}

	private Entity attackEntity;
	public override void AttackEntity(Entity entity)
	{
		attackEntity = entity;
		// show animation
		PlayAnimation("attack", entity.GlobalPosition);
	}

	private string _idleAnimationName = "idle";
	private string _attackedAnimationName = "attacked";
	protected override void PlayAnimation(string animName)
	{
		_animatedSprite.Play(animName);
	}
	protected override void PlayAnimation(string animName, Vector2 position)
	{
		_boom.Position = OriginGlobalPosition;
		_boom.Play(animName);
		// move position
		MoveInParabola(position, 1, 200);
	}
	
	private void MoveInParabola(Vector2 targetPosition, double travelTime, float peakHeight)
	{
		if (travelTime <= 0)
		{
			_boom.GlobalPosition = targetPosition;
			return;
		}

		var tween = CreateTween();

		tween.TweenProperty(this, "parabola_progress", 1.0, travelTime);
		
		var startPosition = _boom.GlobalPosition;
		
		tween.Connect("parabola_progress", Callable.From((float progress) =>
		{
			var newPosition = startPosition.Lerp(targetPosition, progress);
			var t = progress * 2 - 1; 
			var heightOffset = -peakHeight * (t * t - 1);
			_boom.GlobalPosition = newPosition + Vector2.Up * heightOffset;
		}));
	}

	public override void Attacked(int damage)
	{
		CharacterModel characterModel = CharacterDataManager.Instance.Characters[_id];
		if (characterModel.Hp < damage)
		{
			characterModel.Hp = 0;
		}
		else
		{
			characterModel.Hp -= damage;
		}
		PlayAnimation("attacked");
		// display damage value
		var damageLabel = _damageLabelScene.Instantiate();
		if (damageLabel is DamageLabel script)
		{
			script._damageLabel.Text = "-" + damage.ToString();
		}
	}

	protected override void OnAnimationFinished()
	{
		string finishedAnimationName = _animatedSprite.Animation;
		switch (finishedAnimationName)
		{
			case "attack":
				PlayAnimation("idle");
				CharacterModel characterModel = CharacterDataManager.Instance.Characters[_id];
				int damage = CalculateDamage(characterModel.Attack, attackEntity.Defense, characterModel.CriticalDamage, characterModel.CriticalRate);
				attackEntity.Attacked(damage);
				_boom.GlobalPosition = new Vector2(-1000,-1000);
				break;
			case "idle":
				PlayAnimation("idle");
				break;
			case "attacked":
				PlayAnimation("idle");
				break;
			default:
				return;
		}
	}
}
