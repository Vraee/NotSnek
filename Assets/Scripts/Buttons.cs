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


}
