using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public bool shoot;
    public float damage;

    // Use this for initialization
    void Start()
    {

        damage = GameObject.Find("Player").GetComponent<CharacterController>().fireballDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot) {
            gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseDamage(float damage)
    {
        this.damage = this.damage + damage;
    }

    public void Shoot()
    {
        shoot = true;
    }
}
