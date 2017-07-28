using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class RenderOrder : BaseBehaviour
{
	public enum Order { 
		SetMe,
		Foreground,
		Player, 
		PlayerWeapon, 
		PlayerProjectile,
		Enemy,
		EnemyWeapon,
		EnemyProjectile,
		Pickup,
		Background
	};

	public Order renderOrder;
	private SpriteRenderer spriteRenderer;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
	}

	#if UNITY_EDITOR
	public void Update()
	{
		//if (Application.isEditor && !Application.isPlaying)
		if (Application.isEditor)
		{
			spriteRenderer.sortingLayerID = 0;

			switch (renderOrder)
			{
				case Order.SetMe:
					spriteRenderer.sortingOrder = SET_ME;
					break;
					
				case Order.Foreground:
					spriteRenderer.sortingOrder = FOREGROUND_ORDER;
					break;
					
				case Order.Player:
					spriteRenderer.sortingOrder = PLAYER_ORDER;
					break;

				case Order.PlayerWeapon:
					spriteRenderer.sortingOrder = PLAYER_WEAPON_ORDER;
					break;

				case Order.PlayerProjectile:
					spriteRenderer.sortingOrder = PLAYER_PROJECTILE_ORDER;
					break;

				case Order.Enemy:
					spriteRenderer.sortingOrder = ENEMY_ORDER;
					break;

				case Order.EnemyWeapon:
					spriteRenderer.sortingOrder = ENEMY_WEAPON_ORDER;
					break;

				case Order.EnemyProjectile:
					spriteRenderer.sortingOrder = ENEMY_PROJECTILE_ORDER;
					break;
					
				case Order.Pickup:
					spriteRenderer.sortingOrder = PICKUP_ORDER;
					break;

				case Order.Background:
					spriteRenderer.sortingOrder = BACKGROUND_ORDER;
					break;
					
				default:
					break;
			}
		}
	}
	#endif
}
