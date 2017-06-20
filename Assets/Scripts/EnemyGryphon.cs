﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGryphon : EnemyController {
	public Sprite normal;
	public Sprite stone;
	private bool vulnerable;
	
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

		Debug.Log (gameObject.GetComponent<MoveOnPath>().GetOnPath());
	}
}
