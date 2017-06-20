﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Die", 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeParameters(Transform trans)
    {
        gameObject.transform.localScale = trans.localScale;
        gameObject.transform.rotation = trans.rotation;

    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
