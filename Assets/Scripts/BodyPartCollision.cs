using UnityEngine;
using System.Collections;

public class BodyPartCollision : MonoBehaviour {

	private CharacterController parentScript;

	void Start () {
		parentScript = GameObject.Find ("Player").GetComponent<CharacterController> ();
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		if (this.gameObject.tag == "Head" && collider.gameObject.tag == "PowerUp")
			parentScript.AddBodyPart ();
		else if (!(this.gameObject.tag == "Head") && !(this.gameObject.tag == "Tail") && collider.gameObject.tag == "Enemy") {
			parentScript.RemoveBodyPart (this.gameObject);
		}
	}
}
