using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UpdateHUDLives : BaseBehaviour
{
	public Sprite threeLives;
	public Sprite twoLives;
	public Sprite oneLife;

	void Start()
	{
		gameObject.GetComponent<Image>().sprite = threeLives;
	}
}
