using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed;
	public GameObject enemyHolder;
	public GameObject cloudScroller;
    public GameObject endText;

    private Renderer rend;
	//The offset where the entire texture has been shown
	private float endOffset;
	private float eyeSpawnDist;
	private float nextEyeSpawn;
	private float otherEnemySpawnDist;
	private float nextOtherEnemySpawn;
	private float bonusEnemySpawnDist;
	private float nextBonusEnemySpawn;
    private GameObject canvas;
	private GameManager gameManager;
	private GameObject[] eyeWaves;
	private GameObject[] otherEnemyWaves;
	private GameObject[] bonusEnemyWaves;
	private GameObject[] allTimeOfDayComponents;
	private int eyeId;
	private int otherEnemyId;
	private int bonusEnemyId;
	private GameManager.TimeOfDay timeOfDay;
	private float offsetTimer;
	private Vector2 offset;
    private bool bossAppear = false;
	private bool bossDead;
    private GameObject boss;
	private float scaledTexWidth;
	private float scaledTexHeight;
	private bool endGame;

	public bool GetBossDead() {
		return bossDead;
	}

	public void SetBossDead(bool bossDead) {
		this.bossDead = bossDead;
	}

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        canvas = GameObject.Find("Canvas");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		timeOfDay = gameManager.timeOfDay;
		allTimeOfDayComponents = gameManager.timeOfDayComponents;
		SetTimeOfDayValues();

		bossDead = false;
		endGame = false;
    }
	
	// Update is called once per frame
	void Update () {
		Scroll();
		SpawnEnemies();

		//FOR DEBUGGING. PLZ REMOVE PLZ.
		if (Input.GetKeyDown(KeyCode.N))
			MoveToNext();
    }

	private void Scroll()
	{
		offset = new Vector2(0, offsetTimer * scrollSpeed);
		offsetTimer += Time.deltaTime;

		if (offset.y <= endOffset)
		{
			rend.material.mainTextureOffset = offset;
		}

		else if (!bossAppear && !endGame)
		{
            bossAppear = true;
            Instantiate(boss);
			cloudScroller.GetComponent<CloudScroller>().moveClouds = false;
		}

		if (bossAppear)
		{
			DisableEnemyMovement ();

			if (enemyHolder.transform.position.y > -(Camera.main.orthographicSize * 2)) {
				MoveEnemiesOffscreen ();
			}

			if (bossDead)
			{
				bossAppear = false;
				bossDead = false;
				if ((int)timeOfDay < System.Enum.GetValues(typeof(GameManager.TimeOfDay)).Length - 1)
				{
					MoveToNext();
				}
				else
				{
					timeOfDay = 0;
					EndGame();
				}
			}
		}

		/*if (bossDead && bossAppear)
		{
			bossAppear = false;
			bossDead = false;
			if ((int)timeOfDay < System.Enum.GetValues(typeof(GameManager.TimeOfDay)).Length - 1)
			{
				MoveToNext();
			}
			else
			{
				timeOfDay = 0;
                EndGame();
			}
		}*/
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

		if (offset.y >= nextBonusEnemySpawn && bonusEnemyId < bonusEnemyWaves.Length)
		{
			GameObject wave = Instantiate(bonusEnemyWaves[bonusEnemyId], new Vector3(0, 0, 0), Quaternion.identity);
			wave.transform.SetParent(enemyHolder.transform);
			bonusEnemyId++;
			nextBonusEnemySpawn += bonusEnemySpawnDist;

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

	private void MoveEnemiesOffscreen() {
		Vector3 enemiesPos = new Vector3(0, enemyHolder.transform.position.y - scrollSpeed * 10f, 0);
		enemyHolder.transform.position = enemiesPos;

		if (enemyHolder.transform.position.y <= -(Camera.main.orthographicSize * 2))
		{
			DestroyEnemies();
		}
	}

	private void DestroyEnemies()
	{
		foreach (Transform enemySpawner in enemyHolder.transform)
		{
			Destroy(enemySpawner.gameObject);
		}
	}

	private void SetTimeOfDayValues()
	{
		cloudScroller.GetComponent<CloudScroller>().moveClouds = true;
		gameManager.SetScoreAtEndOfPhase (gameManager.GetScore());
		rend.material.mainTexture = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().backgroundTexture;
		offset.y = 0;
		offsetTimer = 0;
		rend.material.mainTextureOffset = offset;
		ScaleToCamera();

		enemyHolder.transform.position = new Vector3(0, 0, 0);
		DestroyEnemies ();
        boss = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().boss;
		eyeWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().eyesToSpawn;
		otherEnemyWaves = allTimeOfDayComponents[(int)timeOfDay].GetComponent<TimeOfDayComponents>().timeSpecificEnemiesToSpawn;
		bonusEnemyWaves = allTimeOfDayComponents [(int)timeOfDay].GetComponent<TimeOfDayComponents> ().bonusEnemiesToSpawn;

		if (allTimeOfDayComponents [(int)timeOfDay].GetComponent<TimeOfDayComponents> ().immediatelySpawnEyes) {
			eyeSpawnDist = endOffset / eyeWaves.Length;
			nextEyeSpawn = 0;
		} else {
			eyeSpawnDist = (1 / scaledTexHeight * (scaledTexHeight - Camera.main.orthographicSize * 4) / eyeWaves.Length);
			nextEyeSpawn = eyeSpawnDist;
		}

		if (allTimeOfDayComponents [(int)timeOfDay].GetComponent<TimeOfDayComponents> ().immediatelySpawnSpecificEnemies) {
			otherEnemySpawnDist = endOffset / otherEnemyWaves.Length;
			nextOtherEnemySpawn = 0;
		} else {
			otherEnemySpawnDist = (1 / scaledTexHeight * (scaledTexHeight - Camera.main.orthographicSize * 4) / otherEnemyWaves.Length);
			nextOtherEnemySpawn = otherEnemySpawnDist;
		}

		if (allTimeOfDayComponents [(int)timeOfDay].GetComponent<TimeOfDayComponents> ().immediatelySpawnBonusEnemies) {
			bonusEnemySpawnDist = endOffset / bonusEnemyWaves.Length;
			nextBonusEnemySpawn = 0;
		} else {
			bonusEnemySpawnDist = (1 / scaledTexHeight * (scaledTexHeight - Camera.main.orthographicSize * 4) / bonusEnemyWaves.Length);
			nextBonusEnemySpawn = bonusEnemySpawnDist;
		}

		eyeId = 0;
		otherEnemyId = 0;
		bonusEnemyId = 0;
    }

	private void ScaleToCamera() {
		Texture background = rend.material.mainTexture;

		scaledTexWidth = gameManager.visibleAreaWidth;
		scaledTexHeight = background.height * (scaledTexWidth / background.width);

		transform.localScale = new Vector3 (scaledTexWidth, scaledTexHeight, 1);
		transform.position = new Vector3 (0, scaledTexHeight / 2 - Camera.main.orthographicSize, 1);

		endOffset = 1 / scaledTexHeight * (scaledTexHeight - Camera.main.orthographicSize * 2);
	}

    private void EndGame()
    {
		cloudScroller.GetComponent<CloudScroller>().moveClouds = true;
		endGame = true;
        float score = GameObject.Find("GameManager").GetComponent<GameManager>().GetScore();
        canvas.GetComponent<EndTextController>().Instantiate();
        Invoke("ChangeScene", 20);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(0);
    }
	private void MoveToNext()
	{
		timeOfDay++;
		SetTimeOfDayValues();
		gameManager.timeOfDay = timeOfDay;
	}
}
