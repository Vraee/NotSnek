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

	//static int ID;

	public int GetDivisionsAmount() {
		return divisionsAmount;
	}

	public void SetDivisionsAmount(int divisionsAmount) {
		this.divisionsAmount = divisionsAmount;
	}

	new void Start() {	
		//ID++;
		base.Start ();
		divisionTimer = Time.time + divisionDelay;
		baseStamina = stamina;
		//Debug.Log ("ID: " + ID + " stamina: " + stamina + " baseStamina " + baseStamina);
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
        {
            //second smallest
            mediumDropRate = 95;
            smallDropRate = 5;
            largeDropRate = 0;
        }
        else
        {
            //smallest
            mediumDropRate = 5;
            smallDropRate = 95;
            largeDropRate = 0;
        }
        StartCoroutine(Scale ());
		EnemyDarkSpirit currentDarkSpirit = this;
		baseStamina = baseStamina / 2;
		stamina = baseStamina;

		EnemyDarkSpirit newDarkSpirit = Instantiate(currentDarkSpirit) as EnemyDarkSpirit;
		newDarkSpirit.StartScaling ();
		newDarkSpirit.SetDivisionsAmount (divisionsAmount);
		newDarkSpirit.transform.parent = this.transform.parent;
		newDarkSpirit.transform.position = transform.position;
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

	public override void Die (Vector3 spawnPos)
	{
		//If all the dark spirits on path have been destroyed, destroys also the DarkSpiritComponents gameobject (and therefore the path)
		if (transform.parent.transform.childCount == 1)
			Destroy (transform.parent.transform.parent.gameObject);
		base.Die (spawnPos);
	}
}
