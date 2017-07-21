using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpawnWithinArea : MonoBehaviour {
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		CheckSpawnPos ();
	}
	
	// Update is called once per frame
	public void CheckSpawnPos() {
		bool inAreaX = false;
		bool inAreaY = false;

		if (transform.position.x >= gameManager.visibleAreaWidth / 2f * (-1f) && transform.position.x <= gameManager.visibleAreaWidth / 2f) {
			inAreaX = true;
		}

		if (transform.position.y >= gameManager.visibleAreaHeight / 2f * (-1f) && transform.position.y <= gameManager.visibleAreaHeight / 2f) {
			inAreaY = true;
		}

		Debug.Log ("inX: " + inAreaX + " inY: " + inAreaY);
	}
}
