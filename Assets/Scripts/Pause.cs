using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject verificationPanel;
    public GameObject verificationSelected;
    public GameObject pauseBackground;
    public GameObject pauseSelected;
    
    private Camera cam;
    private bool paused;
    
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(pauseSelected, null);
        pauseBackground.SetActive(false);
        verificationPanel.SetActive(false);
    }
    public bool GetPause()
    {
        return paused;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") )
        {
            if (pauseBackground.activeSelf == false)
            {
                if(verificationPanel.activeSelf == true)
                {
                    DisableVerification();
                }
                else
                {
                    PauseGame();
                }
            }else if(pauseBackground.activeSelf == true)
            {
                Continue();
            }else if(verificationPanel.activeSelf == true)
            {
                DisableVerification();

            }

        }
        else if (Input.GetButtonDown("Cancel"))
        {
            if (pauseBackground.activeSelf == true)
            {
                Continue();

            }
            else if (verificationPanel.activeSelf == true)
            {
               DisableVerification();
            }
        }
    }

    public void PauseGame()
    {
        pauseBackground.SetActive(true);
        cam.GetComponent<CameraShake>().SetShake(false);
        Time.timeScale = 0;
        paused = true;
    }

    public void Continue()
    {
        pauseBackground.SetActive(false);
        cam.GetComponent<CameraShake>().SetShake(true);
        Invoke("SetPauseToFalse", 0.2f);
        Time.timeScale = 1;
    }


    public void ExitGame()
    {
        pauseBackground.SetActive(false);
        verificationPanel.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(verificationSelected, null);
    }

    public void EnableVerification()
    {
        //
    }

    public void DisableVerification()
    {
        verificationPanel.SetActive(false);
        pauseBackground.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(pauseSelected, null);
    }

    public void SetPauseToFalse()
    {
        paused = false;
    }
    


}
