using DG.Tweening;
using Matcha.Unity;
using Matcha.Dreadful;
using UnityEngine;
using UnityEngine.UI;

namespace Matcha.Dreadful
{
	static class MFX
	{
		public static Sequence Pulse(GameObject gameObject)
		{
			return DOTween.Sequence()
			.Append(gameObject.transform.DOScale(new Vector3(130, 130, 1), .5f))
			.SetEase(Ease.InCirc)
			.SetLoops(-1, LoopType.Restart)
            .SetRecyclable(true);
		}
		
		public static Sequence MoveBackAndForthHorizontal(float pauseTime, Transform transform, float distance, float time)
		{
			return DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.AppendInterval(pauseTime)
			.Append(transform.DOMoveX(distance, time).SetRelative().SetEase(Ease.InOutQuad))
			.AppendInterval(pauseTime)
            .SetRecyclable(true);
		}
		
		public static Sequence MoveBackAndForthVertical(float pauseTime, Transform transform, float distance, float time)
		{
			return DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			.AppendInterval(pauseTime)
			.Append(transform.DOMoveY(distance, time).SetRelative().SetEase(Ease.InOutQuad))
			.AppendInterval(pauseTime)
            .SetRecyclable(true);
		}
		
		public static Sequence ContinualWaterMovement(Transform transform, float distance, float time)
		{
			return DOTween.Sequence().SetLoops(-1, LoopType.Restart)
			.Append(transform.DOMoveX(distance, time)
			.SetRelative()
			.SetEase(Ease.Linear))
            .SetRecyclable(true);
		}
		
		public static Sequence PickupPrize(GameObject gameObject)
		{
			return DOTween.Sequence()
			.Append(gameObject.transform.DOScale(new Vector3(0, 0, 0), .20f)
			.SetEase(Ease.InBounce)
			.OnComplete(() => TweenCompleted(gameObject)))
            .SetRecyclable(true);
		}

		public static Sequence PickupWeapon(GameObject gameObject)
		{
			return DOTween.Sequence()
			.Append(gameObject.transform.DOShakeScale(1f, new Vector3(1f, 1f, 1f), 20, 10f)
			.SetEase(Ease.InBounce))
            .SetRecyclable(true);
		}
		
		public static Tween SlideStashedItem(Transform transform, Vector3 newPos, float distance, float speed, bool snapping)
		{
			return transform.DOLocalMove(new Vector3(newPos.x + distance, newPos.y, newPos.z), speed, snapping).SetRecyclable(true);
		}

		public static Tween SetToStashedTint(SpriteRenderer element)
		{
			return element.DOColor(MCLR.gray, 0f)
			.SetRecyclable(true);
		}

		public static Sequence FadeToStashedTint
		(SpriteRenderer element, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(element.DOColor(MCLR.gray, timeToFade).SetEase(Ease.InOutExpo))
			.SetRecyclable(true);
		}

		public static Tween SetToNoTint(SpriteRenderer element)
		{
			return element.DOColor(MCLR.white, 0f)
			.SetRecyclable(true);
		}

		public static Sequence FadeToNoTint(SpriteRenderer element, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(element.DOColor(MCLR.white, timeToFade).SetEase(Ease.InOutExpo))
			.SetRecyclable(true);
		}

		public static Sequence DisplayScore(GameObject gameObject, Text text)
		{
			return DOTween.Sequence()
		   	.Append(gameObject.transform.DOScale(new Vector3(1, 1, 0), 0f))
		   	.Append(gameObject.transform.DOPunchScale(new Vector3(1f, 1f, 0f), .3f, 3, 1))
			.Append(text.DOColor(Color.yellow, .15f))
			.Append(text.DOColor(Color.white, .15f))
			.Append(text.DOColor(MCLR.orange, .3f))
            .SetRecyclable(true);
		}

		public static Sequence DisplayScoreFX(GameObject gameObject)
		{
			return DOTween.Sequence()
			.Append(gameObject.transform.DOScale(new Vector3(1, 1, 1), 0f))
			.Append(gameObject.transform.DOScale(new Vector3(1.6f, 1.6f, 0), .10f).SetEase(Ease.InBounce))
			.Append(gameObject.transform.DOScale(new Vector3(1, 1, 1), .1f).SetEase(Ease.Linear))
            .SetRecyclable(true);
		}

