using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public float scrollSpeed;

    private Renderer rend;
	private Texture background;
	//The offset where the entire texture has been shown
	private float endOffset;
	private float spawnDistance;
	private float nextSpawn;
	private GameManager gameManager;
	public GameObject timeOfDayComponents;
	public GameObject timeOfDayComponents2;
	private GameObject[] enemyWaves;
	private int id;
	private GameManager.TimeOfDay timeOfDay;
	private float offsetTimer;
	private Texture nextTex;

	Vector2 offset;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
		ScaleToCamera();
		enemyWaves = timeOfDayComponents.GetComponent<EnemyWaveSpawner> ().enemiesToSpawn;
		spawnDistance = endOffset / enemyWaves.Length;
		nextSpawn = 0;
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		nextTex = timeOfDayComponents2.GetComponent<EnemyWaveSpawner> ().backgroundTexture;
    }
	
	// Update is called once per frame
	void Update () {
		offset = new Vector2(0, offsetTimer * scrollSpeed);
		offsetTimer += Time.deltaTime;

		if (offset.y <= endOffset) {
			rend.material.mainTextureOffset = offset;
		} else {
			timeOfDay = GameManager.TimeOfDay.Day;
			rend.material.mainTexture = nextTex;
			offset.y = 0;
			offsetTimer = 0;
			rend.material.mainTextureOffset = offset;
			ScaleToCamera ();
			id = 0;
		}

		if (offset.y >= nextSpawn) {
			Instantiate(enemyWaves[id], new Vector3(0,0,0), Quaternion.identity);
			id++;
			nextSpawn += spawnDistance;
		}
    }

	private void ScaleToCamera() {
		background = rend.material.mainTexture;
		float visibleAreaH = Camera.main.orthographicSize * 2;
		float visibleAreaW = visibleAreaH * Screen.width / Screen.height;

		float scaledTexWidth = visibleAreaW;
		float scaledTexHeight = background.height * (scaledTexWidth / background.width);

		transform.localScale = new Vector3 (scaledTexWidth, scaledTexHeight, 1);
		transform.position = new Vector3 (0, scaledTexHeight / 2 - Camera.main.orthographicSize, 1);

		endOffset = 1 / scaledTexHeight * (scaledTexHeight - Camera.main.orthographicSize * 2);
	}
}
