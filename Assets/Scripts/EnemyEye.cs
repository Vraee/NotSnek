using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : EnemyController {
	public float spawnDelay = 0.5f;
    private Animator anime;
    public float blinkDelay = 3;
	public float destroyDelay = 10f;
	//The amount of enemies to be spawned on the same "path"
	public int spawnAmount;
	public float movementAmount;
	public float amplitude;
	public bool destroyAfterTime;
	//Should enemies move in a queue or together as a wall
	public bool moveAsWall;
	public bool switchStartDirection;
	//Defines whether the enemy should move back and forth or not
	public bool moveBackAndForth;
	public bool bouncyMovement;
	//The point where enemy should turn back, not needed if "turnBack" is false
	public float turnPoint1;
	//The point where enemy should turn back to its original direction, not needed if "turnBack" is false
	public float turnPoint2;

	public enum Direction {LeftRight, RightLeft, UpDown, DownUp};
	public Direction direction;

	private float destroyTimer;
	//The amount of spawned enemies so far; couldn't use a static variable, since it would cause problems when there are more than one enemy at the start of the game
	private int spawned = 0;
	private Vector3 originalPos;
	private Vector3 pos;
	private float index;
	private GameObject originalPowerUp;

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
        anime = GetComponent<Animator>();
        InvokeRepeating("Blink", Random.Range(0.1f, 3f), blinkDelay);
        destroyTimer = Time.time + destroyDelay;
		spawned++;

		if (spawned == 1) {
			originalPos = this.transform.position;
		}
			
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



		/*if (turnPoint1 == 0) {
			if (direction == Direction.LeftRight || direction == Direction.RightLeft)
				turnPoint1 = -pos.x;
			else
				turnPoint1 = -pos.y;
		}

		if (turnPoint2 == 0) {
			if (direction == Direction.LeftRight || direction == Direction.RightLeft)
				turnPoint1 = pos.x;
			else
				turnPoint1 = pos.y;
		}*/
        
	}

	new void Update() {
		base.Update ();

		if (Time.time >= destroyTimer && destroyAfterTime)
			Die (this.gameObject.transform.position);
	}

	public override void MoveEnemy() {
		index += Time.deltaTime;
		Vector3 tmpPos = new Vector3 (0, 0, 0);

		if (direction == Direction.LeftRight) {
			pos.x += movementAmount * Time.deltaTime;
			tmpPos.x = pos.x;

			if (!switchStartDirection)
				tmpPos.y = pos.y + Mathf.Sin (index * speed) * amplitude;
			else
				tmpPos.y = pos.y - Mathf.Sin (index * speed) * amplitude;
		} else if (direction == Direction.RightLeft) {
			pos.x -= movementAmount * Time.deltaTime;
			tmpPos.x = pos.x;
			if (!switchStartDirection)
				tmpPos.y = pos.y + Mathf.Sin (index * speed) * amplitude;
			else
				tmpPos.y = pos.y - Mathf.Sin (index * speed) * amplitude;
		} else if (direction == Direction.UpDown) {
			pos.y -= movementAmount * Time.deltaTime;
			tmpPos.y = pos.y;
			if (!switchStartDirection)
				tmpPos.x = pos.x + Mathf.Sin (index * speed) * amplitude;
			else
				tmpPos.x = pos.x - Mathf.Sin (index * speed) * amplitude;
		} else {
			pos.y += movementAmount * Time.deltaTime;
			tmpPos.y = pos.y;
			if (!switchStartDirection)
				tmpPos.x = pos.x + Mathf.Sin (index * speed) * amplitude;
			else
				tmpPos.x = pos.x - Mathf.Sin (index * speed) * amplitude;
		}

		if (bouncyMovement)
		{
			if (direction == Direction.UpDown || direction == Direction.DownUp)
				tmpPos.x = Mathf.Abs(tmpPos.x);
			if (direction == Direction.LeftRight || direction == Direction.RightLeft)
				tmpPos.y = Mathf.Abs(tmpPos.y);

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

    private void Blink()
    {
        anime.Play("Blink");
    }

}
