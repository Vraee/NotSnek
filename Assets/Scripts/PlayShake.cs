using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShake : MonoBehaviour {

    private Animator anime;

	// Use this for initialization with the bounce
	void Start () {
        anime = GetComponent<Animator>();
	}

    public void Shake()
    {
        //Shake it, yeah
        if(anime != null) {
            anime.Play("MultiplierShake");
        }
    }
}