		public static Sequence Fade(SpriteRenderer element, float fadeTo, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(element.DOFade(fadeTo, timeToFade).SetEase(Ease.InOutExpo))
            .SetRecyclable(true);
		}

		public static Sequence Fade(Text element, float fadeTo, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(element.DOFade(fadeTo, timeToFade).SetEase(Ease.InOutExpo))
            .SetRecyclable(true);
		}

		public static Sequence Fade(Image element, float fadeTo, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(element.DOFade(fadeTo, timeToFade).SetEase(Ease.InOutExpo))
            .SetRecyclable(true);
		}

		public static Tween FadeTween(SpriteRenderer element, float fadeTo, float timeToFade)
		{
			return element.DOFade(fadeTo, timeToFade)
            .SetRecyclable(true);
		}

		public static Sequence FadeInWeapon(SpriteRenderer element, float fadeTo, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(element.DOFade(fadeTo, timeToFade).SetEase(Ease.InOutQuad))
            .SetRecyclable(true);
		}

		public static Sequence FadeInViewport(SpriteRenderer spriteRenderer, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(spriteRenderer.DOFade(0, timeToFade).SetEase(Ease.InOutExpo))
            .SetRecyclable(true);
		}

		public static Sequence FadeOutViewport(SpriteRenderer spriteRenderer, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(spriteRenderer.DOFade(1, timeToFade).SetEase(Ease.OutQuart))
            .SetRecyclable(true);
		}

		public static Sequence FadeIntensity(Light light, float targetIntensity, float fadeAfter, float timeToFade)
		{
			if (targetIntensity > 8) { targetIntensity = 8; }

			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(light.DOIntensity(targetIntensity, timeToFade))
            .SetRecyclable(true);
		}

		public static Sequence ExtinguishLight(Light light, float extinguishAfter, float timeToExtinguish)
		{
			return DOTween.Sequence()
			.AppendInterval(extinguishAfter)
			.Append(light.DOIntensity(8, .00001f))
			.Append(light.DOIntensity(0, timeToExtinguish).SetEase(Ease.InBack))
            .SetRecyclable(true);
		}

		public static Sequence FadeToColor(SpriteRenderer spriteRenderer, Color32 newColor, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(spriteRenderer.DOColor(newColor, timeToFade))
            .SetRecyclable(true);
		}

		public static Sequence FadeToColorAndBack(SpriteRenderer spriteRenderer, Color32 newColor, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(spriteRenderer.DOColor(newColor, timeToFade))
			.Append(spriteRenderer.DOColor(MCLR.white, timeToFade))
            .SetRecyclable(true);
		}
		
		public static Sequence FadeToColorAndBack(Material material, Color32 newColor, float fadeAfter, float timeToFade)
		{
			return DOTween.Sequence()
			.AppendInterval(fadeAfter)
			.Append(material.DOColor(newColor, timeToFade))
            .Append(material.DOColor(MCLR.white, timeToFade))
            .SetRecyclable(true);
		}

		public static Tween RepulseToLeft(Transform transform, float xDistance, float overTime)
		{
			return transform.DOMove(new Vector3(transform.position.x - xDistance, transform.position.y, transform.position.z), overTime, false)
            .SetRecyclable(true);
		}

		public static Tween RepulseToLeftRandomly(Transform transform, float minDistance, float maxDistance, float overTime)
		{
			float distance = Rand.Range(minDistance, maxDistance);

			return transform.DOMove(new Vector3(transform.position.x - distance, transform.position.y, transform.position.z), overTime, false)
            .SetRecyclable(true);
		}

		public static Tween RepulseToRight(Transform transform, float xDistance, float overTime)
		{
			return transform.DOMove(new Vector3(transform.position.x + xDistance, transform.position.y, transform.position.z), overTime, false)
            .SetRecyclable(true);
		}

		public static Tween RepulseToRightRandomly(Transform transform, float minDistance, float maxDistance, float overTime)
		{
			float distance = Rand.Range(minDistance, maxDistance);

			return transform.DOMove(new Vector3(transform.position.x + distance, transform.position.y, transform.position.z), overTime, false)
            .SetRecyclable(true);
		}

		public static void TweenCompleted(GameObject gameObject)
		{
			gameObject.GetComponent<Entity>().OnTweenCompleted();
		}
	}
}
