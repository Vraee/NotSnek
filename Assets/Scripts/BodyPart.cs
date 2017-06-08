using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {
	public int bodyPartHP = 10;

	private CharacterController parentScript;

	void Start () {
		parentScript = GameObject.Find ("Player").GetComponent<CharacterController> ();
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		if (this.gameObject.tag == "Head" && collider.gameObject.tag == "PowerUp") {
			collider.gameObject.GetComponent<PowerUp> ().SetCollectibleSum(collider.gameObject.GetComponent<PowerUp> ().GetCollectibleSum() + collider.gameObject.GetComponent<PowerUp> ().collectibleValue);

			if (collider.gameObject.GetComponent<PowerUp> ().GetCollectibleSum () >= collider.gameObject.GetComponent<PowerUp> ().GetPowerUpLimit ()) {
				collider.gameObject.GetComponent<PowerUp> ().SetCollectibleSum (collider.gameObject.GetComponent<PowerUp> ().GetCollectibleSum() - collider.gameObject.GetComponent<PowerUp> ().GetPowerUpLimit());
				parentScript.AddBodyPart ();
			}
		} else if (!(this.gameObject.tag == "Head") && !(this.gameObject.tag == "Tail") && collider.gameObject.tag == "Enemy") {
			int enemyDamage = 1;
			bodyPartHP -= enemyDamage;
			parentScript.SetHP(parentScript.GetHP() - enemyDamage);

			if (bodyPartHP <= 0) {
				parentScript.RemoveBodyPart (this.gameObject);
			}
		}
	}

	public int GetBodyPartHP()  {
		return bodyPartHP;
	}
}
