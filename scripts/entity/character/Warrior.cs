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
		_id = id;
		originGlobalPosition = globalPosition;
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
		_animatedSprite.Play(animName);
		// move position
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
