using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {
	private CharacterController parentScript;
    private int listIndex;
	private float bodyPartHP;
	private bool isHit;
	private GameObject enemy;

	void Start () {
		parentScript = GameObject.Find ("Player").GetComponent<CharacterController> ();
		bodyPartHP = parentScript.GetBodyPartHP ();
		isHit = false;
		StartCoroutine (Scale ());
	}

    public int GetListIndex()
    {
        return listIndex;
    }

    public void SetListIndex(int newListIndex) {
        listIndex = newListIndex;
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
			int newCollectibleSum = parentScript.GetCollectibleSum () + collider.gameObject.GetComponent<PowerUp> ().collectibleValue;
			int powerUpLimit = parentScript.GetPowerUpLimit ();
			parentScript.SetCollectibleSum(newCollectibleSum);

			if (newCollectibleSum >= powerUpLimit) {
				int resetCollectibleSum = parentScript.GetCollectibleSum () - parentScript.GetPowerUpLimit ();
				parentScript.SetCollectibleSum (resetCollectibleSum);
				parentScript.AddBodyPart ();
			}
            Destroy(collider.gameObject);

        } else if (collider.gameObject.tag == "Enemy") {
			if (!(gameObject.tag == "Fire")) {
				parentScript.StartCoroutine ("EnemyDamage", this);
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
