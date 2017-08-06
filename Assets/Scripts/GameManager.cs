using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public Text scoreText;
	public Text multiplierText;
	public Text greetingsText;
	public GameObject[] timeOfDayComponents;
	public Font font;

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

	private float multiplier;
	private float gameTime;
	private float timer;
	private float score;
	private float scoreAtEndOfPhase;
	private GUIStyle timerStyle;
	private string niceTime;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<CharacterController>();
        multiplier = player.GetBodyPartsAmount() + 1;
		//gameTime = timeToNextSpawn[0];
        greetingsText.enabled = false;
		timeOfDay = TimeOfDay.Morning;

		visibleAreaHeight = Camera.main.orthographicSize * 2;
		visibleAreaWidth = visibleAreaHeight * Screen.width / Screen.height;

		score = PreservableValues.GetScore ();
		timeOfDay = (TimeOfDay)PreservableValues.GetTimeOfDay ();
		timer = PreservableValues.GetTimer ();

		UpdateScore();
        UpdateMultiplier(true);

		timerStyle = new GUIStyle();
		timerStyle.font = font;
		timerStyle.normal.textColor = new Color32(122, 125, 154, 180);
    }

	public float GetScore() {
		return score;
	}

	public void SetScore(float score) {
		this.score = score;
	}

	public float GetScoreAtEndOfPhase() {
		return scoreAtEndOfPhase;
	}

	public void SetScoreAtEndOfPhase(float scoreAtEndOfPhase) {
		this.scoreAtEndOfPhase = scoreAtEndOfPhase;
	}

	public string GetNiceTime()
	{
		return niceTime;
	}

	public void SetNiceTime(string niceTime)
	{
		this.niceTime = niceTime;
	}

    public float GetMultiplier()
    {
        return multiplier;
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
        niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        GUI.Label(new Rect(10, 10, 250, 100), niceTime, timerStyle);
    }

    public void IncreaseScore(float amount)
    {
        score += (amount * multiplier);
        UpdateScore();
    }

    public void UpdateMultiplier(bool increase)
    {
        if(player != null) {
            if(player.GetBodyPartsAmount() > 0) {
                multiplier = player.GetBodyPartsAmount();
            }
            else
            {
                multiplier = 1;
            }
        }
        multiplierText.text = "x" + multiplier;
        if (increase) { 
            multiplierText.GetComponent<PlayBounce>().Bounce();
        }
        else
        {
            multiplierText.GetComponent<PlayShake>().Shake();
        }
    }
    
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void Restart()
    {
		StartCoroutine(CountDown());
        StartCoroutine(LoadLevelAfterDelay(3));
    }

	IEnumerator CountDown()
	{
		greetingsText.enabled = true;
		int seconds = 3;

		while (seconds > 0)
		{
			greetingsText.text = "Restarting in " + seconds;
			seconds--;
			yield return new WaitForSeconds(1f);
		}

	}

    IEnumerator LoadLevelAfterDelay(float delay)
    {
		PreservableValues.SetScore(scoreAtEndOfPhase);
		PreservableValues.SetTimeOFDay((int)timeOfDay);
		greetingsText.enabled = true;

		while (delay > 0)
		{
			greetingsText.text = "Restarting in " + delay;
			delay--;
			yield return new WaitForSeconds(1f);
		}

		PreservableValues.SetTimer (timer);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
