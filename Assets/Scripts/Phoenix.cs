using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : EnemyController {
    public float startSpeed;
    public GameObject pointA;
    public GameObject pointB;
    public float shootDelay;
    public float shootTime;
    public GameObject fireBall;
    public float fireBallAmount;

	private BackgroundScroller backgroundScroller;

    private bool attack = false;
    private bool changePos = true;
    private float cameraWidth;
    private float cameraHeight;
    private float hp;
    private float time;
    private float time2;
    private BulletEmitter emitter;

    public override void MoveEnemy()
    {

    }

    new void Start()
    {
        base.Start();
        StartCoroutine(MoveToPoint(startSpeed));
        Camera.main.GetComponent<CameraShake>().Shake(4, 0.1f);
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * Screen.width / Screen.height;
        hp = stamina;
        emitter = GetComponentInChildren<BulletEmitter>();
        time = shootDelay;
		backgroundScroller = GameObject.Find("BackgroundController").GetComponent<BackgroundScroller>();
    }

    new void Update()
    {
        base.Update();
        if (attack)
        {
            WideAttack();
        }
        if(stamina <= hp/2)
        {
            emitter.MakeAngry();
        }
    }

    private void WideAttack()
    {
        time2 += Time.deltaTime;
        if (time2 <= shootTime)
        {
            time += Time.deltaTime;
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
            StartCoroutine(MoveToPoint(speed));
        }
    }

    IEnumerator MoveToPoint(float movementSpeed)
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
            transform.position = Vector3.MoveTowards(gameObject.transform.position, point, movementSpeed * Time.deltaTime);
            yield return null;
        }
        attack = true;
    }

	public override void Die (Vector3 spawnPos) {
		backgroundScroller.GetComponent<BackgroundScroller> ().SetBossDead (true);
		base.Die(spawnPos);
	}
}
