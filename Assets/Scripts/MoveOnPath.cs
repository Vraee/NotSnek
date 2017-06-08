using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour {

    //Script used to access pathPoints
    public EditorPath pathToFollow;
    //Starting point on the path
    public int currentWayPointID;
    public float speed;
    //how far from the next point should the enemy stop
    private float reachDistance = 0.0f;
    public float rotationSpeed = 5.0f;
    //Maybe change to enum
    public string pathName;
    private Vector3 lastPosition;
    private Vector3 currentPosition;

	// Use this for initialization
	void Start () {
        pathToFollow = GameObject.Find(pathName).GetComponent<EditorPath>();
        lastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //Gets the distance between pathObject (in EditorPath script) and current gameobject position
        float distance = Vector3.Distance(pathToFollow.pathObjects[currentWayPointID].position, transform.position);
        //move to the next waypoint
        transform.position = Vector3.MoveTowards(transform.position, pathToFollow.pathObjects[currentWayPointID].position, Time.deltaTime * speed);

        //rotate towards the next waypoint
        var rotation = Quaternion.LookRotation(pathToFollow.pathObjects[currentWayPointID].position, transform.position);
        rotation.y = 0;
        rotation.x = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (distance <= reachDistance)
        {
            currentWayPointID++;
        }
        

        if (currentWayPointID == pathToFollow.pathObjects.Count)
        {
            currentWayPointID = 0;
            //KillMe();
        }
	}

    //Destroys the object :)
    void KillMe()
    {
        Destroy(gameObject);
    }
}
