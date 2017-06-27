using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
	public float retreatSpeed;
    public float stamina = 5;
	public float damageOutput;
	public float attackDelay = 2;
    public GameObject powerUpPrefab;
	public GameObject targetPlayerPart;

    private SpriteRenderer sprite;
    private Color hitColor = Color.red;
    private int dir = 0;
    private bool inflictDamage;
	private bool attacking;
    private bool retreating;
	private Vector3 destinationPoint;
    private Vector3 attackTarget;
    private Vector3 enemyStartPos;
	private float timer;
	private bool moving;
	private bool vulnerable;
	private GameManager gameManager;

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
		return vulnerable;
	}

	public void SetVulnerable(bool vulnerable) {
		this.vulnerable = vulnerable;
	}

	// Use this for initialization
	public virtual void Start()
	{
		targetPlayerPart = GameObject.Find ("Head");
		sprite = GetComponent<SpriteRenderer>();
		attacking = false;
		retreating = false;
		enemyStartPos = gameObject.transform.position;

		timer = Time.time + attackDelay;
		moving = false;
		vulnerable = true;
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update()
	{
		MoveEnemy();

		if (inflictDamage && vulnerable)
		{

			InflictDamage();
		}
		else
		{
			//Debug.Log (inflictDamage);

			sprite.color = new Color(1, 1, 1);
		}
	}

    public virtual void MoveEnemy()
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
		Vector3 retreatPosition = enemyStartPos;

		if (!attacking && !retreating) {
            attackTarget = targetPlayerPart.transform.position;
            enemyStartPos = gameObject.transform.position;
			attacking = true;
			gameObject.GetComponent<MoveOnPath> ().SetOnPath (false);
        } else if (retreating) {
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, enemyStartPos, retreatSpeed * Time.deltaTime);

            if (gameObject.transform.position == enemyStartPos) {
                retreating = false;
				moving = false;
				gameObject.GetComponent<MoveOnPath> ().SetOnPath (true);
				timer = Time.time + attackDelay;
            }

        } else if (attacking) {
            attackTarget = tempAttackTarget;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, attackTarget, attackSpeed * Time.deltaTime);

           if (gameObject.transform.position == attackTarget)
            {
                attacking = false;
                retreating = true;
                enemyStartPos = retreatPosition;
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
		if (collider.gameObject.tag == "Fire" || collider.gameObject.layer == 11 || collider.gameObject.tag == "Fireball")
        {
			if (collider.gameObject.tag == "Fireball") {
				Destroy (collider.gameObject); //hope this is smart
			}

            inflictDamage = false;
        }
    }

	private void InflictDamage() {
		InflictDamage (Time.deltaTime);
	}

	private void InflictDamage(float damage)
    {
		//Debug.Log ("stamina: " + stamina + " damage: " + damage);
		stamina = stamina - damage;
		//Debug.Log ("new stamina: " + stamina + " damage: " + damage);

        sprite.color = hitColor;
        if (stamina <= 0)
        {
			Die ();
        }
    }
    private void Die()
    {
        Instantiate(powerUpPrefab, transform.position, transform.rotation);
		gameManager.IncreaseScore (1);
        Destroy(gameObject);
    }
}


