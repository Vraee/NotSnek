using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoenixExplosion : MonoBehaviour {
	public int explosionsAmount;
	//Instantiating these prefabs is randomised
	public GameObject[] explosionPrefabs;
	public float delay;
	
	[HideInInspector]
	public float phoenixWidth;
	[HideInInspector]
	public float phoenixHeight;
	[HideInInspector]
	public Vector3 phoenixPosition;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Explosion>().invokeTime = (float)explosionsAmount * delay;
		StartCoroutine(Explosions());
	}

	public IEnumerator Explosions()
	{
		int i = 0;
		while (i < explosionsAmount)
		{
			GameObject death = Instantiate(RandomiseExplosion(), RandomisePosition(), transform.rotation);
			i++;
			yield return new WaitForSeconds(delay);
		}
	}

	private GameObject RandomiseExplosion()
	{
		GameObject randomExplosion;
		int explosionIndex = Random.Range(0, explosionPrefabs.Length);
		randomExplosion = explosionPrefabs[explosionIndex];

		return randomExplosion;
	}

	private Vector3 RandomisePosition()
	{
		float x = Random.RandomRange(phoenixPosition.x - phoenixWidth / 2f, phoenixPosition.x + phoenixWidth / 2f);
		float y = Random.RandomRange(phoenixPosition.y - phoenixHeight / 2f, phoenixPosition.y + phoenixHeight / 2f);

		return new Vector3(x, y, 0);
	}
}
