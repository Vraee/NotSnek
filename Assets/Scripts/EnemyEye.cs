using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : EnemyController {
	public float spawnDelay = 0.5f;
	public int spawnAmount;
	public bool wall;
	public float movementAmount;
	public float amplitude;
	/*public float turnPoint1;
	public float turnPoint2;*/

	public enum Direction {LeftRight, RightLeft, UpDown, DownUp};
	public Direction direction;

	private float spawnTimer;
	private int spawned = 0;
	private Vector3 originalPos;
	private Vector3 pos;
	private float index;
	private Vector3 axis;
	private bool turned;

	private int GetSpawned() {
		return spawned;
	}

	private void SetSpawned(int spawned) {
		this.spawned = spawned;
	}

	private Vector3 GetOriginalPos() {
		return originalPos;
	}

	private void SetOriginalPos(Vector3 originalPos) {
		this.originalPos = originalPos;
	}

	// Use this for initialization
	new void Start () {
		base.Start ();
		spawned++;

		if (spawned == 1) {
			originalPos = this.transform.position;
		}

		spawnTimer = Time.time + spawnDelay;

		if (spawned < spawnAmount) 
			Invoke ("CreateNew", spawnDelay);

		if (wall)
			pos = transform.position;
		else
			pos = originalPos;

		turned = false;
	}

	public override void MoveEnemy() {

		if (direction == Direction.LeftRight) {
			pos.x += movementAmount * Time.deltaTime;
			index += Time.deltaTime;
			pos.y = Mathf.Sin (index * speed) * amplitude;
			transform.position = pos;
		} else if (direction == Direction.RightLeft) {
			pos.x -= movementAmount * Time.deltaTime;
			index += Time.deltaTime;
			pos.y = Mathf.Sin (index * speed) * amplitude;
			transform.position = pos;
		} else if (direction == Direction.UpDown) {
			pos.y -= movementAmount * Time.deltaTime;
			index += Time.deltaTime;
			pos.x = Mathf.Sin (index * speed) * amplitude;
			transform.position = pos;
		} else {
			pos.y += movementAmount * Time.deltaTime;
			index += Time.deltaTime;
			pos.x = Mathf.Sin (index * speed) * amplitude;
			transform.position = pos;
		}
	}

	private void CreateNew() {
		EnemyEye newEye = Instantiate(this) as EnemyEye;
		newEye.SetOriginalPos (this.originalPos);
		newEye.SetSpawned (spawned);
	}
}
