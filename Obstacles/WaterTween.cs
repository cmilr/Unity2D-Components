using DG.Tweening;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.Assertions;

public class WaterTween : BaseBehaviour
{
	private float distance = 2f;
	private float time = 1.5f;
	private new Transform transform;
	private Sequence waterMovement;
	
	void Awake()
	{
		transform = GetComponent<Transform>();
		Assert.IsNotNull(transform);
	}
	
	void Start()
	{
		(waterMovement = MFX.ContinualWaterMovement(transform, distance, time)).Pause();

		waterMovement.Restart();
	}
}
