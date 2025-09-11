using Godot;
using System;

public partial class Warrior : Entity
{
	public override void _Ready()
	{
		_animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	public void Init(string id, Vector2 globalPosition)
	{
		Id = id;
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

	protected override void PlayAnimation(string animName)
	{
		_animatedSprite.Play(animName);
	}
	protected override void PlayAnimation(string animName, Vector2 position)
	{
		_animatedSprite.Play(animName);
		// move position
		MoveInParabola(position, 1, 300);
	}
	
	private void MoveInParabola(Vector2 targetPosition, double travelTime, float peakHeight)
	{
		if (travelTime <= 0)
		{
			GlobalPosition = targetPosition;
			return;
		}

		var tween = CreateTween();

		tween.TweenProperty(this, "parabola_progress", 1.0, travelTime);
		
		var startPosition = GlobalPosition;
		
		tween.Connect("parabola_progress", Callable.From((float progress) =>
		{
			var newPosition = startPosition.Lerp(targetPosition, progress);
			var t = progress * 2 - 1; 
			var heightOffset = -peakHeight * (t * t - 1);
			GlobalPosition = newPosition + Vector2.Up * heightOffset;
		}));
	}

	public override void Attacked(int damage)
	{
		CharacterModel characterModel = CharacterDataManager.Instance.Characters[Id];
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
				CharacterModel characterModel = CharacterDataManager.Instance.Characters[Id];
				int damage = CalculateDamage(characterModel.Attack, attackEntity.Defense, characterModel.CriticalDamage, characterModel.CriticalRate);
				attackEntity.Attacked(damage);
				GlobalPosition = OriginGlobalPosition;
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
