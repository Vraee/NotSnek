using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPart : EnemyController {

    private EnemySkeletonSnake parentScript;
    private int listIndex;
    public float bodyPartHP;
    private bool isHit;
    private GameObject enemy;

    public override void MoveEnemy()
    {
        base.MoveEnemy();
    }

    // Use this for initialization
    new void Start()
    {
        parentScript = GetComponentInParent<EnemySkeletonSnake>();
        bodyPartHP = parentScript.GetBodyPartHP();
        isHit = false;
        StartCoroutine(Scale());
    }


    // Update is called once per frame
    new void Update () {

	}

    IEnumerator Scale()
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0), new Vector3(1f, 1f, 0), progress);
            progress += Time.deltaTime;
            yield return null;
        }
    }
    
}
