//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Unity;
using Matcha.Dreadful;

namespace Matcha.Dreadful {

	public class MFX : CacheBehaviour
	{
		public static void PickupPrize(GameObject gameObject)
		{
			DOTween.Sequence()
				.Append(gameObject.transform.DOScale(new Vector3(0, 0, 0), .20f)
				.SetEase(Ease.InBounce)
				.OnComplete(()=>TweenCompleted(gameObject)));;
		}

		public static void PickupWeapon(GameObject gameObject)
		{
			DOTween.Sequence()
				.Append(gameObject.transform.DOShakeScale(1f, new Vector3(1f, 1f, 1f), 20, 10f)
				.SetEase(Ease.InBounce));;
		}

		public static void DisplayScore(GameObject gameObject, Text text)
		{
		    gameObject.transform.DOScale(new Vector3(1, 1, 0), 0f);
		    gameObject.transform.DOPunchScale(new Vector3(1f, 1f, 0f), .3f, 3, 1);
		    DOTween.Sequence()
		        .Append(text.DOColor(Color.yellow, .15f))
		        .Append(text.DOColor(Color.white, .15f))
		        .Append(text.DOColor(MCLR.orange, .3f));;
		}

		public static void DisplayScoreFX(GameObject gameObject, Text text)
		{
		    DOTween.Sequence()
		        .Append(gameObject.transform.DOScale(new Vector3(1, 1, 1), 0f))
		        .Append(gameObject.transform.DOScale(new Vector3(1.6f, 1.6f, 0), .10f)
		        .SetEase(Ease.InBounce))
		        .Append(gameObject.transform.DOScale(new Vector3(1, 1, 1), .1f)
		        .SetEase(Ease.Linear));;
		}

		public static void TextFlicker(Text text)
		{
			DOTween.Sequence()
	            .Append(text.DOColor(Color.yellow, .05f))
	            .Append(text.DOColor(MCLR.orange, .05f))
	            .Append(text.DOColor(Color.yellow, .05f))
	            .Append(text.DOColor(MCLR.orange, .05f))
	            .Append(text.DOColor(Color.yellow, .05f))
	            .Append(text.DOColor(MCLR.orange, .05f))
	            .Append(text.DOColor(Color.yellow, .05f))
	            .Append(text.DOColor(MCLR.orange, .3f));;
		}

		public static void TextPulse(Text text)
		{
			DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
			    .Append(text.DOColor(Color.white, .6f).SetEase(Ease.InQuad))
			    .AppendInterval(.2f)
			    .Append(text.DOColor(MCLR.orange, .6f).SetEase(Ease.OutQuad))
			    .AppendInterval(.2f);;
		}

		public static void Fade(SpriteRenderer element, float fadeTo, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(element.DOFade(fadeTo, timeToFade)
				.SetEase(Ease.InOutExpo));
		}

		public static void Fade(Text element, float fadeTo, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(element.DOFade(fadeTo, timeToFade)
				.SetEase(Ease.InOutExpo));
		}

		public static void Fade(Image element, float fadeTo, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(element.DOFade(fadeTo, timeToFade)
				.SetEase(Ease.InOutExpo));
		}

		public static void FadeOutProjectile(SpriteRenderer element, float fadeTo, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(element.DOFade(fadeTo, timeToFade)
				.SetEase(Ease.OutQuart));
		}

		public static void FadeInLevel(SpriteRenderer spriteRenderer, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(spriteRenderer.DOFade(0, timeToFade)
				.SetEase(Ease.InOutExpo));
		}

		public static void FadeOutLevel(SpriteRenderer spriteRenderer, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(spriteRenderer.DOFade(1, timeToFade)
				.SetEase(Ease.OutQuart));
		}

		public static void FadeIntensity(Light light, float targetIntensity, float fadeAfter, float timeToFade)
		{
			if (targetIntensity > 8) { targetIntensity = 8; }

			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(light.DOIntensity(targetIntensity, timeToFade));
		}

		public static void Flicker(Light light, float minIntensity, float maxIntensity)
		{
			light.DOKill();
			DOTween.Sequence()
				.Append(light.DOIntensity(
					Rand.Range(minIntensity, maxIntensity),
					Rand.Range(.1f, .5f)));
		}

		public static void ExtinguishLight(Light light, float extinguishAfter, float timeToExtinguish)
		{
			DOTween.Sequence()
				.AppendInterval(extinguishAfter)
				.Append(light.DOIntensity(8, .00001f))
				.Append(light.DOIntensity(0, timeToExtinguish)
				.SetEase(Ease.InBack));
		}

		public static void FadeToColor(SpriteRenderer spriteRenderer, Color32 newColor, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(spriteRenderer.DOColor(newColor, timeToFade));;
		}

		public static void FadeToColorAndBack(SpriteRenderer spriteRenderer, Color32 newColor, float fadeAfter, float timeToFade)
		{
			DOTween.Sequence()
				.AppendInterval(fadeAfter)
				.Append(spriteRenderer.DOColor(newColor, timeToFade))
				.Append(spriteRenderer.DOColor(MCLR.white, timeToFade))
				.Append(spriteRenderer.DOColor(MCLR.bloodPink, timeToFade))
				.Append(spriteRenderer.DOColor(MCLR.white, timeToFade))
				.Append(spriteRenderer.DOColor(MCLR.bloodPink, timeToFade))
				.Append(spriteRenderer.DOColor(MCLR.white, timeToFade));
		}

		public static void RepulseToLeft(Transform transform, float xDistance, float overTime)
		{
		    transform.DOMove(new Vector3(transform.position.x - xDistance, transform.position.y, transform.position.z), overTime, false);
		}

		public static void RepulseToLeftRandomly(Transform transform, float minDistance, float maxDistance, float overTime)
		{
			float distance = Rand.Range(minDistance, maxDistance);

		    transform.DOMove(new Vector3(transform.position.x - distance, transform.position.y, transform.position.z), overTime, false);
		}

		public static void RepulseToRight(Transform transform, float xDistance, float overTime)
		{
		    transform.DOMove(new Vector3(transform.position.x + xDistance, transform.position.y, transform.position.z), overTime, false);
		}

		public static void RepulseToRightRandomly(Transform transform, float minDistance, float maxDistance, float overTime)
		{
			float distance = Rand.Range(minDistance, maxDistance);

		    transform.DOMove(new Vector3(transform.position.x + distance, transform.position.y, transform.position.z), overTime, false);
		}

		public static void TweenCompleted(GameObject gameObject)
	    {
	        gameObject.GetComponent<Entity>().OnTweenCompleted();
	    }
	}
}
