using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEarlyBird : EnemyController {
	public Sprite normalSprite;
	public Sprite attackSprite;
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
			ChangeSprite (attackSprite);

			if (!attacking)
				ChangeSprite (normalSprite);
		}
	}

	public void ChangeSprite(Sprite newSprite) {
		if (newSprite != gameObject.GetComponent<SpriteRenderer> ().sprite) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = newSprite;
		}
	}

	public override void DisableAttacking ()
	{
		if (attacking) {
			ChangeSprite (normalSprite);
			attacking = false;
			retreating = true;
			AttackPlayer (speed, this.gameObject);
			RotateToPlayer ();
		}		
	}

	public override void Die (Transform deadTransform)
	{
		Destroy (transform.parent.gameObject);
		base.Die (deadTransform);
	}
}
