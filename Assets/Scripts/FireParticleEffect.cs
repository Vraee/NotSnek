using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticleEffect : MonoBehaviour {


    public GameObject particlePrefab;
    public float spawnRate = 10; //per sec 
    private float timeSinceLastSpawn = 0;
    PolygonCollider2D col;

    // Use this for initialization
    void Start () {
		
	}
	// Update is called once per frame
	void FixedUpdate () {
        col = GetComponent<PolygonCollider2D>();
        timeSinceLastSpawn += Time.deltaTime;
        float correctTimeBetweenSpawns = 1f / spawnRate;
        while(timeSinceLastSpawn > correctTimeBetweenSpawns)
        {
            //Time to spawn a particle
            SpawnFireAlongOutline();
            timeSinceLastSpawn -= correctTimeBetweenSpawns;
        }
	}

    void SpawnFireAlongOutline()
    {

        int pathIndex = Random.Range(0, col.pathCount);
        Vector2 [] points = col.GetPath(pathIndex);
        int pointIndex = Random.Range(0, points.Length);
        Vector2 pointA = points[pointIndex];
        Vector2 pointB = points[(pointIndex +1) % points.Length];
        Vector2 spawnPoint = Vector2.Lerp(pointA, pointB, Random.Range(0f,1f));
        SpawnFireAtPosition(spawnPoint + (Vector2)this.transform.position);
    }

    void SpawnFireAtPosition(Vector2 position)
    {
        Instantiate(particlePrefab, position, Quaternion.identity);
    }
}
