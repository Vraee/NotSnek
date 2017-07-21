using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : EnemyController {
    public GameObject pointA;
    public GameObject pointB;
    public float shootDelay;
    public GameObject fireBall;
    public float fireBallAmount;
    private float lastFireTime;
    private bool attack = false;
    private float cameraWidth;
    private float cameraHeight;

    public override void MoveEnemy()
    {

    }

    new void Start()
    {
        base.Start();
        StartCoroutine(MoveToPoint(pointA.transform.position));
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * Screen.width / Screen.height;
    }

    new void Update()
    {
        if (attack)
        {
            if (Time.time > (lastFireTime + shootDelay))
            {
                AttackPatternOne();
                lastFireTime = Time.time;
            }
        }
    }

    private void AttackPatternOne()
    {

        float unit = (cameraWidth / 2) / fireBallAmount;
        for(int i = 0; i < fireBallAmount; i++)
        {
            if(transform.position.x > 0)
            {
                Instantiate(fireBall, new Vector3(cameraWidth/2 - (unit * i), transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0,0,180)));
            }
            else if(transform.position.x < 0)
            {
                Instantiate(fireBall, new Vector3((-cameraWidth / 2) + (unit * i), transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)));
            }
        }
    }

    IEnumerator MoveToPoint(Vector3 point)
    {
        while (Vector3.Distance(transform.position, point) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, point, speed * Time.deltaTime);
            yield return null;
        }
        attack = true;
    }

}
