﻿using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {
	private CharacterController parentScript;
	private int bodyPartHP;

	void Start () {
		parentScript = GameObject.Find ("Player").GetComponent<CharacterController> ();
		bodyPartHP = parentScript.GetBodyPartHP ();
		StartCoroutine (Scale ());
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		if (this.gameObject.tag == "Head" && collider.gameObject.tag == "PowerUp") {
			int newCollectibleSum = parentScript.GetCollectibleSum () + collider.gameObject.GetComponent<PowerUp> ().collectibleValue;
			int powerUpLimit = parentScript.GetPowerUpLimit ();
			parentScript.SetCollectibleSum(newCollectibleSum);

			if (newCollectibleSum >= powerUpLimit) {
				int resetCollectibleSum = parentScript.GetCollectibleSum () - parentScript.GetPowerUpLimit ();
				parentScript.SetCollectibleSum (resetCollectibleSum);
				parentScript.AddBodyPart ();
			}

            Destroy(collider.gameObject);
        } else if (!(this.gameObject.tag == "Head") && !(this.gameObject.tag == "Tail") && collider.gameObject.tag == "Enemy") {
			int enemyDamage = 1;
			//bodyPartHP -= enemyDamage;
			parentScript.SetHP(parentScript.GetHP() - enemyDamage);

			if (parentScript.GetHP() <= parentScript.GetComparableHP()) {
				parentScript.SetComparableHP(parentScript.GetComparableHP() - bodyPartHP);
				parentScript.RemoveBodyPart (parentScript.GetTailLength());
			}
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
