using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	public int collectibleValue = 2;
	private GameObject target;
    public float fallSpeed;
    public float dragSpeed;
    public float acceleration;
    private bool pulling;

	void Start()
	{
		target = GameObject.Find ("Head");
	}

    void Update()
    {
        if (!pulling) {
            Fall();
        }
        else
        {
            PullTowards();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Head")
        {
            pulling = true;
        }
    }

    private void Fall()
    {
        transform.Translate(Vector3.down * Time.deltaTime * fallSpeed);
    }
    

    private void PullTowards()
    {
        float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
        

        if (distance >= 0.25)
        {
            dragSpeed = dragSpeed + acceleration * Time.deltaTime;
            float step = dragSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, step);
        }

    }
}
