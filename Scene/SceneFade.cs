using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SceneFade : CacheBehaviour {

	public float timeToFade = 1f;

	void Start() 
	{
		base.CacheComponents();
		
		spriteRenderer.DOKill();

		DOTween.Sequence()
			.Append(spriteRenderer.DOFade(0, timeToFade).SetEase(Ease.InOutExpo));
	}
}
