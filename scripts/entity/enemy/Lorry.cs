using Godot;
using System;

public partial class Lorry : Entity
{
	public override void _Ready()
	{
		_animatedSprite.AnimationFinished += OnAnimationFinished;
		
		HpLimit = 240;
		Hp = 240;
		Attack = 300;
		Defense = 30;
	}

	public void Init(string id, Vector2 globalPosition)
	{
		_id = id;
		OriginGlobalPosition = globalPosition;
		GlobalPosition = GlobalPosition;
	}

	private Entity attackEntity;
	private bool _isAccumulate = false; 
	public override void AttackEntity(Entity entity)
	{
		if (!_isAccumulate)
		{
			PlayAnimation("accumulate");
			_isAccumulate = true;
		}
		else
		{
			_isAccumulate = false;
			attackEntity = entity;
			// show animation
			PlayAnimation("attack", entity.GlobalPosition);
		}
	}

	protected override void PlayAnimation(string animName)
	{
		_animatedSprite.Play(animName);
	}
	protected override void PlayAnimation(string animName, Vector2 position)
	{
		_animatedSprite.Play(animName);
		// move position
		MoveInStraightLine(position, 1);
	}
	
	public void MoveInStraightLine(Vector2 targetPosition, double travelTime)
	{
		if (travelTime <= 0)
		{
			GlobalPosition = targetPosition;
			return;
		}

		var tween = CreateTween();
		
		tween.TweenProperty(this, "global_position", targetPosition, travelTime);
	}

	public override void Attacked(int damage)
	{
		if (Hp < damage)
		{
			Hp = 0;
		}
		else
		{
			Hp -= damage;
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
				int damage = CalculateDamage(Attack, attackEntity.Defense, CriticalDamage, CriticalRate);
				attackEntity.Attacked(damage);
				GlobalPosition = OriginGlobalPosition;
				break;
			case "idle":
				PlayAnimation("idle");
				break;
			case "attacked":
				PlayAnimation("idle");
				break;
			case "accumulate":
				PlayAnimation("idle");
				break;
			default:
				return;
		}
	}
}
