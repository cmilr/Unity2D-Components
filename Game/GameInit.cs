using UnityEngine;
using System.Collections;
using DG.Tweening;


public class GameInit : MonoBehaviour
{
	void Awake()
	{
		// seed Random with current seconds;
		Random.seed = (int)System.DateTime.Now.Ticks;

		// initialize DOTween before first use.
		DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
		DOTween.SetTweensCapacity(2000, 100);

		// ignore collisions between
        Physics2D.IgnoreLayerCollision(LayerID("BodyCollider"), LayerID("One-Way Platform"), true);
        Physics2D.IgnoreLayerCollision(LayerID("WeaponCollider"), LayerID("Enemies"), true);
        Physics2D.IgnoreLayerCollision(LayerID("WeaponCollider"), LayerID("Collectables"), true);
	}

	int LayerID(string layerName)
	{
		return LayerMask.NameToLayer(layerName);
	}
}
