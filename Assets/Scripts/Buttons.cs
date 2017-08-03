using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Buttons : MonoBehaviour {

    public void ChangeScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ContinueButton()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<Pause>().Continue();
    }

    public void ExitButton()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<Pause>().EnableVerification();
    }

    public void VerifyYes()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<Pause>().DisableVerification();
        gameManager.GetComponent<Pause>().Continue();
        ChangeScene(0);
    }

    public void VerifyNo()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<Pause>().DisableVerification();
    }


}
