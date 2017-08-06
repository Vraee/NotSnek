using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroller : MonoBehaviour {
	public Sprite[] cloudSprites;
	public float speed;
	public float minInterval;
	public float maxInterval;
	public float minScale;
	public float maxScale;
	//Different colors for different times of day
	public Color32[] cloudColors = new Color32[4];

	[HideInInspector]
	public bool moveClouds;

	private List<GameObject> clouds;
	private float visibleAreaWidth;
	private float visibleAreaHeight;
	private float timer;
	private GameManager gameManager;
	private GameManager.TimeOfDay timeOfDay;
	private GameManager.TimeOfDay prevTimeOfDay;
	private int currentColorIndex;

	// Use this for initialization
	void Start () {
		clouds = new List<GameObject>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		visibleAreaHeight = Camera.main.orthographicSize * 2;
		visibleAreaWidth = visibleAreaHeight * Screen.width / Screen.height;
		timer = Time.time + RandomiseSpawnTime();
		moveClouds = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (moveClouds)
		{
			MoveClouds();
			CheckLocation();

			if (Time.time >= timer)
			{
				CreateCloud();
			}
		}

		if (currentColorIndex != (int)gameManager.timeOfDay)
		{
			currentColorIndex = (int)gameManager.timeOfDay;
			StartCoroutine(ChangeColor());
		}
	}

	private void CreateCloud()
	{
		timer += RandomiseSpawnTime();
		GameObject newCloud = new GameObject();
		newCloud.AddComponent<SpriteRenderer>();
		newCloud.GetComponent<SpriteRenderer>().sprite = RandomiseSprite();
		newCloud.GetComponent<SpriteRenderer>().sortingOrder = -501;
		newCloud.GetComponent<SpriteRenderer>().color = cloudColors[currentColorIndex];
		float posX = Random.Range((0f - visibleAreaWidth / 2f), (0f + visibleAreaWidth / 2f));
		float posY = visibleAreaHeight / 2f + 3f;

		newCloud.transform.position = new Vector3(posX, posY, 0f);
		float randomScale = RandomiseScale();
		newCloud.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
		newCloud.GetComponent<SpriteRenderer>().flipX = RandomiseFlip();

		clouds.Add(newCloud);
	}

	private Sprite RandomiseSprite()
	{
		Sprite randomSprite;
		int spriteIndex = Random.Range(0, cloudSprites.Length);
		randomSprite = cloudSprites[spriteIndex];

		return randomSprite;
	}

	private float RandomiseSpawnTime()
	{
		return Random.Range(minInterval, maxInterval);
	}

	private float RandomiseScale()
	{
		return Random.Range(minScale, maxScale);
	}

	private bool RandomiseFlip()
	{
		int random = Random.Range(0, 2);

		if (random == 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void MoveClouds()
	{
		foreach (GameObject cloud in clouds)
		{
			if (cloud != null)
			{
				Vector3 newPos = cloud.transform.position;
				newPos.y -= Time.deltaTime * speed;
				cloud.transform.position = newPos;
			}
		}
	}

	private void CheckLocation()
	{
		foreach (GameObject cloud in clouds)
		{
			if (!CheckInArea(cloud))
			{
				Destroy(cloud);
			}
		}
	}

	public bool CheckInArea(GameObject cloud)
	{
		bool inArea = false;

		if (cloud != null && cloud.transform.position.y >= visibleAreaHeight / 2f * (-1f) - 3f)
		{
			inArea = true;
		}

		return inArea;
	}

	IEnumerator ChangeColor()
	{
		float progress = 0;
		while (progress <= 1)
		{
			foreach (GameObject cloud in clouds)
			{
				if (cloud != null)
				{
					cloud.GetComponent<SpriteRenderer>().color = Color.Lerp(cloudColors[currentColorIndex - 1], cloudColors[currentColorIndex], progress);
				}
			}
			progress += Time.deltaTime;
			yield return null;
		}
	}
}
