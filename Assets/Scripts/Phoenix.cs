using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : EnemyController {
    public float startSpeed;
    public GameObject pointA;
    public GameObject pointB;
    public float shootDelay;
    public float shootTime;

	public GameObject FlameWLeft;
	public GameObject FlameWRight;

	[HideInInspector]
	public BackgroundScroller backgroundScroller;

    private bool attack = false;
    private bool changePos = true;
    private float cameraWidth;
    private float cameraHeight;
    private float hp;
    private float time;
    private float time2;
    private BulletEmitter emitter;

	private Component[] children;

	private ParticleSystem GetSystem(string systemName)
	{
		foreach (ParticleSystem childParticleSystem in children)
		{
			if (childParticleSystem.name == systemName)
			{
				return childParticleSystem;
			}
		}
		return null;
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

		children = GetComponentsInChildren<ParticleSystem>();
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
			Enrage();
        }

		//Debug.Log (stamina);
    }

    private void WideAttack()
    {
		var right_wing = GetSystem("Right").main;
		var left_wing = GetSystem("Left").main;

		time2 += Time.deltaTime;
        if (time2 <= shootTime)
        {
			right_wing.gravityModifier = -1 + shootTime *2-time2*2;
			left_wing.gravityModifier = -1 + shootTime *2-time2*2;
			//Debug.Log(-1+shootTime * 2 - time2 * 2);

			time += Time.deltaTime;
            if (time >= shootDelay)
            {
				/* antti's stuff. Most likely not used anymore
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
				New and improved stuff by maximillian*/


				
				if (transform.position.x > 0)
				{
					Instantiate(FlameWLeft, new Vector3(cameraWidth / 4, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)));
					Instantiate(FlameWRight, new Vector3((cameraWidth / 4), transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)));
				}
				else if (transform.position.x < 0)
				{
					Instantiate(FlameWLeft, new Vector3((-cameraWidth / 4), transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)));
					Instantiate(FlameWRight, new Vector3((-cameraWidth / 4), transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)));
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

	private void Enrage()
	{
		emitter.MakeAngry();
		

		//Intensify boss!
		var right_eye = GetSystem("Eye_right").main;
		var left_eye = GetSystem("Eye_left").main;
		var right_wing = GetSystem("Right").main;
		var left_wing = GetSystem("Left").main;
		
		right_eye.startLifetime = 1.2f;
		left_eye.startLifetime = 1.2f;
		right_wing.startLifetime = 1.2f;
		left_wing.startLifetime = 1.2f;


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
		GetComponentInChildren<BulletEmitter> ().DestroyAllFirballs ();
		Destroy (this.transform.parent.gameObject);
		base.Die(spawnPos);
	}
}
