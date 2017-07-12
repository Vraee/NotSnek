using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonSnake : EnemyController
{
    public GameObject head;
    public GameObject tail;
    public GameObject bodyPrefab;
    public GameObject legPrefab;
    public GameObject fireBall;
    private Vector3 destinationPoint2;
    public int startSize;
    public float bodyPartSpeed;
    public float minDistance;
    public float bodyPartDistance;
    public float bodyPartHP;
    public float shootDelay;
    public float fireballDamage;
    public Transform target;
    private GameObject fireball;
    private List<GameObject> bodyParts;
    private List<GameObject> tailParts;
    private float distance;
    private int orderInLayer = -1;
    private int bodyPartsAmount;
    private bool takingDamage;
    private bool shooting;
    private float HP;
    private float comparableHP;
    private Vector3 startPos;

    public Color32 bodyPartColor = new Color32(112, 131, 163, 255);

    public float floatSpan = 0.25f;
    private float startX;

	private float index;
	private Vector3 pos;
	private Vector3 _startPosition;
	private Vector3 prevPos;

    public float GetBodyPartHP()
    {
        return bodyPartHP;
    }

    public override void MoveEnemy()
    {
        base.MoveEnemy();
    }

    // Use this for initialization
    new void Start()
    {
        base.Start();
     //   InvokeRepeating("Shoot", 2, shootDelay);
        takingDamage = false;
        startPos = head.transform.position;
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

		pos = transform.position;
		_startPosition = bodyParts[1].transform.position;
    }


    // Update is called once per frame
    new void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        for (int i = 1; i < bodyParts.Count; i++)
        {
            
            distance = Vector3.Distance(bodyParts[i - 1].transform.position, bodyParts[i].transform.position);
            Vector3 newPosition = bodyParts[i - 1].transform.position;
            float T = Time.deltaTime * distance * minDistance * bodyPartSpeed;

			if (T > 0.5f)
			{
				T = 0.5f;
			}
            

            if (true)
            {
				prevPos = transform.position;
				Vector3 tmpPos = Vector3.Lerp(bodyParts[i].transform.position, newPosition, T);
				bodyParts[i].transform.position = tmpPos;

				//if (i == 1) //PLS DON'T REMOVE ANYTHING HERE PLZ-------------------------------------------------------------------------------------------V T.VREEe
                //{
					//index += Time.deltaTime;
					/*Vector3 tmpPos = new Vector3(0, 0, 0);
					//pos.y += 5 * Time.deltaTime;
					//tmpPos.y = pos.y;
					tmpPos.y = newPos.y;
					tmpPos.x = newPos.x + Mathf.Sin(index * speed) * 0.5f;

					transform.position = tmpPos;
					Debug.Log(tmpPos);*/

					/*Vector3 nextPointPos = head.GetComponent<MoveOnPath>().pathToFollow.pathObjects[head.GetComponent<MoveOnPath>().currentWayPointID].position;
					float distX;
					float distY;
					if (Mathf.Abs(nextPointPos.x) > Mathf.Abs(bodyParts[i].transform.position.x))
						distX = Mathf.Abs(nextPointPos.x) - Mathf.Abs(bodyParts[i].transform.position.x);
					else
						distX = Mathf.Abs(bodyParts[i].transform.position.x) - Mathf.Abs(nextPointPos.x);

					if (Mathf.Abs(nextPointPos.y) > Mathf.Abs(bodyParts[i].transform.position.y))
						distY = Mathf.Abs(nextPointPos.y) - Mathf.Abs(bodyParts[i].transform.position.y);
					else
						distY = Mathf.Abs(bodyParts[i].transform.position.y) - Mathf.Abs(nextPointPos.y);

					//if (Vector3.Distance(head.GetComponent<MoveOnPath>().pathToFollow.pathObjects[head.GetComponent<MoveOnPath>().currentWayPointID].position, transform.position) > head.GetComponent<MoveOnPath>().distance)
					if (distX > head.GetComponent<MoveOnPath>().distance)
					{
						bodyParts[i].transform.position = tmpPos + new Vector3(0.0f, Mathf.Sin(Time.time * speed), 0.0f) * 0.5f;
						//Debug.Log("y");
					}

					//if (Vector3.Distance(head.GetComponent<MoveOnPath>().pathToFollow.pathObjects[head.GetComponent<MoveOnPath>().currentWayPointID].position, transform.position) > head.GetComponent<MoveOnPath>().distance)
					if (distY > head.GetComponent<MoveOnPath>().distance)
					{
						bodyParts[i].transform.position = tmpPos + new Vector3(Mathf.Sin(Time.time * speed), 0.0f, 0.0f) * 0.5f;
						//Debug.Log("x");
					}

					/*Vector3 nextPointPos = head.GetComponent<MoveOnPath>().pathToFollow.pathObjects[head.GetComponent<MoveOnPath>().currentWayPointID].position;
					float distX;
					float distY;

					if (Mathf.Abs(nextPointPos.x) > Mathf.Abs(bodyParts[i].transform.position.x))
						distX = Mathf.Abs(nextPointPos.x) - Mathf.Abs(bodyParts[i].transform.position.x);
					else
						distX = Mathf.Abs(bodyParts[i].transform.position.x) - Mathf.Abs(nextPointPos.x);

					if (Mathf.Abs(nextPointPos.y) > Mathf.Abs(bodyParts[i].transform.position.y))
						distY = Mathf.Abs(nextPointPos.y) - Mathf.Abs(bodyParts[i].transform.position.y);
					else
						distY = Mathf.Abs(bodyParts[i].transform.position.y) - Mathf.Abs(nextPointPos.y);

					bodyParts[i].transform.position = tmpPos + new Vector3(distY * Mathf.Sin(index), distX * Mathf.Sin(index), 0.0f);
					Debug.Log(head.GetComponent<MoveOnPath>().currentWayPointID);
					Debug.Log(nextPointPos + " distX " + distX + " distY: " + distY + " snekpos " + bodyParts[i].transform.position);*/

					//Debug.Log(transform.position);

                    /*destinationPoint2 = bodyParts[i - 1].transform.position;
                    Vector3 target = destinationPoint2 - bodyParts[i].transform.position;
                    float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    bodyParts[i].transform.rotation = Quaternion.Slerp(bodyParts[i].transform.rotation, q, 5 * T);

					/*float AngleRad = Mathf.Atan2(destinationPoint2.y - bodyParts[i].transform.position.y, destinationPoint2.x - bodyParts[i].transform.position.x);
					float angle2 = (180 / Mathf.PI) * AngleRad;

					bodyParts[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle2 - 90f));*/

                    //bodyParts [i].transform.rotation = Quaternion.Lerp (bodyParts [i].transform.rotation, Quaternion.LookRotation(bodyParts[i].transform.position), T);
                //}
                /*else
                {
                    bodyParts[i].transform.rotation = Quaternion.Lerp(bodyParts[i].transform.rotation, bodyParts[i - 1].transform.rotation, T);
                }*/
				destinationPoint2 = bodyParts[i - 1].transform.position;
				Vector3 target = destinationPoint2 - bodyParts[i].transform.position;
				float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
				bodyParts[i].transform.rotation = Quaternion.Slerp(bodyParts[i].transform.rotation, q, 5 * T);

            }
        }
    }

    void Rotate()
    {
        destinationPoint = targetPlayerPart.transform.position;
        float camDis = Camera.main.transform.position.y - head.transform.position.y;
        
        float AngleRad = Mathf.Atan2(destinationPoint.y - head.transform.position.y, destinationPoint.x - head.transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;

        head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
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

    public void RemoveBodyPart(int removableIndex)
    {

        GameObject removablePart = bodyParts[removableIndex];

        if (HP > 0 && removableIndex != 0 && removableIndex != bodyParts.Count - tailParts.Count)
        {
            bodyParts.Remove(removablePart);
            Destroy(removablePart);
            
            bodyPartsAmount--;
        }

        if (HP <= 0)
        {
            Die();
        }
    }

    IEnumerator EnemyDamage(SkeletonPart hitPart) //USED TO BE "TakeDamage"
    {
            float enemyDamage = hitPart.GetEnemy().GetComponent<Fireball>().damage;
            ReduceHP(enemyDamage);
        
            if (!takingDamage)
            {
                takingDamage = true;
                for (int i = 0; i < 2; i++)
                {
                    foreach (GameObject bodyPart in bodyParts)
                    {
                        bodyPart.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    yield return new WaitForSeconds(0.1f);
                    for (int j = 0; j < bodyParts.Count; j++)
                    {
                        if (j == 0)
                            bodyParts[j].GetComponent<SpriteRenderer>().color = Color.white;
                        else
                            bodyParts[j].GetComponent<SpriteRenderer>().color = bodyPartColor;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                takingDamage = false;
            }
        }

    public void ReduceHP(float lostHP)
    {
        HP = HP - lostHP;
        if (HP <= comparableHP)
        {
            comparableHP = comparableHP - bodyPartHP;
            RemoveBodyPart(bodyPartsAmount);
        }
    }

    private void Shoot()
    {
        if (!shooting) return;
        Vector3 playerPos = head.transform.position;
        Vector3 playerDirection = head.transform.up;
        Quaternion playerRotation = head.transform.rotation;
        float spawnDistance = 1;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
        fireball = Instantiate(fireBall, spawnPos, head.transform.rotation);
        fireball.GetComponent<Fireball>().damage = fireballDamage;
    }

}