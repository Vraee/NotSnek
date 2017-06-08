using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {
	public GameObject head;
	public GameObject tail;
	public GameObject bodyPrefab;
	public GameObject legPrefab;
	public GameObject fire;
	public ParticleSystem fireParticles;
	//Needed only if translate is used to move the character; not in use currently
	//public float speed;
	public float bodyPartSpeed; 
	public float minDistance;
	public float bodyPartDistance;
	public int HPPerBodypart;

	private List<GameObject> bodyParts;
	private Vector3 destinationPoint;
	private float distance;
	private int tailLength;
	private int HP;

	// Use this for initialization
	void Start () {
		bodyParts = new List<GameObject> ();
		bodyParts.Add (head);
		bodyParts.Add (tail);

		fireParticles = head.GetComponent<ParticleSystem> ();
		fireParticles.Stop ();
		fire.SetActive (false);

		for (int i = 0; i < 10; i++) {
			AddBodyPart ();
		}
	}

	void Update() {
		destinationPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		RotateToMouse ();
		Move ();

		if (Input.GetMouseButtonDown (0)) {
			Fire ();
		}
		if (Input.GetMouseButtonUp (0)) {
			StopFire ();
		}

		Debug.Log (HP);
	}

	public int GetHP() {
		return HP;
	}

	public void SetHP(int newHP) {
		HP = newHP;
	}

	private void RotateToMouse()
	{
		Quaternion rotation = Quaternion.LookRotation (head.transform.position - destinationPoint, Vector3.forward);
		head.transform.rotation = rotation;
		head.transform.eulerAngles = new Vector3 (0, 0, head.transform.eulerAngles.z);
	}
		
	public void Move() {
		if (Input.GetMouseButton (1) || Input.GetKey(KeyCode.W)) {
			//Get the Screen positions of the object
			Vector3 _target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//revert z to 0
			_target.z = head.transform.position.z;
			//distance between mouse and player
			var mouseDistance = Vector3.Distance (head.transform.position, _target);
			//MoveThisPieceOfTrash if distance is long enough
			if (mouseDistance > 0.1f) {
				//head.transform.Translate (head.transform.up * speed * Time.deltaTime, Space.World);
				head.transform.position = Vector3.Lerp (head.transform.position, _target, 0.1f);
			}
		}
				
		for (int i = 1; i < bodyParts.Count; i++) {
			distance = Vector3.Distance (bodyParts [i - 1].transform.position, bodyParts [i].transform.position);
			Vector3 newPosition = bodyParts [i - 1].transform.position;
			float T = Time.deltaTime * distance * minDistance * bodyPartSpeed;

			if (T > 0.5f) {
				T = 0.5f;
			}

			if (distance > bodyPartDistance) {
				bodyParts [i].transform.position = Vector3.Lerp (bodyParts [i].transform.position, newPosition, T);	
				bodyParts [i].transform.rotation = Quaternion.Lerp (bodyParts [i].transform.rotation, bodyParts [i - 1].transform.rotation, T);
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

		tailLength++;
		HP += HPPerBodypart;
	}

	public void RemoveBodyPart(GameObject removablePart) {
		bodyParts.Remove (removablePart);
		Destroy(removablePart);

		tailLength--;
	}

	public void Fire() {
		fire.SetActive(true);

		if (!fireParticles.isPlaying)
		fireParticles.Play ();
		//Invoke("StopFire", 0.4f);
	}

	public void StopFire() {
		fireParticles.Stop ();
		fire.SetActive(false);
	}

}
