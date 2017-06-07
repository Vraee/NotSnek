using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //attempting tut
    //List of transforms for the body parts
    public List<Transform> BodyParts = new List<Transform>();
    public GameObject firstBodyPart;
    public float minDistance;

    public float BodyPartSpeed = 1;
    public float RotationSpeed = 50;

    
    public GameObject BodyPrefab;

    public int beginSize;
    private float distance;
    private Transform curBodyPart;
    private Transform previousBodyPart;
    //----------------------------------------------------------


    public float speed;
    private bool mouseReached;
    // Use this for initialization
    void Start () {
        RotateToMouse();
        for(int i = 0; i < beginSize-1; i++)
        {
            AddBodyPart();
        }
    }

    private void FixedUpdate()
    {
        RotateToMouse();
        Move();
    }

    private void RotateToMouse()
    {
        
            //Get the Screen positions of the object
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(firstBodyPart.transform.position);

            //Get the Screen position of the mouse
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        
            //Get the angle between the points
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            //Ta Daaa
            BodyParts[0].transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle-90));
    }


    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        //https://docs.unity3d.com/ScriptReference/Mathf.Atan2.html
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void Move()
    {



        if (Input.GetMouseButton(1))
        {


            //Get the Screen positions of the object
            Vector3 _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //revert z to 0
            _target.z = firstBodyPart.transform.position.z;
            //distance between mouse and player
            var mouseDistance = Vector3.Distance(firstBodyPart.transform.position, _target);
            //Debug.Log(mouseDistance);
            //MoveThisPieceOfTrash if distance is long enough
            if (mouseDistance > 0.1)
            {
               // BodyParts[0].position = Vector3.MoveTowards(transform.position, _target, Time.smoothDeltaTime * speed);
                //BodyParts[0].Translate(BodyParts[0].position = Vector3.MoveTowards(transform.position, _target, Time.smoothDeltaTime * speed));
                BodyParts[0].Translate(-BodyParts[0].up * speed * Time.smoothDeltaTime, Space.World);
               //transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
            }



            float curSpeed = speed;
            //  BodyParts[0].Translate(BodyParts[0].up * curSpeed * Time.smoothDeltaTime, Space.World);
            
        for (int i = 1; i < BodyParts.Count; i++)
        {
            
            curBodyPart = BodyParts[i];
            previousBodyPart = BodyParts[i - 1];
            distance = Vector3.Distance(previousBodyPart.position, curBodyPart.position);
            Vector3 newPos = previousBodyPart.position;
            newPos.z = BodyParts[0].position.z;

            float T = Time.deltaTime * distance / minDistance * curSpeed;
            
               // Debug.Log("T:" + T);
            if(T > 0.05)
            {
                 //   Debug.Log("moi");
                T = 0.05f;
                curBodyPart.position = Vector3.Slerp(curBodyPart.position, newPos, Time.deltaTime);
                curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, previousBodyPart.rotation, 1);
            }
        }

            
        }
    }

    private void AddBodyPart()
    {
        Transform newPart = (Instantiate(BodyPrefab, BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;
        newPart.SetParent(transform);
        BodyParts.Add(newPart);
    }
}

