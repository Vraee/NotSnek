using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour {
	public GameObject head;
	public GameObject tail;
	public GameObject bodyPrefab;
	public GameObject legPrefab;
    public int startSize;
	public GameObject fire;
    public GameObject fireBall;
    public float speed;
	public float bodyPartSpeed; 
	public float minDistance;
	public float bodyPartDistance;
	public float bodyPartHP;
	public int takeDamageDelay = 1;
	public int powerUpLimit = 5;
    
    //Holds the fireball gameobject during buildup
    private GameObject temp;
    //how much damage is added to the fireball per sec
    public float damageIncrease;
    public float fireballDamage;

    public bool controllerInput;

    private GameManager gameManager;
    private List<GameObject> bodyParts;
	private List<GameObject> tailParts;
	private Vector3 destinationPoint;
	private float distance;
    private int orderInLayer = -1;
	private ParticleSystem fireParticles;
	private int bodyPartsAmount; //USED TO BE "tailLength"!
	private float baseHP;
	private float HP;
	//If HP reaches this, a body part is removed, the amount of body parts * bodyPartHP - bodyPartHP
	private float comparableHP;
	private bool takingDamage;
	private int collectibleSum;
	private bool berserk;
	private float damageSum;
		
	public int GetBodyPartsAmount() {
		return bodyPartsAmount;
	}

	public void SetBodyPartsAmount(int newBodyPartsAmount) {
		bodyPartsAmount = newBodyPartsAmount;
	}

	public float GetComparableHP() {
		return comparableHP;
	}

	public void SetComparableHP(int newComparableHP) {
		comparableHP = newComparableHP;
	}

	public float GetHP() {
		return HP;
	}

	public void SetHP(int newHP) {
		HP = newHP;
	}

	public float GetBodyPartHP() {
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

	public bool GetBerserk()
	{
		return berserk;
	}

	public void SetBerserk(bool newBerserk)
	{
		berserk = newBerserk;
	}

	public float GetDamageSum()
	{
		return damageSum;
	}

	public void SetDamageSum(float newDamageSum)
	{
		damageSum = newDamageSum;
	}

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bodyParts = new List<GameObject> ();
		tailParts = new List<GameObject>();
		bodyParts.Add (head);

		for (int i = 0; i < tail.transform.childCount; i++)
		{
			bodyParts.Add(tail.transform.GetChild(i).gameObject);
			tailParts.Add(tail.transform.GetChild(i).gameObject);
		}

		//pisa shit
		fireParticles = head.GetComponent<ParticleSystem>();
		fire.SetActive (false);
		//fireParticles.Play();
		var em = fireParticles.emission;
		em.enabled = false;

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
        
        if (!controllerInput) {
            RotateToMouse();
        }

        Movement();
		//Fire();
		Fire3();

		if (Input.GetKey(KeyCode.Space) && !berserk && bodyParts.Count > 1 + tailParts.Count)
		{
			berserk = true;
			StartCoroutine("Berserk");
		}
	}
		
	private void RotateToMouse()
	{
        //plz fix
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

    private void ControllerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
        
        head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        if (Input.GetButton("Move"))
        {
            head.transform.Translate(head.transform.up * speed * Time.deltaTime, Space.World);
        }

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

    private void Movement()
    {
        if (controllerInput)
        {
            head.transform.Translate(Input.GetAxis("HorizontalStick1") * Time.deltaTime * speed, Input.GetAxis("VerticalStick1") * Time.deltaTime * speed, 0, Space.World);
            float _angle = Mathf.Atan2(-Input.GetAxis("HorizontalStick2"), -Input.GetAxis("VerticalStick2")) * Mathf.Rad2Deg;
        
            if (new Vector2(Input.GetAxis("HorizontalStick2"), Input.GetAxis("VerticalStick2")) != Vector2.zero) {
                var rotation = Quaternion.AngleAxis(_angle, new Vector3(0, 0, 1));
                head.transform.rotation = Quaternion.Lerp(head.transform.rotation, rotation, 10 * Time.deltaTime);
            }
        }
        else
        {
            head.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed, Input.GetAxis("Vertical") * Time.deltaTime * speed, 0, Space.World);
        }
        

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

				if (i == 1) {
					destinationPoint = bodyParts[i - 1].transform.position;
					Vector3 target = destinationPoint - bodyParts[i].transform.position;
					float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
					Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
					bodyParts[i].transform.rotation = Quaternion.Slerp(bodyParts[i].transform.rotation, q, T);

					//bodyParts [i].transform.rotation = Quaternion.Lerp (bodyParts [i].transform.rotation, Quaternion.LookRotation(bodyParts[i].transform.position), T);
				} else {
					bodyParts [i].transform.rotation = Quaternion.Lerp (bodyParts [i].transform.rotation, bodyParts [i - 1].transform.rotation, T);
				}
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

		//Adds new part behind previous
        GameObject newBodyPart = Instantiate(bodyPartType, previousBodyPart.transform.position - (previousBodyPart.transform.up / 2), previousBodyPart.transform.rotation) as GameObject;
		newBodyPart.transform.parent = this.transform;

        newBodyPart.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

		//Beacuse the tail is 3 parts long and the new part has to be added in front of it
		bodyParts [bodyParts.Count - 3] = newBodyPart;
		//Sets the body parts index on the bodyParts list (don't remember if there was point to this, can possibly be removed)
        newBodyPart.gameObject.GetComponent<BodyPart>().SetListIndex(bodyParts.Count - 3);
			
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
        gameManager.UpdateMultiplier();
        //Debug.Log (bodyPartsAmount);
    }

	public void RemoveBodyPart(int removableIndex) {

		GameObject removablePart = bodyParts [removableIndex];

		if (HP > 0 && removableIndex != 0 && removableIndex != bodyParts.Count - tailParts.Count) {
            bodyParts.Remove(removablePart);
            Destroy(removablePart);

            bodyPartsAmount--;
        }
        gameManager.UpdateMultiplier();
        //Debug.Log ("bodyparts left " + bodyPartsAmount + " " + (bodyParts.Count - 1 - tailParts.Count));
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
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            Vector3 playerPos = head.transform.position;
            Vector3 playerDirection = head.transform.up;
            Quaternion playerRotation = head.transform.rotation;
            float spawnDistance = 1;
            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
            temp = Instantiate(fireBall, spawnPos, head.transform.rotation);
        }

        if (Input.GetMouseButton(0) || Input.GetButton("Fire1") && temp != null) {
            temp.transform.position = head.transform.position + (head.transform.up * 1);
            temp.transform.rotation = head.transform.rotation;

            //Increase fireball size and damage when mouse is held
            if (temp.transform.localScale.x <= 10) {
                temp.transform.localScale += new Vector3(0.25f * Time.deltaTime, 0.25f * Time.deltaTime, 0.25f * Time.deltaTime);
                //fireballDamage = fireballDamage + damageIncrease * Time.deltaTime;
                temp.GetComponent<Fireball>().IncreaseDamage(damageIncrease * Time.deltaTime);
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1") && temp != null)
        {
            temp.GetComponent<Fireball>().Shoot();
        }
    }

	IEnumerator Berserk() {
		if (berserk)
		{
			int bodyPartAmoutAtStart = bodyParts.Count;

			//Changes the layer of each bodypart to 11, which is "Berserk"
			foreach (GameObject bodyPart in bodyParts)
			{
				bodyPart.layer = 11;
			}

			//Add effects to each bodypart after a delay of 0,1 seconds
			foreach (GameObject bodyPart in bodyParts)
			{
				yield return new WaitForSeconds (0.1f);
				var emni = bodyPart.GetComponent<ParticleSystem>().emission;
				emni.enabled = true;

				bodyPart.GetComponent<SpriteRenderer>().color = new Vector4 (0.9f,0.9f,0.9f,0.9f);
			}

			//Removes each bodypart (excluding head and tail) starting from the last one after 1 second delay
			for (int i = bodyParts.Count - tailParts.Count - 1; i > 0; i--)
			{
				yield return new WaitForSeconds(1);
				RemoveBodyPart(i);

				//Reduces HP if there is too much of it in relation to amount of bodyparts
				//if (HP > bodyPartsAmount * bodyPartHP + baseHP) {
				while (HP > bodyPartsAmount * bodyPartHP + baseHP) {
					ReduceHP (1);
				}
				//}

				//Once the last part is reached, changes remaining bodyparts back to normal (layer 8 = "Plyaer")
				if (i == 1) {
					berserk = false;
					for (int j = 0; j < bodyParts.Count; j++)
					{
						bodyParts[j].GetComponent<SpriteRenderer>().color = Color.white;
						bodyParts [j].layer = 8;
						var emni = bodyParts[j].GetComponent<ParticleSystem>().emission;
						emni.enabled = false;
					}
				}
			}
		}
	}

	public void ReduceHP (float lostHP) {
		HP = HP - lostHP;

		if (HP <= comparableHP) {
			comparableHP = comparableHP - bodyPartHP;

			if (!berserk) {
				RemoveBodyPart (bodyPartsAmount);
			}
		}

		/*if (HP <= 0) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().name); //Probably should reaload the scene from the start in finished versiom
		}*/

		/*Debug.Log ("ReduceHP: HP " + HP);
		Debug.Log ("ReduceHP: comparableHP" + comparableHP);*/
	}

	IEnumerator EnemyDamage(BodyPart hitPart) //USED TO BE "TakeDamage"
	{
		if (!berserk) {
			//Gets the enemy's damage output and subtracts it form player's HP
			float enemyDamage = hitPart.GetEnemy ().GetComponent<EnemyController> ().damageOutput;
			ReduceHP (enemyDamage);

			//Changes player's color to red and back twice
			if (!takingDamage && !berserk) {
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
}
