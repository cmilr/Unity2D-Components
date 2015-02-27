using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Matcha.Game.Tweens;


public class DisplayScore : BaseBehaviour
{
	public bool topLayer;
	public GameManager gameManager;
	private Text scoreUI;
	private int currentScore;
	private float timeToFade = 2f;

	void Start ()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		currentScore = gameManager.CurrentScore;

		scoreUI = gameObject.GetComponent<Text>();
		scoreUI.text = currentScore.ToString();

		// fade score to zero instantly upon start-up, then fade up slowly
		MTween.FadeOutText(scoreUI, 0, 0);
		MTween.FadeInText(scoreUI, HUD_FADE_IN_AFTER, timeToFade);
	}

	void OnChangeScore(int newScore)
	{
		scoreUI.text = newScore.ToString();

		if (topLayer)
		{
			MTween.DisplayScore(gameObject, scoreUI);
		}
		else 
		{
			MTween.DisplayScoreFX(gameObject, scoreUI);			
		}
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		MTween.FadeOutText(scoreUI, HUD_FADE_OUT_AFTER, timeToFade);
	}

	void OnEnable()
	{
		Messenger.AddListener<int>("change score", OnChangeScore);
		Messenger.AddListener<string, Collider2D>( "player dead", OnPlayerDead);
	}
	
	void OnDestroy()
	{
		Messenger.RemoveListener<int>("change score", OnChangeScore);
		Messenger.RemoveListener<string, Collider2D>( "player dead", OnPlayerDead);
	}
}