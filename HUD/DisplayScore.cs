using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Matcha.Game.Tweens;


public class DisplayScore : MonoBehaviour 
{
	private Text score;
	private int currentscore;

	void Start () 
    {
		score = gameObject.GetComponent<Text>();
		score.text = currentscore.ToString();
	}

    // EVENT LISTENERS
    void OnEnable()
    {
        Messenger.AddListener<int>("change score", OnChangeScore);
    }

    void OnDisable()
    {
        Messenger.RemoveListener<int>("change score", OnChangeScore);
    }

    // EVENT RESPONDERS
    void OnChangeScore(int newScore)
    {
    	score.text = newScore.ToString();
        MTween.DisplayScore(gameObject, score);
    }
}