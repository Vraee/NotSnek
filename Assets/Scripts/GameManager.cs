﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float[] timeToNextSpawn;
    public GameObject[] spawnObjects;
    public int id;
    public float gameTime;

    public float timer;

    public float score;
    public Text scoreText;
    

	// Use this for initialization
	void Start () {
        gameTime = timeToNextSpawn[0];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        gameTime -= Time.deltaTime;
        if(gameTime <= 0 && id != spawnObjects.Length )
        {
            Instantiate(spawnObjects[id], new Vector3(0,0,0), Quaternion.identity);
            gameTime = timeToNextSpawn[id];
            id++;


        }
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
        score += amount;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "super duper highscore: " + score;
    }

}
