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

    private Camera cam;
    private float viewHeight;
    private float viewWidth;

    private void Awake()
    {
        StartCoroutine(MoveAway());
    }

    void Start()
	{
        cam = Camera.main;
        viewHeight = 2f * cam.orthographicSize;
        viewWidth = viewHeight * cam.aspect;
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
        CheckLocation();
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

	public void PickUp(CharacterController player) {
		int newCollectibleSum = player.GetCollectibleSum () + collectibleValue;
		int powerUpLimit = player.GetPowerUpLimit ();
		player.SetCollectibleSum(newCollectibleSum);

		while (player.GetCollectibleSum() >= powerUpLimit) {
			int resetCollectibleSum = player.GetCollectibleSum () - player.GetPowerUpLimit ();
			player.SetCollectibleSum (resetCollectibleSum);
			player.AddBodyPart ();
            
		}
	}

    private void CheckLocation()
    {
        if (gameObject.transform.position.y >= (viewHeight / 2) + transform.localScale.y)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.y <= (-viewHeight / 2) - transform.localScale.y)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.x >= (viewWidth / 2) + transform.localScale.x)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.x <= (-viewWidth / 2) - transform.localScale.x)
        {
            Destroy(gameObject);
        }

    }
    private IEnumerator MoveAway()
    {
        // save it current rotation
        Quaternion rotation = transform.rotation;
        // Spin it around a random amount
        transform.Rotate(Vector3.forward, Random.Range(0, 360));
        //time to move
        float endTime = Time.time + 0.5f;

        while (Time.time < endTime)
        {
            transform.Translate(Vector3.up * 0.5f * Time.deltaTime);
            yield return null;
        }
        // put it back to its original rotation 
        transform.rotation = rotation;
    }
}
