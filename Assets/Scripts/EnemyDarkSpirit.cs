using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDarkSpirit : EnemyController {
	public float divisionDelay = 5f;
	public GameObject secondPowerUp;
	public GameObject thirdPowerUp;

	private float baseStamina;
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
		baseStamina = stamina;

		if (divisionsAmount == 3)
			RandomisePowerUp ();
	}

	new void Update() {
		base.Update ();
		if (Time.time >= divisionTimer && divisionsAmount < 3 && !divided && gameObject.GetComponent<MoveOnPath>().GetOnPath()) {
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
	}

	public void Divide() {
		if (divisionsAmount >= 0 && divisionsAmount < 3)
			powerUpPrefab = secondPowerUp;
		else
			powerUpPrefab = thirdPowerUp;

		StartCoroutine(Scale ());
		EnemyDarkSpirit currentDarkSpirit = this;
		baseStamina = baseStamina / 2;
		stamina = baseStamina;

		EnemyDarkSpirit newDarkSpirit = Instantiate(currentDarkSpirit) as EnemyDarkSpirit;
		newDarkSpirit.StartScaling ();
		newDarkSpirit.SetDivisionsAmount (divisionsAmount);
		newDarkSpirit.transform.SetParent(this.transform.parent);
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
			gameObject.GetComponent<MoveOnPath> ().speedOnPath += Time.deltaTime;
			yield return null;
		}
	}

	private void RandomisePowerUp() {
		int random = Random.Range (0, 2);

		if (random == 0)
			powerUpPrefab = null;
	}

	public override void Die() {
		//If all the dark spirits on path have been destroyed, destroys also the DarkSpiritComponents gameobject (and therefore the path)
		if (transform.parent.transform.childCount == 1) 
			Destroy (transform.parent.transform.parent.gameObject);

		base.Die ();
	}
}
