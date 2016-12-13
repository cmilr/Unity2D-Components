using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : BaseBehaviour
{
	private int diffDamageModifier;
	private PlayerData player;
	private SpriteRenderer spriteRenderer;
	private Sequence fadeWhenHit;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}

	void Start()
	{
		player = GameObject.Find(_DATA).GetComponent<PlayerData>();
		Assert.IsNotNull(player);

		(fadeWhenHit = MFX.FadeToColorAndBack(spriteRenderer.material, MCLR.bloodRed, 0f, .2f)).Pause();

		Init();
	}

	void Init()
	{
		EventKit.Broadcast("init lvl", player.LVL);
		EventKit.Broadcast("init hp", player.HP);
		EventKit.Broadcast("init ac", player.AC);
		EventKit.Broadcast("init xp", player.XP);
		EventKit.Broadcast("init weapons", player.equippedWeapon, player.leftWeapon, player.rightWeapon);
	}

	public void TakesHit(Hit hit)
	{
		player.HP -= (hit.weapon.damage * diffDamageModifier);

		//params for ShakeCamera = duration, strength, vibrato, randomness
		EventKit.Broadcast("shake camera", .2f, .1f, 10, 3f);
		EventKit.Broadcast("reduce hp", player.HP);

		if (hit.horizontalSide == Side.Right)
		{
			gameObject.SendEventDown("RepulseToLeft", 5.0F);
		}
		else
		{
			gameObject.SendEventDown("RepulseToRight", 5.0F);
		}

		if (player.HP > 0)
		{
			fadeWhenHit.Restart();
		}
		else
		{
			OnPlayerDead(hit);
			EventKit.Broadcast("player dead", hit);
		}
	}

	void OnSetDiffDamageModifier(int modifier)
	{
		diffDamageModifier = modifier;
	}

	void OnPlayerDead(Hit incomingHit)
	{
		spriteRenderer.enabled = false;
	}

	void OnPlayerDrowned(Collider2D incomingColl)
	{
		spriteRenderer.enabled = false;
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("set difficulty damage modifier", OnSetDiffDamageModifier);
		EventKit.Subscribe<Collider2D>("player drowned", OnPlayerDrowned);
		EventKit.Subscribe<Hit>("player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("set difficulty damage modifier", OnSetDiffDamageModifier);
		EventKit.Unsubscribe<Collider2D>("player drowned", OnPlayerDrowned);
		EventKit.Unsubscribe<Hit>("player dead", OnPlayerDead);
	}
}

