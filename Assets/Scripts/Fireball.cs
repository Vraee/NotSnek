using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public float amplitude;
    public bool shoot;
    public bool sideToSide;
    public float damage;
    public GameObject explosion;
	public GameObject player;
	public float damageIncrease;
	private ParticleSystem particlesystem;

    private float lifeTime;
    private Camera cam;
    private float viewHeight;
    private float viewWidth;

    // Use this for initialization
    void Start()
    {
		//Debug.Log("Ya'll need some jeesus");
		//damage = GameObject.Find("Player").GetComponent<CharacterController>().fireballDamage;
		particlesystem = GetComponent<ParticleSystem>();
        cam = Camera.main;
        viewHeight = 2f * cam.orthographicSize;
        viewWidth = viewHeight * cam.aspect;

    }

    // Update is called once per frame
    void Update()
    {
        CheckLocation();
        gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);
        if (sideToSide)
        {
            lifeTime += Time.deltaTime;
            gameObject.transform.localPosition = new Vector2(transform.localPosition.x + Mathf.Sin(lifeTime) * amplitude * Time.deltaTime, transform.localPosition.y);
        }
    }

	//This is called in the enemy's OnTriggerEnter2D; Destroy() is called in the enemy's OnTriggerExit2D
	public void Explode() {
		GameObject expl = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
		expl.GetComponent<Explosion>().ChangeParameters(gameObject.transform);
	}

    public void IncreaseDamage(float damage)
    {
        this.damage = this.damage + damage;
    }

    private void CheckLocation()
    {
        if (gameObject.transform.position.y >= viewHeight / 2)
        {
            RemoveFireball();
        }
        if (gameObject.transform.position.y <= -viewHeight / 2)
        {
            RemoveFireball();
        }
        if (gameObject.transform.position.x >= viewWidth / 2)
        {
            RemoveFireball();
        }
        if (gameObject.transform.position.x <= -viewWidth / 2)
        {
            RemoveFireball();
        }

    }

    public void Shoot()
    {
        shoot = true;
	}

	public void RemoveFireball()
	{
		var emission = particlesystem.emission;
		emission.enabled = false;
		Invoke("Destroy", 2f);

	}
	private void Destroy()
	{
		Destroy(gameObject);
	}


}
