using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGryphon : EnemyController {
	public Sprite normal;
	public Sprite stone;
	public float attackDelay = 2;
	public float retreatSpeed;
	private Animator anime;

	new void Start()
	{
		base.Start();
		anime = GetComponent<Animator>();
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

		if (gameObject.GetComponent<MoveOnPath>().GetOnPath()) {
			SetVulnerable (false);
			anime.SetBool("flyNormal", false);
			anime.SetBool("flyStone", true);
		} else {
			SetVulnerable (true);
			anime.SetBool("flyNormal", true);
			anime.SetBool("flyStone", false);
		}

		if (GetAttacking() && !anime.GetBool("attacking"))
		{
			anime.SetBool("attacking", true);
		}

		if (GetRetreating() && anime.GetBool("attacking"))
		{
			anime.SetBool("attacking", false);
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

	public override void Die (Transform deadTransform)
	{
		Destroy (transform.parent.gameObject);
		base.Die (deadTransform);
	}
}
