using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UpdateHUDLives : MonoBehaviour
{
    public Sprite threeLives;
    public Sprite twoLives;
    public Sprite oneLife;

    void Start()
    {
        gameObject.GetComponent<Image>().sprite = threeLives;
    }
}
