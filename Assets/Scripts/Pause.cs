using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject pauseBackground;
    private bool pause = false;
    private bool showGUI = false;

	// Use this for initialization
	void Start () {
        
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
        pause = !pause;
        if (pause)
        {
            Time.timeScale = 0;
            showGUI = true;
        }
        else
        {
            Time.timeScale = 1;
            showGUI = false;
            
        }

        if (pause)
        {
            pauseBackground.SetActive(true);
        }
        else
        {
            pauseBackground.SetActive(false);
        }
    }
}

