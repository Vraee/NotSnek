using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float stamina;
	public float damageOutput;
    public float smallDropRate;
    public float mediumDropRate;
    public float largeDropRate;
    public GameObject[] powerUps;
    public int maxPowerUpAmount;
    public int minPowerUpAmount;
    private int powerUpAmount;
	public GameObject targetPlayerPart;
    public GameObject deathPrefab;
	public float screenShakeDuration;
	public float screenShakeAmount;
	public float score = 1;
    
	[HideInInspector]
	public GameManager gameManager;
    private SpriteRenderer sprite;
    private Color hitColor = Color.red;
    private bool inflictDamage;
	protected bool attacking;
    protected bool retreating;
	public Vector3 destinationPoint;
    private GameObject canvas;
    private Vector3 attackTarget;
	private Vector3 attackStartPos;
    private float berserkDamage;
	private float timer;
	private bool moving;
	private bool vulnerable;
	private float childAttackDelay;
	private float childRetreatSpeed;

	public float GetDamageOutput() {
		return damageOutput;
	}

	public void SetDamageOutput(int damageOutput) {
		this.damageOutput = damageOutput;
	}

	public bool GetAttacking() {
		return attacking;
	}

	public void SetAttacking(bool attacking) {
		this.attacking = attacking;
	}

	public bool SetRetreating() {
		return retreating;
	}

	public void SetRetreating(bool retreating) {
		this.retreating = retreating;
	}

	public float GetTimer() {
		return timer;
	}

	public void SetTimer(float timer) {
		this.timer = timer;
	}

	public bool GetMoving() {
		return moving;
	}

	public void SetMoving(bool moving) {
		this.moving = moving;
	}

	public bool GetVulnerable() {
		return moving;
	}

	public void SetVulnerable(bool vulnerable) {
		this.vulnerable = vulnerable;
	}

	// Use this for initialization
	protected void Start()
	{
        canvas = GameObject.Find("Canvas");
        powerUpAmount = Random.Range(minPowerUpAmount, maxPowerUpAmount + 1);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        targetPlayerPart = GameObject.Find ("Head");
		sprite = GetComponent<SpriteRenderer>();
		attacking = false;
		retreating = false;
		attackStartPos = gameObject.transform.position;

		if (this is EnemyEarlyBird) {
			childAttackDelay = this.GetComponent<EnemyEarlyBird> ().attackDelay;
			childRetreatSpeed = this.GetComponent<EnemyEarlyBird> ().retreatSpeed;
			timer = Time.time + childAttackDelay;
		}

		if (this is EnemyGryphon) {
			childAttackDelay = this.GetComponent<EnemyGryphon> ().attackDelay;
			childRetreatSpeed = this.GetComponent<EnemyGryphon> ().retreatSpeed;
			timer = Time.time + childAttackDelay;
		}
		moving = false;
		vulnerable = true;
        berserkDamage = GameObject.Find("Player").GetComponent<CharacterController>().GetBerserkDamage();
	}

	// Update is called once per frame
	protected void Update()
	{
		MoveEnemy();
		if (inflictDamage && vulnerable)
		{
			InflictDamage();
		}
		else
		{
			sprite.color = new Color(1, 1, 1);
		}
	}

    public virtual void MoveEnemy()
    {
    }

	public virtual void DisableAttacking()
	{
	}

	public void RotateToPlayer()
	{
        destinationPoint = targetPlayerPart.transform.position;
        Vector3 target = destinationPoint - transform.position;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 30);
    }

    public void AttackPlayer (float attackSpeed, GameObject enemy) {
        Vector3 tempAttackTarget = attackTarget;
		Vector3 retreatPosition = attackStartPos;

		if (!attacking && !retreating) {
            attackTarget = targetPlayerPart.transform.position;
            attackStartPos = gameObject.transform.position;
			attacking = true;
			gameObject.GetComponent<MoveOnPath> ().SetOnPath (false);
        } else if (retreating) {
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, attackStartPos, childRetreatSpeed * Time.deltaTime);

            if (gameObject.transform.position == attackStartPos) {
                retreating = false;
				moving = false;
				gameObject.GetComponent<MoveOnPath> ().SetOnPath (true);
				timer = Time.time + childAttackDelay;
            }

        } else if (attacking) {
            attackTarget = tempAttackTarget;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, attackTarget, attackSpeed * Time.deltaTime);

           if (gameObject.transform.position == attackTarget)
            {
                attacking = false;
                retreating = true;
                attackStartPos = retreatPosition;
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D collider)
    {
		if (vulnerable && (collider.gameObject.tag == "Fire" || collider.gameObject.layer == 11 || collider.gameObject.tag == "Fireball"))
        {
			inflictDamage = true;

			if (collider.gameObject.tag == "Fire" || collider.gameObject.layer == 11) {
				InflictDamage ();
			} else {
				InflictDamage (collider.GetComponent<Fireball>().damage);
				collider.gameObject.GetComponent<Fireball> ().Explode ();
			}
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
		if (collider.gameObject.tag == "Fire" || collider.gameObject.layer == 11 || collider.gameObject.layer == 8 ||collider.gameObject.tag == "Fireball")//Hope adding Player layer here didn't break anything.
        {
			if (collider.gameObject.tag == "Fireball") {
				collider.gameObject.GetComponent<Fireball> ().RemoveFireball(); //this is now smart
			}

            inflictDamage = false;
        }
    }

	private void InflictDamage() {
		InflictDamage (Time.deltaTime * berserkDamage);
	}

	private void InflictDamage(float damage)
    {
		if (CheckInArea()) {
			stamina = stamina - damage;
			sprite.color = hitColor;
		}

        if (stamina <= 0)
        {
			Die (gameObject.transform.position);
        }
    }

	public virtual void Die(Vector3 spawnPos)
    {
		Camera.main.GetComponent<CameraShake>().Shake(screenShakeDuration, screenShakeAmount);

		if (deathPrefab != null) {
			GameObject death = Instantiate (deathPrefab, transform.position, transform.rotation);
			death.GetComponent<Explosion> ().ChangeParameters (gameObject.transform);
		}

        for (int i = 0; i < powerUpAmount; i++)
        {
            RandomisePowerUps(smallDropRate, mediumDropRate, largeDropRate, spawnPos);
        }
        canvas.GetComponent<PopupController>().CreateFloathingText((score * gameManager.GetMultiplier()).ToString(), gameObject.transform);
        gameManager.IncreaseScore(score);
	    Destroy (gameObject);
    }


    private void RandomisePowerUps(float smallDropRate, float mediumDropRate, float largeDropRate, Vector3 spawnPos)
    {
        float random = Random.Range(0, (smallDropRate + mediumDropRate + largeDropRate));
        
        if (random <= smallDropRate)
        {
            Instantiate(powerUps[0], spawnPos, Quaternion.identity);
        }
        else if (random <= (mediumDropRate + smallDropRate))
        {
            Instantiate(powerUps[1], spawnPos, Quaternion.identity);
        }
        else if (random <= (largeDropRate + mediumDropRate + smallDropRate))
        {
            Instantiate(powerUps[2], spawnPos, Quaternion.identity);
        }
        else
        {

        }
    }

	public bool CheckInArea() {
		bool inArea = false;

		if (transform.position.x >= gameManager.visibleAreaWidth / 2f * (-1f) && transform.position.x <= gameManager.visibleAreaWidth / 2f 
			&& transform.position.y >= gameManager.visibleAreaHeight / 2f * (-1f) && transform.position.y <= gameManager.visibleAreaHeight / 2f) {
			inArea = true;
		}
			
		return inArea;
	}
}


