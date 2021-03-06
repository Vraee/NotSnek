﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public float amplitude;
    public bool shoot;
    public float damage;
    public GameObject explosion;
	public GameObject player;
	public float damageIncrease;
	public bool ownMovingMethod = false;

	private ParticleSystem particlesystem;
    private Camera cam;
    private float viewHeight;
    private float viewWidth;
	private Vector3 startPos;

    // Use this for initialization
    void Start()
    {
		//Debug.Log("Ya'll need some jeesus");
		//damage = GameObject.Find("Player").GetComponent<CharacterController>().fireballDamage;
		particlesystem = GetComponent<ParticleSystem>();
        cam = Camera.main;
        viewHeight = 2f * cam.orthographicSize;
        viewWidth = viewHeight * cam.aspect;
		startPos = transform.position;
        var rotation = this.transform.rotation;
        rotation.z += 45f;
    }

    // Update is called once per frame
    void Update()
    {
		CheckLocation ();

		if (!ownMovingMethod)
        	gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);

            //lifeTime += Time.deltaTime;
           // gameObject.transform.localPosition = new Vector2(transform.localPosition.x + Mathf.Sin(lifeTime) * amplitude * Time.deltaTime, transform.localPosition.y);
            //gameObject.transform.Translate(gameObject.transform.right * Mathf.Sin(lifeTime) * amplitude * Time.deltaTime, Space.Self);
    }

	//This is called in the enemy's OnTriggerEnter2D; Destroy() is called in the enemy's OnTriggerExit2D
	public void Explode() {
		GameObject expl = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
		expl.GetComponent<Explosion>().ChangeParameters(gameObject.transform);
	}

    public void IncreaseDamage(float damage)
    {
        this.damage = this.damage + damage;
    }

    private void CheckLocation()
    {
		if (!CheckInArea() && (Mathf.Abs (startPos.x - transform.position.x) > viewWidth || Mathf.Abs (startPos.y - transform.position.y) > viewHeight)) {
			RemoveFireball ();
		}
    }

	public bool CheckInArea() {
		bool inArea = false;

		if (transform.position.x >= viewWidth / 2f * (-1f) && transform.position.x <= viewWidth / 2f 
			&& transform.position.y >= viewHeight / 2f * (-1f) && transform.position.y <= viewHeight / 2f) {
			inArea = true;
		}

		return inArea;
	}

    public void Shoot()
    {
        shoot = true;
	}

	public void RemoveFireball()
	{
		if (particlesystem != null) {
			var emission = particlesystem.emission;
			emission.enabled = false;
		} 
		Invoke("Destroy", 1f);

	}
	private void Destroy()
	{
		Destroy(gameObject);
	}


}
