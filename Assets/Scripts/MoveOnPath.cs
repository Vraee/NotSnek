using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour {

    //Script used to access pathPoints
    public EditorPath pathToFollow;
    //Starting point on the path
    public int currentWayPointID = 0;
    public float speedOnPath;
    //how far from the next point should the enemy stop
    public float reachDistance = 0.0f;
    public float rotationSpeed = 5.0f;
    //true to slerp between points
    public bool Slerp;
    //Defines whether the object moves in a loop or back and forth
    public bool Rotation = true;
	public bool looping;
	public bool startClockwise;

    private Vector3 currentPosition;
	private bool goingBack;
	private int startIndex = 0;
	private int endIndex;
	private bool onPath;
	private bool pathReached;
	private float distance;
	private Vector3 destinationPoint;

	public bool GetOnPath() {
		return onPath;
	}

	public void SetOnPath(bool onPath) {
		this.onPath = onPath;
	}

	public bool GetPathReached() {
		return pathReached;
	}

	public int GetStartIndex () {
		return startIndex;
	}

	public void SetStartIndex(int startIndex) {
		this.startIndex = startIndex;
	}

	// Use this for initialization
	void Start () {
		goingBack = false;

		if (startClockwise) {
			endIndex = 0;
			goingBack = true;
		} /*else {
			startIndex = 0;
		}*/

		currentWayPointID = startIndex;
		onPath = false;
		pathReached = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (pathToFollow.pathObjects.Count > 0) {
			if (pathReached) {
				MoveAlongPath ();
			} else {
				ReachPath ();
			}
		}
	}

	public void ReachPath() {
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

		MoveTowardsNext ();

		if (distance <= reachDistance) {
			onPath = true;
			pathReached = true;
		}
	}

	public void MoveAlongPath() {
		if (onPath) {
			MoveTowardsNext ();
			if (distance <= reachDistance) {
				if (!goingBack) {
                    currentWayPointID++;
                } else {
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

	public void MoveTowardsNext() {
		//Gets the distance between pathObject (in EditorPath script) and current gameobject position
		distance = Vector3.Distance (pathToFollow.pathObjects [currentWayPointID].position, transform.position);
		//move to the next waypoint
		transform.position = Vector3.MoveTowards (transform.position, pathToFollow.pathObjects [currentWayPointID].position, Time.deltaTime * speedOnPath);

		if (pathToFollow.pathObjects [currentWayPointID].position != Vector3.zero) {
            if (Rotation)
            {
				destinationPoint = pathToFollow.pathObjects[currentWayPointID].position;
				Vector3 target = destinationPoint - transform.position;
				float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

			    if (Slerp) {
					transform.rotation = Quaternion.Slerp(transform.rotation, q, rotationSpeed * Time.deltaTime);
			    } else {
					transform.rotation = Quaternion.Lerp(transform.rotation, q, rotationSpeed * Time.deltaTime);
			    }
            }
        }
	}

    //Destroys the object :)
    void KillMe()
    {
        Destroy(gameObject);
    }
}
