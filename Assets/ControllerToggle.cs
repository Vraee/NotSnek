using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ControllerToggle : MonoBehaviour {
    private bool controllerCheck;

	// Use this for initialization
	void Start () {
        controllerCheck = GameObject.Find("ControllerInformation").GetComponent<ControllerCheck1>().GetControllerInput();
        if (controllerCheck)
        {
            gameObject.GetComponentInChildren<Toggle>().isOn = true;
        }
        else
        {
            gameObject.GetComponentInChildren<Toggle>().isOn = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponentInChildren<Toggle>().isOn)
        {
            GameObject.Find("ControllerInformation").GetComponent<ControllerCheck1>().SetControllerInput(true);
        }
        else
        {
            GameObject.Find("ControllerInformation").GetComponent<ControllerCheck1>().SetControllerInput(false);
        }
    }
}
