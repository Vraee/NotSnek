﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour {

    //Script used to access pathPoints
    public EditorPath pathToFollow;
    //Starting point on the path
    public int currentWayPointID = 0;
    public float speed;
    //how far from the next point should the enemy stop
    public float reachDistance = 0.0f;
    public float rotationSpeed = 5.0f;
    //true to slerp between points
    public bool Slerp;
	//Defines whether the object moves in a loop or back and forth
	public bool looping;
	public bool startClockwise;

    private Vector3 lastPosition;
    private Vector3 currentPosition;
	private bool goingBack;
	private int startIndex;
	private int endIndex;
	private bool pathReached;

	public bool GetPathReached() {
		return pathReached;
	}

	// Use this for initialization
	void Start () {
        lastPosition = transform.position;
		goingBack = false;

		if (startClockwise) {
			endIndex = 0;
			goingBack = true;
		} else {
			startIndex = 0;
		}

		currentWayPointID = startIndex;
		pathReached = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (pathToFollow.pathObjects.Count > 0) {
			if (currentWayPointID != startIndex && pathReached == false) {
				pathReached = true;
			}

			if (startClockwise) {
				if (pathToFollow.GetComponent<EditorPath> ().listLenght - 1 != startIndex) {
					startIndex = pathToFollow.GetComponent<EditorPath> ().listLenght - 1;
					currentWayPointID = startIndex;
				}
			} else {
				if (pathToFollow.GetComponent<EditorPath> ().listLenght - 1 != endIndex) {
					endIndex = pathToFollow.GetComponent<EditorPath> ().listLenght - 1;
				}
			}

			//Gets the distance between pathObject (in EditorPath script) and current gameobject position
			float distance = Vector3.Distance (pathToFollow.pathObjects [currentWayPointID].position, transform.position);
			//move to the next waypoint
			transform.position = Vector3.MoveTowards (transform.position, pathToFollow.pathObjects [currentWayPointID].position, Time.deltaTime * speed);

			//rotate towards the next waypoint
			var rotation = Quaternion.LookRotation (pathToFollow.pathObjects [currentWayPointID].position, transform.position);
			rotation.y = 0;
			rotation.x = 0;
			if (Slerp) {
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
			} else {
				transform.rotation = Quaternion.Lerp (transform.rotation, rotation, rotationSpeed * Time.deltaTime);
			}

			if (distance <= reachDistance) {
				if (!goingBack)
					currentWayPointID++;
				else {
					currentWayPointID--;
				}
			}

			if (looping && currentWayPointID == endIndex) {
				currentWayPointID = startIndex;
			} else if (!looping && distance <= reachDistance && (currentWayPointID == endIndex || currentWayPointID == startIndex)) {
				goingBack = !goingBack;
			}
		}
	}

    //Destroys the object :)
    void KillMe()
    {
        Destroy(gameObject);
    }
}
