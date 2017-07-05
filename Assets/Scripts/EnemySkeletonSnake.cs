using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonSnake : MonoBehaviour
{
    public GameObject head;
    public GameObject tail;
    public GameObject bodyPrefab;
    public GameObject legPrefab;
    public int startSize;
    public float speed;
    public float bodyPartSpeed;
    public float minDistance;
    public float bodyPartDistance;
    public float bodyPartHP;
    private List<GameObject> bodyParts;
    private List<GameObject> tailParts;
    private float distance;
    private GameManager gameManager;
    private int orderInLayer = -1;
    private int bodyPartsAmount;
    private float HP;
    private float comparableHP;

    public float GetBodyPartHP()
    {
        return bodyPartHP;
    }

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bodyParts = new List<GameObject>();
        tailParts = new List<GameObject>();
        bodyParts.Add(head);
        for (int i = 0; i < tail.transform.childCount; i++)
        {
            bodyParts.Add(tail.transform.GetChild(i).gameObject);
            tailParts.Add(tail.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < startSize; i++)
        {
            AddBodyPart();
        }
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        head.transform.Translate(Vector3.up* Time.deltaTime * speed);
        
        for (int i = 1; i < bodyParts.Count; i++)
        {

            distance = Vector3.Distance(bodyParts[i - 1].transform.position, bodyParts[i].transform.position);
            Vector3 newPosition = bodyParts[i - 1].transform.position;
            float T = Time.deltaTime * distance * minDistance * bodyPartSpeed;

            if (T > 0.5f)
            {
                T = 0.5f;
            }

            if (distance > bodyPartDistance)
            {
                bodyParts[i].transform.position = Vector3.Lerp(bodyParts[i].transform.position, newPosition, T);
                bodyParts[i].transform.rotation = Quaternion.Lerp(bodyParts[i].transform.rotation, bodyParts[i - 1].transform.rotation, T);
            }
        }
    }

    void AddBodyPart()
    {
        GameObject previousBodyPart = bodyParts[bodyParts.Count - 1];
        GameObject bodyPartType;
        if ((bodyParts.Count) % 5 == 0)
        {
            bodyPartType = legPrefab;
        }
        else
        {
            bodyPartType = bodyPrefab;
        }

        //Adds new part behind previous
        GameObject newBodyPart = Instantiate(bodyPartType, previousBodyPart.transform.position - (previousBodyPart.transform.up / 2), previousBodyPart.transform.rotation) as GameObject;
        newBodyPart.transform.parent = this.transform;

        newBodyPart.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

        //Beacuse the tail is 3 parts long and the new part has to be added in front of it
        bodyParts[bodyParts.Count - 3] = newBodyPart;
        //Sets the body parts index on the bodyParts list (don't remember if there was point to this, can possibly be removed)
        //newBodyPart.gameObject.GetComponent<BodyPart>().SetListIndex(bodyParts.Count - 3);

        /*Adds the tail parts behind all the other parts; first two parts are just moved one index forward, the last one is added as a new list item.
		Also puts the parts in the right order on top of each other when rendering (the first one is below everything, the other body and tail parts, the last one
		is above other tail parts but below the rest of the body parts).*/
        int j = 0;
        for (int i = bodyParts.Count - 2; i <= bodyParts.Count; i++)
        {
            if (i == bodyParts.Count)
            {
                bodyParts.Add(tailParts[j]);
                tailParts[j].GetComponent<SpriteRenderer>().sortingOrder = orderInLayer - tailParts.Count + j;
                break;
            }
            else
            {
                bodyParts[i] = tailParts[j];
                tailParts[j].GetComponent<SpriteRenderer>().sortingOrder = orderInLayer - tailParts.Count + j;
            }
            j++;
        }

        orderInLayer--;
        bodyPartsAmount++;
        HP += bodyPartHP;
        comparableHP += bodyPartHP;
        //Debug.Log (bodyPartsAmount);
    }

  

}