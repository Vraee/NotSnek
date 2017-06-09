using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	private CharacterController parentScript;
	private int bodyPartHP;
    public float timeScale;

	void Start () {
		parentScript = GameObject.Find ("Player").GetComponent<CharacterController> ();
		bodyPartHP = parentScript.HPPerBodypart;
        //Attempt at couroutine :D
        StartCoroutine(Scale());
    }

	public void OnTriggerEnter2D(Collider2D collider) {
		if (this.gameObject.tag == "Head" && collider.gameObject.tag == "PowerUp") {
			parentScript.AddBodyPart ();
            Destroy(collider.gameObject);
        }
        else if (!(this.gameObject.tag == "Head") && !(this.gameObject.tag == "Tail") && collider.gameObject.tag == "Enemy") {
			int enemyDamage = 1;
			bodyPartHP -= enemyDamage;
			parentScript.SetHP(parentScript.GetHP() - enemyDamage);

			if (bodyPartHP <= 0) {
				parentScript.RemoveBodyPart (this.gameObject);
			}
		}
	}

    IEnumerator Scale()
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0.1f,0.1f,0), new Vector3(1f, 1f, 0), progress);
            progress += Time.deltaTime;
            yield return null;
        }

    }
}
