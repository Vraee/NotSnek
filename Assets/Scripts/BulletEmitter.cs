using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

    public GameObject fireBall;
    public float shootEvery;
    private float shootTime;
    private bool angry = false;
    private GameObject targetPlayerPart;

    public void MakeAngry()
    {
        angry = true;
    }

    private void Start()
    {
        targetPlayerPart = GameObject.Find("Head");
    }

    private void Update()
    {
        if (angry)
        {
            shootTime += (Time.deltaTime * 2);
        }
        else
        {
            shootTime += Time.deltaTime;
        }

        if(shootTime >= shootEvery)
        {
            Instantiate();
            shootTime = 0;
        }
        RotateToPlayer();
    }

    public void RotateToPlayer()
    {
        Vector3 target = targetPlayerPart.transform.position - transform.position;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 30);
    }

    private void Instantiate()
    {
        if (gameObject.transform.position.y <= (2f * Camera.main.orthographicSize) / 2) {
            Vector3 pos = transform.position;
            pos.y += Mathf.Sin(Time.time * 180) * 0.5f;
            Instantiate(fireBall, pos, transform.rotation);
        }
    }
}
