using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public float scrollSpeed;

    private Renderer rend;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
		//ScaleToCamera();
		SetToPixelPerfect();
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 offset = new Vector2(0, Time.time * scrollSpeed);

        rend.material.mainTextureOffset = offset;
		//SetToPixelPerfect();
    }
    private void ScaleToCamera()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        transform.localScale = new Vector3(width, height, 1);
        transform.position = new Vector3(0, 0, 1);
    }

	private void SetToPixelPerfect() {
		Texture texture = rend.material.mainTexture;
		float scale = (Screen.height / 2.0f) / Camera.main.orthographicSize;
		float newX = texture.width / scale;
		float newY = texture.height / scale;
		transform.localScale = new Vector3(newX, newY, 1);
		transform.position = new Vector3 (0, newY / 2 - Camera.main.orthographicSize, 1);
	}
}
