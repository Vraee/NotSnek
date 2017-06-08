using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	private CharacterController parentScript;
	private int bodyPartHP;

	void Start () {
		parentScript = GameObject.Find ("Player").GetComponent<CharacterController> ();
		bodyPartHP = parentScript.HPPerBodypart;
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		if (this.gameObject.tag == "Head" && collider.gameObject.tag == "PowerUp")
			parentScript.AddBodyPart ();
		else if (!(this.gameObject.tag == "Head") && !(this.gameObject.tag == "Tail") && collider.gameObject.tag == "Enemy") {
			int enemyDamage = 1;
			bodyPartHP -= enemyDamage;
			parentScript.SetHP(parentScript.GetHP() - enemyDamage);

			if (bodyPartHP <= 0) {
				parentScript.RemoveBodyPart (this.gameObject);
			}
		}
	}
}
