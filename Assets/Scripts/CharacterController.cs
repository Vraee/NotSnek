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
    public float deadzone = 0.25f;

    //Holds the fireball gameobject during buildup
    private GameObject fireball;
    //how much damage is added to the fireball per sec
    public float damageIncrease;
    public float fireballDamage;

    public bool controllerInput;

    private GameManager gameManager;
	//Contains ALL the bodyparts, including head and tail parts
    private List<GameObject> bodyParts;
	private List<GameObject> tailParts;
	private Vector3 destinationPoint;
	private float distance;
    private int orderInLayer = -1;
	//Counts ONLY the bodyparts between head and tail
	private int bodyPartsAmount; //USED TO BE "tailLength"!
	private float baseHP;
	private float HP;
	//If HP reaches this, a body part is removed, the amount of body parts * bodyPartHP - bodyPartHP
	private float comparableHP;
	private bool takingDamage;
	private int collectibleSum;
	private bool berserk;
	private float berserkCooldown = 2f;
	private float berserkCoolDownTimer;
	private float damageSum;
    private Camera cam;
    private float viewHeight;
    private float viewWidth;
	//The colour for all the bodyparts besides head; since only the head takes damage, the other parts are a bit darker
	private Color32 bodyPartColor = new Color32 (112, 131, 163, 255);
    
    public float bulletsPerSecond = 14.0f;
    private bool shooting = false;

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
        cam = Camera.main;
        viewHeight = 2f * cam.orthographicSize;
        viewWidth = viewHeight * cam.aspect;
        InvokeRepeating("Shoot", 1.0f, 1.0f / bulletsPerSecond);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bodyParts = new List<GameObject> ();
		tailParts = new List<GameObject>();
		bodyParts.Add (head);

		for (int i = 0; i < tail.transform.childCount; i++)
		{
			bodyParts.Add(tail.transform.GetChild(i).gameObject);
			tailParts.Add(tail.transform.GetChild(i).gameObject);
			tail.transform.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().color = bodyPartColor;
			tail.transform.GetChild (i).gameObject.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer - tailParts.Count + i;
		}


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
        Fire();

		if (!berserk && bodyPartsAmount > 0 && (Input.GetKeyDown(KeyCode.Space) || (Input.GetButtonDown("Berserk"))))
		{
			berserk = true;
			StartCoroutine("Berserk");
			berserkCoolDownTimer = Time.time + berserkCooldown;
		} else if  (berserk && Time.time >= berserkCoolDownTimer && (Input.GetKeyDown(KeyCode.Space) || (Input.GetButtonDown("Berserk")))) {
			StopCoroutine ("Berserk");
			StopBerserk ();
		}
	}
		
	private void RotateToMouse()
	{
		//fixed by internet with collaboration with the cleaner man god_Miika
		Debug.DrawLine(head.transform.position, destinationPoint);

		float camDis = Camera.main.transform.position.y - head.transform.position.y;

		// Get the mouse position in world space. Using camDis for the Z axis.
		Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

		float AngleRad = Mathf.Atan2(mouse.y - head.transform.position.y, mouse.x - head.transform.position.x);
		float angle = (180 / Mathf.PI) * AngleRad;

		head.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90f));
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
            Vector2 stickInput = new Vector2(Input.GetAxis("HorizontalStick1"), Input.GetAxis("VerticalStick1"));
            if (stickInput.magnitude < deadzone) {
                stickInput = Vector2.zero;
            }
            else
            {
                stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
            }

            head.transform.Translate(stickInput * Time.deltaTime * speed, Space.World);
            

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

        //checking if the player is going off screen
        if (head.transform.position.y >= viewHeight / 2)
        {
            head.transform.position = new Vector2(head.transform.position.x, viewHeight / 2);
        }
        if (head.transform.position.y <= -viewHeight / 2)
        {
            head.transform.position = new Vector2(head.transform.position.x, -viewHeight / 2);
        }
        if (head.transform.position.x >= viewWidth / 2)
        {
            head.transform.position = new Vector2(viewWidth / 2, head.transform.position.y);
        }
        if (head.transform.position.x <= -viewWidth / 2)
        {
            head.transform.position = new Vector2(-viewWidth / 2, head.transform.position.y);
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

				destinationPoint = bodyParts[i - 1].transform.position;
				Vector3 target = destinationPoint - bodyParts[i].transform.position;
				float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
				bodyParts[i].transform.rotation = Quaternion.Slerp(bodyParts[i].transform.rotation, q, T);

				//bodyParts [i].transform.rotation = Quaternion.Lerp (bodyParts [i].transform.rotation, bodyParts [i - 1].transform.rotation, T);
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
		newBodyPart.GetComponent<SpriteRenderer>().color = bodyPartColor;

		//Beacuse the tail is 3 parts long and the new part has to be added in front of it
		bodyParts [bodyParts.Count - 3] = newBodyPart;
			
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
				tailParts[j].GetComponent<SpriteRenderer>().color = bodyPartColor;
				break;
			}
			else
			{
				bodyParts[i] = tailParts[j];
				tailParts[j].GetComponent<SpriteRenderer>().sortingOrder = orderInLayer - tailParts.Count + j;
				tailParts[j].GetComponent<SpriteRenderer>().color = bodyPartColor;
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
        if(HP <= 0)
        {
            shooting = false;
            gameObject.SetActive(false);
            gameManager.Restart();
        }
        //Debug.Log ("bodyparts left " + bodyPartsAmount + " " + (bodyParts.Count - 1 - tailParts.Count));
    }


    private void Fire()
    {

        shooting = false;
        if (Input.GetMouseButton(0) || Input.GetButton("Fire1"))
        {
            shooting = true;
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


    


    private void Fire3()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            Vector3 playerPos = head.transform.position;
			Vector3 playerDirection = head.transform.up;
            Quaternion playerRotation = head.transform.rotation;
            float spawnDistance = 1;
            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
			
            fireball = Instantiate(fireBall, spawnPos, head.transform.rotation);
			
			fireball.GetComponent<Fireball>().player = head;
			fireball.GetComponent<Fireball>().damageIncrease = damageIncrease;
			fireball.GetComponent<Fireball>().damage = fireballDamage;
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
				yield return new WaitForSeconds (0.05f);
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
				if (i == 1 && berserk) {
					berserk = false;
					for (int j = 0; j < bodyParts.Count; j++)
					{
						StopBerserk ();
					}
				}
			}
		}
	}

	public void StopBerserk() {
		berserk = false;
		for (int i = 0; i < bodyParts.Count; i++)
		{
			if (i == 0)
				bodyParts[i].GetComponent<SpriteRenderer>().color = Color.white;
			else
				bodyParts[i].GetComponent<SpriteRenderer>().color = bodyPartColor;
			bodyParts [i].layer = 8;
			var emni = bodyParts[i].GetComponent<ParticleSystem>().emission;
			emni.enabled = false;
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
					for (int j = 0; j < bodyParts.Count; j++) {
						if (j == 0)
							bodyParts [j].GetComponent<SpriteRenderer> ().color = Color.white;
						else
							bodyParts [j].GetComponent<SpriteRenderer> ().color = bodyPartColor;
					}
					yield return new WaitForSeconds (0.1f);
				}
				takingDamage = false;
			}
		}
	}

    IEnumerator EnemyFireballDamage(float damage)
    {
        if (!berserk)
        {
            ReduceHP(damage);
            if (!takingDamage && !berserk)
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
    }
}
