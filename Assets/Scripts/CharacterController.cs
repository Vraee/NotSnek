using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {
	public GameObject head;
	public GameObject tail;
	public GameObject bodyPrefab;
	public GameObject legPrefab;
	public float speed;

	private List<GameObject> bodyParts;

	private float distance;//
	public float minDistance = 0.05f;//



	// Use this for initialization
	void Start () {
		bodyParts = new List<GameObject> ();
		bodyParts.Add (head);
		bodyParts.Add (tail);
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		Move ();
	}

	public void Move() {
		var mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Quaternion rotation = Quaternion.LookRotation (head.transform.position - mousePosition, Vector3.forward);

		head.transform.rotation = rotation;
		head.transform.eulerAngles = new Vector3 (0, 0, head.transform.eulerAngles.z);
		head.GetComponent<Rigidbody2D>().angularVelocity = 0;

		head.GetComponent<Rigidbody2D>().AddForce (head.transform.up * speed);

		for (int i = 1; i < bodyParts.Count; i++) {
			distance = Vector3.Distance (bodyParts[i - 1].transform.position, bodyParts[i].transform.position);
			Vector3 newPosition = bodyParts[i - 1].transform.position;
			float T = Time.deltaTime * distance * minDistance * speed;

			if (T > 0.5f)//
				T = 0.5f;//

			bodyParts [i].transform.position = Vector3.Slerp (bodyParts [i].transform.position, newPosition, T);
			bodyParts[i].transform.rotation= Quaternion.Slerp (bodyParts[i].transform.rotation, bodyParts[i - 1].transform.rotation, T);
		}
	}

	public void AddBodyPart() {
		GameObject previousBodyPart = bodyParts[bodyParts.Count - 1];
		GameObject bodyPartType;
		if ((bodyParts.Count) % 5 == 0) {
			bodyPartType = legPrefab;
		} else {
			bodyPartType = bodyPrefab;
		}

		GameObject newBodyPart = Instantiate(bodyPartType, previousBodyPart.transform.position - (previousBodyPart.transform.up / 2), previousBodyPart.transform.rotation) as GameObject;
		newBodyPart.transform.parent = GameObject.Find ("Player").transform;

		GameObject tempTail = bodyParts [bodyParts.Count - 1];
		bodyParts [bodyParts.Count - 1] = newBodyPart;
		bodyParts.Add (tempTail);
	}

	public void RemoveBodyPart(GameObject removablePart) {
		bodyParts.Remove (removablePart);
		Destroy(removablePart);
	}
}
