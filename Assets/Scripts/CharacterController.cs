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
	private Vector3 destinationPoint;

	private float distance;
	public float minDistance = 0.05f;

	private bool idle = false;
	/*public float unit = 0.1f;
	public float freq = 0.1f;*/

	/*float circleSpeed = 1f;
	float forwardSpeed = -1f; // Assuming negative Z is towards the camera
	float circleSize = 1f;
	float circleGrowSpeed = 0.1f;
	float zPos = 1f;*/




	// Use this for initialization
	void Start () {
		bodyParts = new List<GameObject> ();
		bodyParts.Add (head);
		bodyParts.Add (tail);

		for (int i = 0; i < 100; i++) {
			AddBodyPart ();
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (!(currentMousePosition == destinationPoint)) {
			idle = false;
			destinationPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		} else {
			/*idle = true;
			destinationPoint.x = unit * Mathf.Cos (Time.time * 10 * freq);
			destinationPoint.y = unit * Mathf.Sin (Time.time * 10 * freq);
			unit += Time.deltaTime;
			destinationPoint.z = 0;*/

			/*float xPos = Mathf.Sin(Time.time * circleSpeed) * circleSize;
			float yPos = Mathf.Cos(Time.time * circleSpeed) * circleSize;
			zPos += forwardSpeed * Time.deltaTime;

			circleSize += circleGrowSpeed;

			destinationPoint.x = xPos;
			destinationPoint.y = yPos;
			destinationPoint.z = zPos;*/

		}

		Move ();
	}

	public void Move() {
		Quaternion rotation = Quaternion.LookRotation (head.transform.position - destinationPoint, Vector3.forward);

		head.transform.rotation = rotation;
		head.transform.eulerAngles = new Vector3 (0, 0, head.transform.eulerAngles.z);
		head.GetComponent<Rigidbody2D>().angularVelocity = 0;
		float input = Input.GetAxis ("Vertical");


		if (!idle) {
			//head.GetComponent<Rigidbody2D> ().AddForce (head.transform.up * speed * input);
			head.gameObject.transform.Translate(head.transform.up * speed * input * Time.smoothDeltaTime, Space.World);
		} else {
			//destinationPoint.z = 0;
			//head.GetComponent<Rigidbody2D> ().transform.position = destinationPoint;
		}

		//Vector3 position = new Vector3 (mousePosition.x, mousePosition.y, 0f);
		//head.GetComponent<Rigidbody2D> ().transform.position = position;

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
