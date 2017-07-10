using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : EnemyController {
	public float spawnDelay = 0.5f;
	//The amount of enemies to be spawned on the same "path"
	public int spawnAmount;
	//Should enemies move in a queue or together as a wall
	public bool moveAsWall;
	public float movementAmount;
	public float amplitude;

	//Defines whether the enemy should move back and forth or not
	public bool moveBackAndForth;
	//The point where enemy should turn back, not needed if "turnBack" is false
	public float turnPoint1;
	//The point where enemy should turn back to its original direction, not needed if "turnBack" is false
	public float turnPoint2;

	public enum Direction {LeftRight, RightLeft, UpDown, DownUp};
	public Direction direction;

	private float spawnTimer;
	//The amount of spawned enemies so far; couldn't use a static variable, since it would cause problems when there are more than one enemy at the start of the game
	private int spawned = 0;
	private Vector3 originalPos;
	private Vector3 pos;
	private float index;

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

		if (moveAsWall)
			pos = transform.position;
		else
			pos = originalPos;

		/*If enemy moves from right to left or down up at the start, flips turning points; 
		this just makes checking when the enemy should turn back later in the code simpler.*/
		if ((direction == Direction.RightLeft || direction == Direction.DownUp) && spawned == 1) {
			float tmpTurnPoint1 = turnPoint1;
			turnPoint1 = turnPoint2;
			turnPoint2 = tmpTurnPoint1;
		}	

		RandomisePowerUp ();
	}

	public override void MoveEnemy() {
		index += Time.deltaTime;
		Vector3 tmpPos = new Vector3 (0, 0, 0);

		if (direction == Direction.LeftRight) {
			pos.x += movementAmount * Time.deltaTime;
			tmpPos.x = pos.x;
			tmpPos.y = pos.y + Mathf.Sin (index * speed) * amplitude;
		} else if (direction == Direction.RightLeft) {
			pos.x -= movementAmount * Time.deltaTime;
			tmpPos.x = pos.x;
			tmpPos.y = pos.y + Mathf.Sin (index * speed) * amplitude;
		} else if (direction == Direction.UpDown) {
			pos.y -= movementAmount * Time.deltaTime;
			tmpPos.y = pos.y;
			tmpPos.x = pos.x +Mathf.Sin (index * speed) * amplitude;
		} else {
			pos.y += movementAmount * Time.deltaTime;
			tmpPos.y = pos.y;
			tmpPos.x = pos.x + Mathf.Sin (index * speed) * amplitude;
		}

		transform.position = tmpPos;

		if (moveBackAndForth)
			CheckTurning();
	}

	private void CheckTurning() {
		if (direction == Direction.LeftRight) {
			if (transform.position.x >= turnPoint1) {
				direction = Direction.RightLeft;
			}
		} else if (direction == Direction.RightLeft) {
			if (transform.position.x <= turnPoint2) {
				direction = Direction.LeftRight;
			}
		} else if (direction == Direction.UpDown) {
			if (transform.position.y <= turnPoint1) {
				direction = Direction.DownUp;
			}
		} else {
			if (transform.position.y >= turnPoint2) {
				direction = Direction.UpDown;
			}
		}
	}

	private void CreateNew() {
		EnemyEye newEye = Instantiate(this) as EnemyEye;
		newEye.SetOriginalPos (this.originalPos);
		newEye.SetSpawned (spawned);
	}

	private void RandomisePowerUp() {
		int random = Random.Range (0, 2);

		if (random == 0)
			powerUpPrefab = null;
	}
}
