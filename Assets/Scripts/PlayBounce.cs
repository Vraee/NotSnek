using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBounce : MonoBehaviour {

    //Please go play bounce. It's a better game. Has been superior to other mobile games since 2002. 
    //God created bounce as his/her own reflection
    //plz
    private Animator anime;
	// Use this for initialization with the bounce
	void Start () {
        anime = GetComponent<Animator>();
        //this it the game
	}

    public void Bounce()
    {
        //this is you playing it
        if(anime != null) {
            anime.Play("MultiplierBounce");
        }
    }
}
