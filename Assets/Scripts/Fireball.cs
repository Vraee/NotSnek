using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public bool shoot;
    public float damage;
    public GameObject explosion;
	public GameObject player;
	public float damageIncrease;
	private ParticleSystem particlesystem;


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
        //	if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1"))
        //	{
        //Shoot();
        //	}

        CheckLocation();
            gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);
        

        if (gameObject.transform.position.x > 20 || gameObject.transform.position.y > 20 || gameObject.transform.position.x < -20 || gameObject.transform.position.y < -20)
        {
			RemoveFireball();
        }

		/*if ((Input.GetMouseButton(0) || Input.GetButton("Fire1")) && !shoot)
		{
			transform.position = player.transform.position + (player.transform.up * 1);
			transform.rotation = player.transform.rotation;

			//Increase fireball size and damage when mouse is held
			if (transform.localScale.x <= 10)
			{
				transform.localScale += new Vector3(0.25f * Time.deltaTime, 0.25f * Time.deltaTime, 0.25f * Time.deltaTime);
				//fireballDamage = fireballDamage + damageIncrease * Time.deltaTime;
				IncreaseDamage(damageIncrease * Time.deltaTime);
			}
		}
        */
		
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
		//var main = particlesystem.main;
		//main.simulationSpace = ParticleSystemSimulationSpace.World;
	}

	public void RemoveFireball()
	{
		if (particlesystem != null) {
			var emission = particlesystem.emission;
			emission.enabled = false;
		} 
		Invoke("Destroy", 2f);

	}
	private void Destroy()
	{
		Destroy(gameObject);
	}


}
