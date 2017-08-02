using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour {
    private GameObject verificationPanel;
    private GameObject gameManager;
    private GameObject pauseBackground;
    private Camera cam;
    private enum PauseStates
    {
        pause,
        verification,
        none
    }
    private PauseStates current;


    private void Start()
    {
        cam = Camera.main;
        pauseBackground = GameObject.Find("PauseMenu");
        pauseBackground.SetActive(false);
        verificationPanel = GameObject.Find("VerificationPanel");
        verificationPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void ChangeScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
	
    public void QuitApplication()
    {
        Application.Quit();
    }

    public void EnableVerification()
    {
        verificationPanel.SetActive(true);
        current = PauseStates.verification;
    }

    public void DisableVerificationPanel()
    {
        verificationPanel.SetActive(false);
        current = PauseStates.pause;
    }

    public void PauseGame()
    {
        if (current == PauseStates.none)
        {
            Time.timeScale = 0;
            pauseBackground.SetActive(true);
            current = PauseStates.pause;
            cam.GetComponent<CameraShake>().SetShake(false);
        }
        else if (current == PauseStates.pause)
        {
            pauseBackground.SetActive(false);
            Time.timeScale = 1;
            current = PauseStates.none;
            cam.GetComponent<CameraShake>().SetShake(true);
        }
        else if (current == PauseStates.verification)
        {
            current = PauseStates.pause;
            verificationPanel.SetActive(false);
        }
    }
}
