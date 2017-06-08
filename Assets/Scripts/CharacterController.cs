using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {
	public GameObject head;
	public GameObject tail;
	public GameObject bodyPrefab;
	public GameObject legPrefab;
	public float speed;
	public float bodyPartSpeed;

	private List<GameObject> bodyParts;
	private Vector3 destinationPoint;

	private float distance;
	public float minDistance = 0.05f;

	// Use this for initialization
	void Start () {
		bodyParts = new List<GameObject> ();
		bodyParts.Add (head);
		bodyParts.Add (tail);

		/*for (int i = 0; i < 500; i++) {
			AddBodyPart ();
		}*/
	}

	void Update() {
		Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		destinationPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		RotateToMouse ();
		Move ();
	}

	private void RotateToMouse()
	{
		Quaternion rotation = Quaternion.LookRotation (head.transform.position - destinationPoint, Vector3.forward);

		head.transform.rotation = rotation;
		head.transform.eulerAngles = new Vector3 (0, 0, head.transform.eulerAngles.z);
	}
		
	public void Move() {
		if (Input.GetMouseButton (1)) {
			//Get the Screen positions of the object
			Vector3 _target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//revert z to 0
			_target.z = head.transform.position.z;
			//distance between mouse and player
			var mouseDistance = Vector3.Distance (head.transform.position, _target);
			//Debug.Log(mouseDistance);
			//MoveThisPieceOfTrash if distance is long enough
			if (mouseDistance > 1f) {
				// BodyParts[0].position = Vector3.MoveTowards(transform.position, _target, Time.smoothDeltaTime * speed);
				//BodyParts[0].Translate(BodyParts[0].position = Vector3.MoveTowards(transform.position, _target, Time.smoothDeltaTime * speed));
				head.transform.Translate (head.transform.up * speed * Time.deltaTime, Space.World);
				//transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
			}

			for (int i = 1; i < bodyParts.Count; i++) {
				distance = Vector3.Distance (bodyParts [i - 1].transform.position, bodyParts [i].transform.position);
				Vector3 newPosition = bodyParts [i - 1].transform.position;
				float T = Time.deltaTime * distance * minDistance * bodyPartSpeed;

				if (T > 0.5f)//
				T = 0.5f;//

				bodyParts [i].transform.position = Vector3.Slerp (bodyParts [i].transform.position, newPosition, T);
				bodyParts [i].transform.rotation = Quaternion.Slerp (bodyParts [i].transform.rotation, bodyParts [i - 1].transform.rotation, T);
			}
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
