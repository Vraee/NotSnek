﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {
	public GameObject head;
	public GameObject tail;
	public GameObject bodyPrefab;
	public GameObject legPrefab;
    public int startSize;
	public GameObject fire;
    public GameObject fireBall;
    //Needed only if translate is used to move the character; not in use currently
    public float speed;
	public float bodyPartSpeed; 
	public float minDistance;
	public float bodyPartDistance;
	public int bodyPartHP;
    
    //Holds the fireball gameobject during buildup
    private GameObject temp;
    //how much damage is added to the fireball per sec
    public float damageIncrease;
    public float fireballDamage;

    private List<GameObject> bodyParts;
	private List<string> bodyPartTags;
	private Vector3 destinationPoint;
	private float distance;
    private int orderInLayer = -1;
	private ParticleSystem fireParticles;
	private int tailLength;
    private int baseHP;
	private int HP;
	private int comparableHP;
	private bool takingDamage;
	private int powerUpLimit = 5;
	private int collectibleSum;
	private bool berserk;
	private int berserkTime;

	// Use this for initialization
	void Start () {
		bodyParts = new List<GameObject> ();
		bodyPartTags = new List<string>();
		bodyParts.Add (head);
		bodyParts.Add (tail);
        
       
		fireParticles = head.GetComponent<ParticleSystem> ();
		fire.SetActive (false);
        fireParticles.Stop();

        for (int i = 0; i < startSize; i++) {
			AddBodyPart ();
		}

        baseHP = bodyPartHP;
        HP = HP + baseHP;
		comparableHP = HP - bodyPartHP;
		takingDamage = false;
		berserk = false;
	}

	void Update() {
		destinationPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		RotateToMouse ();
		Move ();
       // Fire();
        Fire3();
		


        if (Input.GetKey(KeyCode.Space))
		{
			StartCoroutine(Berserk());
		}

	}

	public int GetTailLength() {
		return tailLength;
	}

	public void SetTailLength(int newTailLength) {
		tailLength = newTailLength;
	}

	public int GetComparableHP() {
		return comparableHP;
	}

	public void SetComparableHP(int newComparableHP) {
		comparableHP = newComparableHP;
	}

	public int GetHP() {
		return HP;
	}

	public void SetHP(int newHP) {
		HP = newHP;
	}

	public int GetBodyPartHP() {
		return bodyPartHP;
	}

	public void SetBodyPartHP(int newBodyPartHP) {
		bodyPartHP = newBodyPartHP;
	}

	public int GetPowerUpLimit() {
		return powerUpLimit;
	}

	public void SetPowerUpLimit(int newPowerUpLimit) {
		powerUpLimit = newPowerUpLimit;
	}

	public int GetCollectibleSum() {
		return collectibleSum;
	}

	public void SetCollectibleSum(int newCollectibleSum) {
		collectibleSum = newCollectibleSum;
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
            //multiplayer used in translate to move the player
            float speedMultiplier;
			//MoveThisPieceOfTrash if distance is long enough
			if (mouseDistance > 0.1f) {
                //If distance between mouse and player is below certain limit, use this to calculate the multiplier, else use preset value
                if (mouseDistance <= 5)
                {
                    speedMultiplier = mouseDistance;
                }
                else
                {
                    speedMultiplier = 5;
                }

				if (speedMultiplier > 5)
				Debug.Log("5 is too small for speedmultiplier " + speedMultiplier);
				
				head.transform.Translate (head.transform.up * speed * speedMultiplier * Time.deltaTime, Space.World);
				//head.transform.position = Vector3.Lerp (head.transform.position, _target, 0.1f);
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

        newBodyPart.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
        orderInLayer--;

		GameObject tempTail = bodyParts [bodyParts.Count - 1];
		bodyParts [bodyParts.Count - 1] = newBodyPart;
        newBodyPart.gameObject.GetComponent<BodyPart>().SetListIndex(bodyParts.Count - 1);

		bodyParts.Add (tempTail);
        tempTail.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

		tailLength++;
		berserkTime = tailLength;
		HP += bodyPartHP;
		comparableHP += bodyPartHP;
	}

	public void RemoveBodyPart(int removableIndex) {
		GameObject removablePart = bodyParts [removableIndex];

        if (HP > 0 && removableIndex != 0 && removableIndex != bodyParts.Count - 1) {
            bodyParts.Remove(removablePart);
            Destroy(removablePart);

            tailLength--;
        }
	}

	public void Fire() {

        if (Input.GetMouseButton(0))
        {
            fire.SetActive(true);

            if (!fireParticles.isPlaying)
            {
                fireParticles.Play();
            }
        }
        else
        {
            fireParticles.Stop();
            fire.SetActive(false);
        }   
	}
    

    private void Fire3()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 playerPos = head.transform.position;
            Vector3 playerDirection = head.transform.up;
            Quaternion playerRotation = head.transform.rotation;
            float spawnDistance = 1;
            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
            temp = Instantiate(fireBall, spawnPos, head.transform.rotation);
        }

        if (Input.GetMouseButton(0) && temp != null) {
            temp.transform.position = head.transform.position + (head.transform.up * 1);
            temp.transform.rotation = head.transform.rotation;

            //Increase fireball size and damage when mouse is held
            if (temp.transform.localScale.x <= 1) {
                temp.transform.localScale += new Vector3(0.25F * Time.deltaTime, 0.25F * Time.deltaTime, 0);
                //fireballDamage = fireballDamage + damageIncrease * Time.deltaTime;
                temp.GetComponent<Fireball>().IncreaseDamage(damageIncrease * Time.deltaTime);
            }
        }
        if (Input.GetMouseButtonUp(0) && temp != null)
        {
            temp.GetComponent<Fireball>().Shoot();
        }
    }

	IEnumerator Berserk()
	{
		for (int i = 1; i < bodyParts.Count - 1; i++)
		{
			RemoveBodyPart(i);
		}

		foreach (GameObject bodyPart in bodyParts)
		{
			bodyPartTags.Add(bodyPart.tag);
			bodyPart.GetComponent<SpriteRenderer>().color = Color.blue;
			bodyPart.tag = "Fire";
		}

		yield return new WaitForSeconds(berserkTime);
		//berserk = false;

		for (int i = 0; i < bodyParts.Count; i++ )
		{
			bodyParts[i].GetComponent<SpriteRenderer>().color = Color.white; 
			bodyParts[i].tag = bodyPartTags[i];
		}

		HP = baseHP;

	}

	IEnumerator TakeDamage()
	{
		if (!takingDamage) {
			takingDamage = true;
			for (int i = 0; i < 2; i++) {
				foreach (GameObject bodyPart in bodyParts) {
					bodyPart.GetComponent<SpriteRenderer> ().color = Color.red;     
				}
				yield return new WaitForSeconds (0.1f);
				foreach (GameObject bodyPart in bodyParts) {
					bodyPart.GetComponent<SpriteRenderer> ().color = Color.white;     
				}
				yield return new WaitForSeconds (0.1f);
			}
			takingDamage = false;
		}
	}
}
