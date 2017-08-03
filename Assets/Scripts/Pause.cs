using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    private GameObject verificationPanel;
    private GameObject gameManager;
    private GameObject pauseBackground;
    private Camera cam;
    
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        pauseBackground = GameObject.Find("PauseMenu");
        pauseBackground.SetActive(false);
        verificationPanel = GameObject.Find("VerificationPanel");
        verificationPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseBackground.activeSelf == true && verificationPanel.activeSelf == false)
            {
                Continue();
            }else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        cam.GetComponent<CameraShake>().SetShake(false);
        pauseBackground.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        cam.GetComponent<CameraShake>().SetShake(true);
        pauseBackground.SetActive(false);
        Time.timeScale = 1;
    }

    public void EnableVerification()
    {
        verificationPanel.SetActive(true);
    }

    public void DisableVerification()
    {
        verificationPanel.SetActive(false);
    }

}
