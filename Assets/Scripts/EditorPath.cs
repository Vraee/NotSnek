using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPath : MonoBehaviour {
    //For visualizing the path in scene mode
    public Color rayColor = Color.white;
    //List of positions to move between
    public List<Transform> pathObjects = new List<Transform>();
    public Transform[] transformArray;
	public int listLenght;
    
	void Start() {
		CreatePathObjectList ();
	}

	void Update() {
	}

	public void CreatePathObjectList() {
		transformArray = GetComponentsInChildren<Transform>();
		pathObjects.Clear();
		foreach (Transform pathObject in transformArray) {
			if (pathObject != this.transform) {
				pathObjects.Add (pathObject);
			}
		}
		listLenght = pathObjects.Count;
	}

    private void OnDrawGizmos()
    {		
		Debug.Log ("..........");
        Gizmos.color = rayColor;
        for(int i = 0; i < pathObjects.Count; i++)
        {
            Vector3 position = pathObjects[i].position;

            if(i > 0)
            {
                Vector3 previous = pathObjects[i - 1].position;
                Gizmos.DrawLine(position, previous);
                Gizmos.DrawSphere(position, 0.05f);
            }
        }
    }
}
