﻿using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {
	private CharacterController parentScript;
    private int listIndex;
	private bool isHit;
    private bool takeDamage;
	private GameObject enemy;

	void Start () {
		parentScript = GameObject.Find ("Player").GetComponent<CharacterController> ();
		isHit = false;
		StartCoroutine (Scale ());
	}

	public bool GetIsHit () {
		return isHit;
	}

	public void SetIsHit (bool newIsHit) {
		isHit = newIsHit;
	}

	public GameObject GetEnemy () {
		return enemy;
	}

	public void SetEnemy(GameObject newEnemy) {
		enemy = newEnemy;
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		if (this.gameObject.tag == "Head" && collider.gameObject.tag == "PowerUp") {
			collider.gameObject.GetComponent<PowerUp> ().PickUp (parentScript);
            Destroy(collider.gameObject);

		} else if (this.gameObject.tag == "Head" && collider.gameObject.tag == "Enemy") {
			/*Checks if the hit object is a part of skeleton or other enemy; if it's skeleton, 
			the parent object is used instead, since it has the damage output value*/
			if (collider.gameObject.GetComponent<SkeletonPart> () as SkeletonPart == null) {
				enemy = collider.gameObject;
			} else {
				enemy = collider.transform.parent.gameObject;
			}
			parentScript.StartCoroutine ("EnemyDamage", this);
            
		} else if (this.gameObject.tag == "Head" && collider.gameObject.tag == "EnemyFireball")
        {
            float damage = collider.gameObject.GetComponent<Fireball>().damage;
            parentScript.StartCoroutine("EnemyFireballDamage", damage);

			if (collider.gameObject.GetComponent<Fireball> ().explosion != null) {
				collider.gameObject.GetComponent<Fireball> ().Explode ();
				Destroy (collider.gameObject);
			}
        } else if (this.gameObject.tag == "Head" && collider.gameObject.tag == "Phoenix")
        {
            enemy = collider.gameObject;
            takeDamage = true;
            StartCoroutine(DealDamageToSelf());
        }
	}

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (this.gameObject.tag == "Head" && collider.gameObject.tag == "Phoenix")
        {
            takeDamage = false;
        }
    }

    IEnumerator DealDamageToSelf()
    {
        float progress = 0;
        while (takeDamage)
        {
            progress += Time.deltaTime;
            if (progress >= 0.5)
            {
                parentScript.StartCoroutine("EnemyDamage", this);
                progress = 0;
            }
            yield return null;
        }
    }
		

    IEnumerator Scale()
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0), new Vector3(1f, 1f, 0), progress);
            progress += Time.deltaTime;
            yield return null;
        }
    }
}
