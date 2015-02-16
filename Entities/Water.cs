using UnityEngine;
using System.Collections;
using DG.Tweening;


public class Water : EntityBehaviour
{
    private float distance = 2f;
    private float time = 1.5f;

    void Start()
    {
        base.CacheComponents();
        transform.DOKill();

        DOTween.Sequence().SetLoops(-1, LoopType.Restart)
        	.Append(transform.DOMoveX(distance, time)
        	.SetRelative()
        	.SetEase(Ease.Linear));
    }
}
