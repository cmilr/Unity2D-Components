using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Matcha.Game.Tweens;


public class DisplayScoreFX : BaseBehaviour
{
	private Text scoreUI;
	private int currentscore;
	private float timeToFade = 2f;

	void Start ()
	{
		scoreUI = gameObject.GetComponent<Text>();
		scoreUI.text = currentscore.ToString();
		MTween.FadeOutText(scoreUI, 0, 0);
		MTween.FadeInText(scoreUI, 1, timeToFade);
	}

	// EVENT LISTENERS
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

	// EVENT RESPONDERS
	void OnChangeScore(int newScore)
	{
		scoreUI.text = newScore.ToString();
		MTween.DisplayScoreFX(gameObject, scoreUI);
	}

	void OnPlayerDead(string methodOfDeath, Collider2D coll)
	{
		MTween.FadeOutText(scoreUI, .5f, timeToFade);
	}
}