using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyType;
    public float speed;
    public float stamina;
    public GameObject powerUpPrefab;
	public GameObject player;

    private SpriteRenderer sprite;
    private Color hitColor = Color.red;
    private int dir = 0;
    private bool inflictDamage;
	private bool attacking;

	private Vector3 destinationPoint;


    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
		attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
		destinationPoint = player.transform.position;
		RotateToPlayer ();

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

    void MoveEnemy()
    {
        //gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right*speed*Time.deltaTime);
		MoveEarlyBird();

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
    }

	private void RotateToPlayer()
	{
		Quaternion rotation = Quaternion.LookRotation (gameObject.transform.position - destinationPoint, Vector3.forward);
		gameObject.transform.rotation = rotation;
		gameObject.transform.eulerAngles = new Vector3 (0, 0, gameObject.transform.eulerAngles.z);
	}

	IEnumerator AttackPlayer (float delay, float attackSpeed) {
		if (!attacking) {
			attacking = true;
			yield return new WaitForSeconds (delay);
			gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, player.transform.position, attackSpeed * Time.deltaTime);
			attacking = false;
		}
	}


	public void MoveEye() {

	}

	public void MoveEarlyBird() {
		RotateToPlayer ();
		StartCoroutine (AttackPlayer (1f, 50f));
	}

	public void MoveGriffin() {

	}

    public void OnTriggerEnter2D(Collider2D collider)
    {
		//Debug.Log ("mo");
        if(collider.gameObject.tag == "Fire")
        {
            inflictDamage = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Fire")
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
}
