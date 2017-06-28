using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDarkSpirit : EnemyController {
	public float divisionDelay = 5f;
	private float baseStamina;
	private float divisionTimer;
	private bool divided = false;
	private int divisionsAmount = 0;

	static int ID;

	public int GetDivisionsAmount() {
		return divisionsAmount;
	}

	public void SetDivisionsAmount(int divisionsAmount) {
		this.divisionsAmount = divisionsAmount;
	}

	new void Start() {	
		ID++;
		base.Start ();
		divisionTimer = Time.time + divisionDelay;
		baseStamina = stamina;
		//Debug.Log ("ID: " + ID + " stamina: " + stamina + " baseStamina " + baseStamina);
	}

	new void Update() {
		base.Update ();
		if (Time.time >= divisionTimer && divisionsAmount < 3 && !divided) {
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
		StartCoroutine(Scale ());
		EnemyDarkSpirit currentDarkSpirit = this;
		baseStamina = baseStamina / 2;
		stamina = baseStamina;

		EnemyDarkSpirit newDarkSpirit = Instantiate(currentDarkSpirit) as EnemyDarkSpirit;
		newDarkSpirit.StartScaling ();
		newDarkSpirit.SetDivisionsAmount (divisionsAmount);
	}

	public void StartScaling() {
		StartCoroutine(Scale ());
	}

	IEnumerator Scale()
	{
		Vector3 startSize = transform.localScale;
		Vector3 targetSize = transform.localScale / 2;
		float progress = 0;

		while (progress <= 1)
		{
			transform.localScale = Vector3.Lerp(startSize, targetSize, progress);
			progress += Time.deltaTime;
			yield return null;
		}
	}
}
