using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float multiplier;
    public float gameTime;
    public float timer;
	public float score;
	public Text scoreText;
	public Text multiplierText;
	public Text greetingsText;
	public GameObject[] timeOfDayComponents;
	public GameObject preservableValues;

	[HideInInspector]
	public CharacterController player;
	[HideInInspector]
	public enum TimeOfDay { Morning, Day, Evening, Night };
	[HideInInspector]
	public TimeOfDay timeOfDay;
	[HideInInspector]
	public float visibleAreaWidth;
	[HideInInspector]
	public float visibleAreaHeight;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<CharacterController>();
        multiplier = player.GetBodyPartsAmount() + 1;
		//gameTime = timeToNextSpawn[0];
        greetingsText.enabled = false;
		timeOfDay = TimeOfDay.Morning;

		visibleAreaHeight = Camera.main.orthographicSize * 2;
		visibleAreaWidth = visibleAreaHeight * Screen.width / Screen.height;
		score = preservableValues.GetComponent<PreservableValues>().GetScore();
		timeOfDay = (TimeOfDay)preservableValues.GetComponent<PreservableValues>().GetTimeOfDay();
		UpdateMultiplier();
		UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTime -= Time.deltaTime;
    }

    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(10, 10, 250, 100), niceTime);
    }

    public void IncreaseScore(float amount)
    {
        score += (amount * multiplier);
        UpdateScore();
    }

    public void UpdateMultiplier()
    {
        if(player != null) {
            multiplier = player.GetBodyPartsAmount() + 1;
        }

        multiplierText.text = "x" + multiplier;
    }

    void UpdateScore()
    {
        scoreText.text = "super duper highscore: " + score;
    }

    public void Restart()
    {
        greetingsText.enabled = true;
        StartCoroutine(LoadLevelAfterDelay(3));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
		PreservableValues.SetScore(score);
		PreservableValues.SetTimeOFDay((int)timeOfDay);
		Debug.Log(preservableValues.GetComponent<PreservableValues>().GetScore() + " " + preservableValues.GetComponent<PreservableValues>().GetTimeOfDay());
        yield return new WaitForSeconds(delay);
		DontDestroyOnLoad(preservableValues);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
