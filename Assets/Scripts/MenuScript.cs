using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public void ChangeScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
	
    public void QuitApplication()
    {
        Application.Quit();
    }
}
