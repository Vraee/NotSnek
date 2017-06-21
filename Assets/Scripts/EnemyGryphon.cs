using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGryphon : EnemyController {
	public Sprite normal;
	public Sprite stone;

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

		if (gameObject.GetComponent<MoveOnPath>().GetOnPath()) {
			SetVulnerable (false);
			ChangeSprite (stone);
		} else {
			SetVulnerable (true);
			ChangeSprite (normal);
		}
	}

	public void ChangeSprite(Sprite newSprite) {
		if (newSprite != gameObject.GetComponent<SpriteRenderer> ().sprite) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = newSprite;
		}
	}
}
