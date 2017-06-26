﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public bool shoot;
    public float damage;
    public GameObject explosion;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("Ya'll need some jeesus");
        damage = GameObject.Find("Player").GetComponent<CharacterController>().fireballDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot) {
            gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);
        }

        if (gameObject.transform.position.x > 20 || gameObject.transform.position.y > 20 || gameObject.transform.position.x < -20 || gameObject.transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }

    /*public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Enemy")
        {
            GameObject expl = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            expl.GetComponent<Explosion>().ChangeParameters(gameObject.transform);
            //Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }*/

	//This is called in the enemy's OnTriggerEnter2D; Destroy() is called in the enemy's OnTriggerExit2D
	public void Explode() {
		GameObject expl = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
		expl.GetComponent<Explosion>().ChangeParameters(gameObject.transform);
	}

    public void IncreaseDamage(float damage)
    {
        this.damage = this.damage + damage;
    }

    public void Shoot()
    {
        shoot = true;
    }


}
