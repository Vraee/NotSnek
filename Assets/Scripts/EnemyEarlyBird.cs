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

	new void Update(){
		base.Update ();

		if (!attacking) {
			_rend.enabled = false;
		} else {
			_rend.enabled = true;
		}
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

	public override void DisableAttacking ()
	{
		if (attacking) {
			attacking = false;
			retreating = true;
			AttackPlayer (speed, this.gameObject);
			RotateToPlayer ();
		}		
	}
}
