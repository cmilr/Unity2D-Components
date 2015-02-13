using UnityEngine;
using System.Collections;
using DG.Tweening;


public class SceneManager : MonoBehaviour {

	public float timeToFade = 1f;
	private SpriteRenderer spriteRenderer;

	void Start() 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.DOKill();

		FadeIn();
	}

	void FadeIn()
	{
		DOTween.Sequence()
			.Append(spriteRenderer.DOFade(0, timeToFade)
			.SetEase(Ease.InOutExpo));
	}

	void FadeOut()
	{
		DOTween.Sequence()
			.AppendInterval(1f)
			.Append(spriteRenderer.DOFade(100, timeToFade)
			.SetEase(Ease.InOutExpo));
	}

	// EVENT LISTENERS
	void OnEnable()
	{
		Messenger.AddListener<bool>( "player dead", OnPlayerDead);
	}

	void OnDestroy()
	{
		Messenger.RemoveListener<bool>( "player dead", OnPlayerDead);
	}

	// EVENT RESPONDERS
	void OnPlayerDead(bool value)
	{
		FadeOut();

		StartCoroutine(Timer.Start(timeToFade + 1, true, () =>
		{
			Application.LoadLevel(Application.loadedLevel);
		}));
	}
}
