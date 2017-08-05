using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpawnWithinArea : MonoBehaviour {
	public enum SpawnSide {Left, Right, Up, Down};
	public SpawnSide spawnSide;

	private bool inAreaX;
	private bool inAreaY;
	private Renderer rend;
	private float visibleAreaWidth;
	private float visibleAreaHeight;

	// Use this for initialization
	void Start () {
		visibleAreaHeight = Camera.main.orthographicSize * 2;
		visibleAreaWidth = visibleAreaHeight * Screen.width / Screen.height;

		//Checks that divided dark spirits' spawn position aren't moved offscreen
		if (gameObject.GetComponent<EnemyDarkSpirit>() == null || (gameObject.GetComponent<EnemyDarkSpirit>() != null && gameObject.GetComponent<EnemyDarkSpirit>().GetDivisionsAmount() == 0))
		{
			CheckSpawnPos();
		}

		if (inAreaX) {
			if (spawnSide == SpawnSide.Right) {
				MoveRight ();
			}

			if (spawnSide == SpawnSide.Left) {
				MoveLeft ();
			}
		}

		if (inAreaY) {
			if (spawnSide == SpawnSide.Up) {
				MoveUp ();
			}

			if (spawnSide == SpawnSide.Down) {
				MoveDown ();
			}
		}

	}
	
	private void CheckSpawnPos() {
		inAreaX = false;
		inAreaY = false;

		if (transform.position.x >= visibleAreaWidth / 2f * (-1f) && transform.position.x <= visibleAreaWidth / 2f) {
			inAreaX = true;
		}

		if (transform.position.y >= visibleAreaHeight / 2f * (-1f) && transform.position.y <= visibleAreaHeight / 2f) {
			inAreaY = true;
		}
	}

	private void MoveLeft() {
		Vector3 newPos = transform.position;
		newPos.x = -(visibleAreaWidth / 2);
		transform.position = newPos;
	}

	private void MoveRight() {
		Vector3 newPos = transform.position;
		newPos.x = + visibleAreaWidth / 2;
		transform.position = newPos;
	}

	private void MoveUp() {
		Vector3 newPos = transform.position;
		newPos.y = visibleAreaHeight / 2;
		transform.position = newPos;
	}

	private void MoveDown() {
		Vector3 newPos = transform.position;
		newPos.y = -(visibleAreaHeight) / 2;
		transform.position = newPos;
	}
}
