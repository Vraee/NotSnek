using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : EnemyController {
    public GameObject pointA;
    public GameObject pointB;
    public float shootDelay;
    public float shootTime;
    public GameObject fireBall;
    public float fireBallAmount;
    private bool attack = false;
    private bool changePos = true;
    private float cameraWidth;
    private float cameraHeight;
    private float time;
    private float time2;

    public override void MoveEnemy()
    {

    }

    new void Start()
    {
        base.Start();
        StartCoroutine(MoveToPoint());
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * Screen.width / Screen.height;
        time = shootDelay;
    }

    new void Update()
    {
        if (attack)
        {
            WideAttack();
        }
    }

    private void WideAttack()
    {
        time2 += Time.deltaTime;
        Debug.Log(time2);
        if (time2 <= shootTime)
        {
            time += Time.deltaTime;
            Debug.Log(time);
            if (time >= shootDelay)
            {
                float unit = (cameraWidth / 2) / fireBallAmount;
                for (int i = 0; i < fireBallAmount; i++)
                {
                    if (transform.position.x > 0)
                    {
                        Instantiate(fireBall, new Vector3(cameraWidth / 2 - (unit * i), transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)));
                    }
                    else if (transform.position.x < 0)
                    {
                        Instantiate(fireBall, new Vector3((-cameraWidth / 2) + (unit * i), transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)));
                    }
                }
                time = 0;
            }
        }
        else
        {
            time2 = 0;
            StartCoroutine(MoveToPoint());
        }
    }

    IEnumerator MoveToPoint()
    {
        attack = false;
        Vector3 point = new Vector3();
        if (changePos)
        {
            point = pointA.transform.position;
            changePos = false;
        }
        else
        {
            point = pointB.transform.position;
            changePos = true;
        }

        while (Vector3.Distance(transform.position, point) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, point, speed * Time.deltaTime);
            yield return null;
        }
        attack = true;
    }

}
