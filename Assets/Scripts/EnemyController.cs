using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int EnemyType;
    public float speed;
    public float stamina;
    public GameObject powerUpPrefab;
    private SpriteRenderer sprite;
    private Color hitColor = Color.red;
    private int dir = 0;
    private bool inflictDamage;

    // Use this for initialization
    void Start()
    {

        sprite = GetComponent<SpriteRenderer>();
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
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
		Debug.Log ("mo");
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
		Debug.Log (stamina);
        sprite.color = hitColor;
        if (stamina <= 0)
        {
            Instantiate(powerUpPrefab,transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    
}
