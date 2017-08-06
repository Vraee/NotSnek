using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCheck1 : MonoBehaviour {
    private bool ControllerInput = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool GetControllerInput()
    {
        return ControllerInput;
    }

    public void SetControllerInput(bool input)
    {
        ControllerInput = input;
    }
    
}
