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
	private float eyeSpawnDist;
	private float nextEyeSpawn;
	private float otherEnemySpawnDist;
	private float nextOtherEnemySpawn;
	private GameManager gameManager;
	private GameObject[] eyeWaves;
	private GameObject[] otherEnemyWaves;
	private GameObject[] allTimeOfDayComponents;
	private int eyeId;
	private int otherEnemyId;
	private GameManager.TimeOfDay timeOfDay;
	private float offsetTimer;
	private Vector2 offset;
    private bool bossAppear = false;
	private bool bossDead;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		ScaleToCamera();
		allTimeOfDayComponents = gameManager.timeOfDayComponents;
		eyeWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().eyesToSpawn;
		otherEnemyWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().timeSpecificEnemiesToSpawn;

		eyeSpawnDist = endOffset / eyeWaves.Length;
		otherEnemySpawnDist = endOffset / otherEnemyWaves.Length;
		nextEyeSpawn = 0;
		nextOtherEnemySpawn = 0;

		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		bossDead = true;
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
            bossAppear = true;
			if (bossAppear && enemyHolder.transform.position.y > -(Camera.main.orthographicSize * 2))
			{
				Vector3 enemiesPos = new Vector3(0, enemyHolder.transform.position.y - scrollSpeed * 10f, 0);
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
				eyeWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().eyesToSpawn;
				otherEnemyWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().timeSpecificEnemiesToSpawn;

				nextEyeSpawn = 0;
				nextOtherEnemySpawn = 0;
				eyeId = 0;
				otherEnemyId = 0;

				enemyHolder.transform.position = new Vector3(0, 0, 0);
			}
		}
	}

	private void SpawnEnemies()
	{
		if (offset.y >= nextEyeSpawn && eyeId < eyeWaves.Length)
		{
			GameObject wave = Instantiate(eyeWaves[eyeId], new Vector3(0, 0, 0), Quaternion.identity);
			wave.transform.SetParent(enemyHolder.transform);
			eyeId++;
			nextEyeSpawn += eyeSpawnDist;

			if (allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().increaseEyeSpeed)
			{
				foreach (EnemyController enemy in wave.GetComponentsInChildren<EnemyController>()) {
					enemy.speed += enemy.speed * offset.y;
				}
			}
		}

		if (offset.y >= nextOtherEnemySpawn && otherEnemyId < otherEnemyWaves.Length)
		{
			GameObject wave = Instantiate(otherEnemyWaves[otherEnemyId], new Vector3(0, 0, 0), Quaternion.identity);
			wave.transform.SetParent(enemyHolder.transform);
			otherEnemyId++;
			nextOtherEnemySpawn += otherEnemySpawnDist;

			if (allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().increaseSpecificEnemySpeed)
			{
				foreach (EnemyController enemy in wave.GetComponentsInChildren<EnemyController>()) {
					enemy.speed += enemy.speed * offset.y;
				}
			}
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

		float scaledTexWidth = gameManager.visibleAreaWidth;
		float scaledTexHeight = background.height * (scaledTexWidth / background.width);

		transform.localScale = new Vector3 (scaledTexWidth, scaledTexHeight, 1);
		transform.position = new Vector3 (0, scaledTexHeight / 2 - Camera.main.orthographicSize, 1);

		endOffset = 1 / scaledTexHeight * (scaledTexHeight - Camera.main.orthographicSize * 2);
	}
}
