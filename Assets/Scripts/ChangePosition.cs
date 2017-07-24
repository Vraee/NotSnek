using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePosition : MonoBehaviour {
    public enum Sides
    {
        left,
        right
    }
    public Sides StartSide;
    private float cameraWidth;
    private float cameraHeight;

    // Use this for initialization
    void Start () {
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * Screen.width / Screen.height;
        ChangePos();
    }

    private void ChangePos()
    {
        if(StartSide == Sides.left)
        {
            gameObject.transform.position = new Vector2(-cameraWidth / 4 , transform.position.y);
        }else if (StartSide == Sides.right)
        {
            gameObject.transform.position = new Vector2(cameraWidth / 4, transform.position.y);
        }
    }
}
