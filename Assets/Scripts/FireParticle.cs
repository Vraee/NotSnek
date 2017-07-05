using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour {
    
    Vector2 velocity;
    public float timeAlive = 0f;
    public float lifeSpan = 2f;
    public Vector2 MaxVelocity = new Vector2(-0.05f, 0.1f);
    public Vector2 MinVelocity = new Vector2(0.05f, 0.2f);


	// Use this for initialization
	void Start () {
        velocity = new Vector2(Random.Range(MinVelocity.x, MaxVelocity.x), Random.Range(MinVelocity.y, MaxVelocity.y));
        lifeSpan = lifeSpan * Random.Range(0.1f, 1.1f);

    }
	
	// Update is called once per frame
	void Update () {
        timeAlive += Time.deltaTime;
        if(timeAlive >= lifeSpan)
        {
            Destroy(gameObject);
            return;
        }
        this.transform.Translate(velocity * Time.deltaTime);
	}
}
