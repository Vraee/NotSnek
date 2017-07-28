using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour {

    public Animator anime;
    private Text damageText;
	// Use this for initialization
	void Start () {
        AnimatorClipInfo[] clipInfo = anime.GetCurrentAnimatorClipInfo(0);
        //Destroys the gameObject once the animation is over
        Destroy(gameObject, clipInfo[0].clip.length);
	}

    public void SetText(string texti)
    {
        GetComponentInChildren<Text>().text = "+" + texti;
    }
	
}
