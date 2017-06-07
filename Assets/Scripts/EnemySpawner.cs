using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


    public GameObject enemyPrefab;
    private float spawntimer = 5; // 5 seconds between enemies, counting down 1 seconds per 15 seconds
	// Use this for initialization
	void Start () {
        Invoke("CreateEnemy", 5f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void CreateEnemy()
    {

        Instantiate(enemyPrefab, new Vector2(-11f, Random.Range(-5f, 5f)), Quaternion.identity);
        Invoke("CreateEnemy", spawntimer);
        if (spawntimer > 1)
        {
            spawntimer = spawntimer - Time.deltaTime * 15; //each 1 takes 15 seconds
            //Debug.Log(spawntimer);
        }
    }

}
