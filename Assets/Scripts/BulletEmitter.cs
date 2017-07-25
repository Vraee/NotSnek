using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

    public GameObject fireBall;
    public float shootDelay;
    public float shootEvery;
    private GameObject targetPlayerPart;

    private void Start()
    {
        targetPlayerPart = GameObject.Find("Head");
        InvokeRepeating("Instantiate", shootDelay, shootEvery);
    }

    private void Update()
    {
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
        Instantiate(fireBall, transform.position, transform.rotation);
    }
}
