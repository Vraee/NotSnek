using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEarlyBird : EnemyController {

	TrailRenderer _rend;
	public float attackDelay = 2;
	public float retreatSpeed;

	new void Start(){
		base.Start ();
		_rend = GetComponent<TrailRenderer> ();
	}

	public override void MoveEnemy() {
		if (!GetAttacking()) {
			RotateToPlayer ();
		}

		if (Time.time >= GetTimer()) {
			SetMoving(true);
		}

		if (GetMoving() && gameObject.GetComponent<MoveOnPath>().GetPathReached()) {
			AttackPlayer (speed, this.gameObject);
		}
	}

	new void Update(){
		base.Update ();

		if (!attacking) {
			//Debug.Log ("moikkelis");
			_rend.enabled = false;
		} else {
			_rend.enabled = true;
			//Debug.Log ("hellurei");
		}


	}
}
