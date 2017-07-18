using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float invokeTime = 1;

	// Use this for initialization
	void Start () {
        Invoke("Die", invokeTime);
        Camera.main.GetComponent<CameraShake>().Shake(0.5f, 0.4f);
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
