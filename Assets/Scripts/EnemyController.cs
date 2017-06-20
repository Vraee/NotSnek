using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyType;
    public float speed;
    public float stamina;
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

    // Use this for initialization
    void Start()
    {
        targetPlayerPart = GameObject.Find("Player/Head");
        sprite = GetComponent<SpriteRenderer>();
		attacking = false;
        retreating = false;
        enemyStartPos = gameObject.transform.position;

		timer = Time.time + attackDelay;
		moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
        if (inflictDamage)
        {
            InflictDamage();
        }
        else
        {
            sprite.color = new Color(1, 1, 1);
        }
        
    }

	public float GetDamageOutput() {
		return damageOutput;
	}

	public void SetDamageOutput(int newDamageOutput) {
		damageOutput = newDamageOutput;
	}

    void MoveEnemy()
    {
        //gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right*speed*Time.deltaTime);
        if (EnemyType == 0)
        {
            if (dir == 0)
            {
                gameObject.GetComponent<Transform>().Translate(Vector2.right * speed * Time.deltaTime);
                if (gameObject.transform.position.x > 11)
                {
                    dir = 1;
                }
            }
            if (dir == 1)
            {
                gameObject.GetComponent<Transform>().Translate(Vector2.left * speed * Time.deltaTime);
                if (gameObject.transform.position.x < -11)
                {
                    dir = 0;
                }
            }
        }

        if (EnemyType == 1)
        {
            MoveEarlyBird();
        }
    }

	private void RotateToPlayer()
	{
        destinationPoint = targetPlayerPart.transform.position;
        //Quaternion rotation = Quaternion.LookRotation(gameObject.transform.position - destinationPoint, Vector3.forward);
        //gameObject.transform.rotation = rotation;
        //gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z);

        Vector3 target = destinationPoint - transform.position;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 30);
	}

	public void AttackPlayer (float attackSpeed) {
        Vector3 tempAttackTarget = attackTarget;
		Vector3 retreatPosition = enemyStartPos;

		if (!attacking && !retreating) {
            attackTarget = targetPlayerPart.transform.position;
            enemyStartPos = gameObject.transform.position;
			attacking = true;
        } else if (retreating) {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, enemyStartPos, attackSpeed / 4 * Time.deltaTime);

            if (gameObject.transform.position == enemyStartPos) {
                retreating = false;
				moving = false;
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

	public void MoveEye() {

	}

	public void MoveEarlyBird() {
		if (!attacking) {
			RotateToPlayer ();
		}

		if (Time.time >= timer) {
			moving = true;
		}

		if (moving && gameObject.GetComponent<MoveOnPath>().GetPathReached()) {
			AttackPlayer (speed);
		}

	}

	public void MoveGriffin() {

	}
    public void OnTriggerEnter2D(Collider2D collider)
    {
		if (collider.gameObject.tag == "Fire" || collider.gameObject.layer == 11)
        {
            inflictDamage = true;
        }
        if (collider.gameObject.tag == "Fireball")
        {
            stamina = stamina - collider.GetComponent<Fireball>().damage;
            if (stamina <= 0)
            {
                Die();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
		if (collider.gameObject.tag == "Fire" || collider.gameObject.layer == 11)
        {
            inflictDamage = false;
        }
    }

    private void InflictDamage()
    {
        stamina = stamina - Time.deltaTime;
		//Debug.Log (stamina);
        sprite.color = hitColor;
        if (stamina <= 0)
        {
            Instantiate(powerUpPrefab,transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void Die()
    {
        Instantiate(powerUpPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}


