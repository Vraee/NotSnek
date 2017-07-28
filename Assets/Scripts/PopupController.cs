using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour {
    public PopupText popupText;
    //private static GameObject canvas;

    public void Initialize()
    {
        //canvas = GameObject.Find("Canvas");
    }


    public void CreateFloathingText(string text, Transform location)
    {
        PopupText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(this.transform , false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}