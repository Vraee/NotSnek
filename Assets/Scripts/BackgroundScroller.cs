using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed;
	public GameObject enemyHolder;

    private Renderer rend;
	//The offset where the entire texture has been shown
	private float endOffset;
	private float spawnDistance;
	private float nextSpawn;
	private GameManager gameManager;
	private GameObject[] enemyWaves;
	private GameObject[] allTimeOfDayComponents;
	private int id;
	private GameManager.TimeOfDay timeOfDay;
	private float offsetTimer;
	private Vector2 offset;
	private bool bossDead;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
		ScaleToCamera();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		allTimeOfDayComponents = gameManager.timeOfDayComponents;
		enemyWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().enemiesToSpawn;

		spawnDistance = endOffset / enemyWaves.Length;
		nextSpawn = 0;
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		bossDead = false;
    }
	
	// Update is called once per frame
	void Update () {
		Scroll();
		SpawnEnemies();
    }

	private void Scroll()
	{
		offset = new Vector2(0, offsetTimer * scrollSpeed);
		offsetTimer += Time.deltaTime;

		if (offset.y <= endOffset)
		{
			rend.material.mainTextureOffset = offset;
		}

		else
		{
			if (!bossDead && enemyHolder.transform.position.y > -(Camera.main.orthographicSize * 2))
			{
				Vector3 enemiesPos = new Vector3(0, enemyHolder.transform.position.y - scrollSpeed, 0);
				enemyHolder.transform.position = enemiesPos;
			}

			DisableEnemyMovement ();

			if (enemyHolder.transform.position.y <= -(Camera.main.orthographicSize * 2))
			{
				DestroyEnemies();
			}

			if (bossDead)
			{
				//if ((int)timeOfDay < System.Enum.GetValues(typeof(GameManager.TimeOfDay)).Length)
				if ((int)timeOfDay < 2)
				{
					timeOfDay++;
				}
				else
				{
					timeOfDay = 0;
				}

				rend.material.mainTexture = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().backgroundTexture;
				offset.y = 0;
				offsetTimer = 0;
				rend.material.mainTextureOffset = offset;
				ScaleToCamera();
				enemyWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().enemiesToSpawn;
				nextSpawn = 0;
				enemyHolder.transform.position = new Vector3(0, 0, 0);
			}
		}
	}

	private void SpawnEnemies()
	{
		if (offset.y >= nextSpawn && id < enemyWaves.Length)
		{
			GameObject wave;
			wave = Instantiate(enemyWaves[id], new Vector3(0, 0, 0), Quaternion.identity);
			wave.transform.SetParent(enemyHolder.transform);
			id++;
			nextSpawn += spawnDistance;
		}
	}

	private void DisableEnemyMovement() {
		foreach (Transform enemySpawner in enemyHolder.transform)
		{
			foreach (EnemyController enemy in enemySpawner.GetComponentsInChildren<EnemyController>()) {
				enemy.DisableAttacking ();
			}
		}
	}

	private void DestroyEnemies()
	{
		foreach (Transform enemySpawner in enemyHolder.transform)
		{
			Destroy(enemySpawner.gameObject);
		}
	}

	private void ScaleToCamera() {
		Texture background = rend.material.mainTexture;
		float visibleAreaH = Camera.main.orthographicSize * 2;
		float visibleAreaW = visibleAreaH * Screen.width / Screen.height;

		float scaledTexWidth = visibleAreaW;
		float scaledTexHeight = background.height * (scaledTexWidth / background.width);

		transform.localScale = new Vector3 (scaledTexWidth, scaledTexHeight, 1);
		transform.position = new Vector3 (0, scaledTexHeight / 2 - Camera.main.orthographicSize, 1);

		endOffset = 1 / scaledTexHeight * (scaledTexHeight - Camera.main.orthographicSize * 2);
	}
}
