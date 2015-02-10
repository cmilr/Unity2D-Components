using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Matcha.Game.Tweens;


public class DisplayScoreFX : MonoBehaviour 
{
    private Text scoreUI;
    private int currentscore;

    void Start () 
    {
        scoreUI = gameObject.GetComponent<Text>();
        scoreUI.text = currentscore.ToString();
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
        scoreUI.text = newScore.ToString();
        MTween.DisplayScoreFX(gameObject, scoreUI);
    }
}