using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDarkSpirit : EnemyController {
	public float divisionDelay = 5f;

	private float baseStamina;
	private float baseDamageOutput;
	private float baseScore;
	private float divisionTimer;
	private bool divided = false;
	private int divisionsAmount = 0;
	private int startWayPointID;

	public int GetDivisionsAmount() {
		return divisionsAmount;
	}

	public void SetDivisionsAmount(int divisionsAmount) {
		this.divisionsAmount = divisionsAmount;
	}

	public int GetStartWayPointID() {
		return startWayPointID;
	}

	public void SetStartWayPointID(int startWayPointID) {
		this.startWayPointID = startWayPointID;
	}

	new void Start() {	
		base.Start ();
		divisionTimer = Time.time + divisionDelay;
		baseStamina = stamina;
		baseDamageOutput = damageOutput;
		baseScore = score;
		//Debug.Log ("stamina: " + stamina + " damage: " + damageOutput + " score: " + score);
		//gameObject.GetComponent<MoveOnPath> ().SetStartIndex(startWayPointID);
		//Debug.Log ("start: " + gameObject.GetComponent<MoveOnPath> ().currentWayPointID );
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

		maxPowerUpAmount = 3;
        StartCoroutine(Scale ());
		EnemyDarkSpirit currentDarkSpirit = this;
		baseStamina = baseStamina / 2;
		stamina = baseStamina;
		baseDamageOutput = baseDamageOutput / 2;
		damageOutput = baseDamageOutput;
		baseScore = baseScore / 2;
		score = baseScore;

		/*if (gameObject.GetComponent<MoveOnPath> ().pathToFollow.pathObjects.Count > this.gameObject.GetComponent<MoveOnPath> ().currentWayPointID + 2) {
			startWayPointID = this.gameObject.GetComponent<MoveOnPath> ().currentWayPointID + 2;
		} else {
			startWayPointID = this.gameObject.GetComponent<MoveOnPath> ().currentWayPointID - 2;
		}*/

		//Debug.Log (startWayPointID);

		EnemyDarkSpirit newDarkSpirit = Instantiate(currentDarkSpirit) as EnemyDarkSpirit;
		newDarkSpirit.StartScaling ();
		newDarkSpirit.SetDivisionsAmount (divisionsAmount);
		newDarkSpirit.transform.parent = this.transform.parent;
		newDarkSpirit.transform.position = transform.position;
		//newDarkSpirit.SetStartWayPointID (startWayPointID);
	}

	public void StartScaling() {
		StartCoroutine(Scale ());
	}

	IEnumerator Scale()
	{
		Vector3 startSize = transform.localScale;
		Vector3 targetSize = transform.localScale * 0.75f;
		float progress = 0;

		while (progress <= 1)
		{
			transform.localScale = Vector3.Lerp(startSize, targetSize, progress);
			progress += Time.deltaTime;
			gameObject.GetComponent<MoveOnPath> ().speedOnPath += Time.deltaTime;
			yield return null;
		}
	}

	public override void Die (Transform deadTransform)
	{
		//If all the dark spirits on path have been destroyed, destroys also the DarkSpiritComponents gameobject (and therefore the path)
		if (transform.parent.transform.childCount == 1)
			Destroy (transform.parent.transform.parent.gameObject);
		base.Die (deadTransform);
	}
}
