using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public int EnemyType;
    public GameObject enemyPrefab;
    public float speed;
    private int dir = 0;


	// Use this for initialization
	void Start () {       
        //Invoke("CreateEnemy", 1f);
	}
	
	// Update is called once per frame
	void Update () {
        MoveEnemy();
	}

    void MoveEnemy()
    {
        //gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right*speed*Time.deltaTime);

        if (EnemyType==0) {
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
    void CreateEnemy()
    {
        //Instantiate(enemyPrefab, new Vector2(-11f, Random.Range(-5f,5f)), Quaternion.identity);
    }
}
