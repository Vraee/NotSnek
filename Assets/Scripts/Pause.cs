using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Pause : MonoBehaviour {

    private GameObject pauseBackground;
    private bool paused = false;
    private bool showGUI = false;

	// Use this for initialization
	void Start () {
        pauseBackground = GameObject.Find("PauseMenu");
        pauseBackground.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
	}

    public void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            showGUI = true;
        }
        else
        {
            Time.timeScale = 1;
            showGUI = false;
            
        }

        if (paused)
        {
            pauseBackground.SetActive(true);
        }
        else
        {
            pauseBackground.SetActive(false);
        }
    }
}

