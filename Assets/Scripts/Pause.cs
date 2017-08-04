using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Pause : MonoBehaviour {

    private GameObject verificationPanel;
    private GameObject gameManager;
    private GameObject pauseBackground;
    private Camera cam;
    private bool paused;
    
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        pauseBackground = GameObject.Find("PauseMenu");
        pauseBackground.SetActive(false);
        verificationPanel = GameObject.Find("VerificationPanel");
        verificationPanel.SetActive(false);
    }
    public bool GetPause()
    {
        return paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (pauseBackground.activeSelf == false)
            {
                PauseGame();
                HighlightFirstButton();
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (pauseBackground.activeSelf == true && verificationPanel.activeSelf == false)
            {
                Continue();
            }
            else if (pauseBackground.activeSelf == true && verificationPanel.activeSelf == true)
            {
                DisableVerification();
            }
        }
    }

    public void PauseGame()
    {
        paused = true;
        cam.GetComponent<CameraShake>().SetShake(false);
        pauseBackground.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        Invoke("SetPauseToFalse", 0.2f);
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
    private void SetPauseToFalse()
    {
        paused = false;
    }

    private void HighlightFirstButton()
    {
        EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(es.firstSelectedGameObject);
    }
}
