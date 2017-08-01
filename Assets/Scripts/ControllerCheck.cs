using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCheck : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        string[] temp = Input.GetJoystickNames();
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    //Not empty, controller temp[i] is connected
                    Debug.Log("Controller " + i + " is connected using: " + temp[i]);

                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    Debug.Log("Controller: " + i + " is disconnected.");
                }
            }
        }
    }
}
