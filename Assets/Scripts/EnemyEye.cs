using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : EnemyController {
	public float frequency = 2f;  // Speed of sine movement
	public float magnitude = 2f;   // Size of sine movement
	private Vector3 axis;
	private Vector3 pos;

	float amplitudeX = -10.0f;
	float amplitudeY = 5.0f;
	float omegaX = 1f;
	float omegaY = 5f;
	float index;

	private float spawnTimer;
	public float spawnDelay = 0.5f;
	public int spawnAmount;

	static int spawned = 0;
	static Vector3 originalPos;


	// Use this for initialization
	new void Start () {
		base.Start ();
		spawned++;

		if (spawned == 1) {
			originalPos = this.transform.position;
			pos = transform.position;

		}

		spawnTimer = Time.time + spawnDelay;

		if (spawned < spawnAmount) 
			Invoke ("CreateNew", spawnDelay);

		pos = transform.position;
		//DestroyObject(gameObject, 5.0f);
		axis = transform.up;  // May or may not be the axis you want
	}

	/*new void Update() {
		if (spawnAmount < 3 && Time.time >= spawnTimer) {
			Debug.Log (spawnTimer);
			EnemyEye newEye = Instantiate(this) as EnemyEye;
			newEye.transform.position = originalPos;
			Debug.Log (spawnTimer);
			spawnTimer = Time.time + spawnDelay;
			Debug.Log (spawnTimer);
		}

		base.Update ();
	}*/

	public override void MoveEnemy() {
		/*pos += transform.right * Time.deltaTime * speed;
		transform.position = pos + axis * Mathf.Sin (Time.time * frequency) * magnitude;*/

		index += Time.deltaTime;
		float x = amplitudeX * Mathf.Cos (omegaX * index);
		float y = amplitudeY * Mathf.Sin (omegaY * index);
		//Debug.Log ("x: " + x + " y: " + y + " amplitudeX: " + amplitudeX + " amplitudeY: " + amplitudeY + " omegaX: " + omegaX + " omegaY " + omegaY + " index: " + index);
		transform.localPosition = new Vector3(x, y, 0);
	}

	private void CreateNew() {
		EnemyEye newEye = Instantiate(this) as EnemyEye;
		newEye.transform.position = originalPos;
	}
}
