using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour {


    public float floatSpan = 0.25f;
    public float speed = 1.0f;
    public bool yFloat;
    public bool xFloat;
    private float startY;
    private float startX;

    // Use this for initialization
    void Start()
    {
        startY = transform.position.y;
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempPos = transform.position;
        if (yFloat)
        {
            tempPos.y = startY + Mathf.Sin(Time.time * speed) * floatSpan / 2.0f;
        }
        if (xFloat)
        {
            tempPos.x = startX + Mathf.Sin(Time.time * speed) * floatSpan / 2.0f;
        }
        transform.position = tempPos;
    }
}
