using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDarkSpirit : EnemyController {
	public float divisionDelay = 5f;
	private float divisionTimer;
	private bool divided = false;
	private int divisionsAmount = 0;

	public int GetDivisionsAmount() {
		return divisionsAmount;
	}

	public void SetDivisionsAmount(int divisionsAmount) {
		this.divisionsAmount = divisionsAmount;
	}

	new void Start() {
		base.Start ();
		divisionTimer = Time.time + divisionDelay;
	}

	public void Update() {
		if (Time.time >= divisionTimer && divisionsAmount < 2 && !divided) {
			divisionsAmount++;
			Divide ();
			divided = true;
			divisionTimer = Time.time + divisionDelay;
		}

		if (Time.time >= divisionTimer && divisionsAmount < 3 && divided) {
			divided = !divided;
		}
	}


	public override void MoveEnemy() {
		//SSSOOOOON
	}

	public void Divide() {
		this.transform.localScale = this.transform.localScale / 2;
		EnemyDarkSpirit currentDarkSpirit = this;

		EnemyDarkSpirit newDarkSpirit = Instantiate(currentDarkSpirit) as EnemyDarkSpirit;
		newDarkSpirit.SetDivisionsAmount (divisionsAmount);
	}
}
