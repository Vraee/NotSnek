using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public float scrollSpeed;
    private Renderer rend;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        ScaleToCamera();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 offset = new Vector2(0, Time.time * scrollSpeed);

        rend.material.mainTextureOffset = offset;
    }
    private void ScaleToCamera()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        Debug.Log(height);
        Debug.Log(width);
        transform.localScale = new Vector3(width, height, 1);
        transform.position = new Vector3(0, 0, 1);
    }
}
