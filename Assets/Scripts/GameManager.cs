using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private float gameTime;
    public Text scoreText;
    private float score;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        gameTime += Time.deltaTime;
        
        //Debug.Log(gameTime);
    }

    public void IncreaseScore(float amount)
    {
        score += amount;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "super duper highscore: " + score;
    }
}
