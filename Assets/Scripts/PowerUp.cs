using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	public int collectibleValue = 1;

	//Probably should eventually move these to game controller class. Also create that class
	private int powerUpLimit = 5;
	private int collectibleSum;

	// Use this for initialization
	void Start () {
		
	}


	public int GetPowerUpLimit() {
		return powerUpLimit;
	}

	public void SetPowerUpLimit(int newPowerUpLimit) {
		powerUpLimit = newPowerUpLimit;
	}

	public int GetCollectibleSum() {
		Debug.Log ("GetCollectibleSum: " + collectibleSum);
		return collectibleSum;
	}

	public void SetCollectibleSum(int newCollectibleSum) {
		Debug.Log ("SetCollectibleSum: " + collectibleSum);
		collectibleSum = newCollectibleSum;
	}
}
