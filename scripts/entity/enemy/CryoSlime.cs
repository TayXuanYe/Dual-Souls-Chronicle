using Godot;
using System;

public partial class CryoSlime : Entity
{
	public int Shield { get; set; } = 2;
	public int RestorationShieldCounter { get; set; } = 2;
	public bool HaveSluggishness { get; set; } = false;
	[Export] protected AnimatedSprite2D _thorn;
	public override void _Ready()
	{
		_animatedSprite.AnimationFinished += OnAnimationFinished;

		HpLimit = 100;
		Hp = 200;
		Attack = 60;
		Defense = 30;
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
		if (HaveSluggishness)
		{
			HaveSluggishness = false;
			PlayAnimation("sluggishness");
		}
		else
		{
			HaveSluggishness = true;
			PlayAnimation("attack", entity.GlobalPosition);
		}
		attackEntity = entity;
	}

	protected override void PlayAnimation(string animName)
	{
		_animatedSprite.Play(animName);
	}
	protected override void PlayAnimation(string animName, Vector2 position)
	{
		_animatedSprite.Play(animName);
		// move position
		_thorn.GlobalPosition = position;
	}

	public override void Attacked(int damage)
	{
		string displayText;
		if (Shield == 0)
		{
			if (Hp < damage)
			{
				Hp = 0;
			}
			else
			{
				Hp -= damage;
			}
			displayText = "-" + damage.ToString();
		}
		else
		{
			Shield--;
			displayText = "Shattered Shield";
		}
		
		PlayAnimation("attacked");
		// display damage value
		var damageLabel = _damageLabelScene.Instantiate();
		if (damageLabel is DamageLabel script)
		{
			script._damageLabel.Text = displayText;
		}
	}

	public void RestoreShield()
	{
		RestorationShieldCounter--;
		if (RestorationShieldCounter == 0)
		{
			RestorationShieldCounter = 2;
			Shield = 2;
			PlayAnimation("restoreShield");
		}
	}
	

	protected override void OnAnimationFinished()
	{
		string finishedAnimationName = _animatedSprite.Animation;
		switch (finishedAnimationName)
		{
			case "sluggishness":
				SignalManager.Instance.EmitSluggishnessCharacterSignal(attackEntity.Id);
				int sluggishnessDamage = CalculateDamage(Attack, attackEntity.Defense, CriticalDamage, CriticalRate);
				attackEntity.Attacked(sluggishnessDamage);
				break;
			case "attack":
				PlayAnimation("idle");
				int damage = CalculateDamage(Attack, attackEntity.Defense, CriticalDamage, CriticalRate);
				attackEntity.Attacked(damage);
				GlobalPosition = OriginGlobalPosition;
				break;
			case "idleHalfShield":
				PlayAnimation("idleHalfShield");
				break;
			case "idleNoShield":
				PlayAnimation("idleNoShield");
				break;
			case "idle":
			case "restoreShield":
				PlayAnimation("idle");
				break;
			case "attacked":
				if (Shield == 2)
				{
					PlayAnimation("idle");
				}
				else if (Shield != 0)
				{
					PlayAnimation("idleHalfShield");
				}
				else
				{
					PlayAnimation("idleNoShield");
				}
				_thorn.GlobalPosition = new Vector2(-1000, -1000);
				break;
			default:
				return;
		}
	}
}
