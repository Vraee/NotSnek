using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

    public GameObject fireBall;
	public bool shootAtPlayer;
	public bool circleWaves;
    public float shootEvery;
	public float circleWavesEvery;
	public int amountOfBulletsCircle;
	public bool increaseCircleSpawnSpeed;

    private float shootTime;
	private float circleWaveTime;
    private bool angry = false;
    private bool firing = false;
    private ParticleSystem particlesystem;
    private GameObject targetPlayerPart;
	private Phoenix phoenix;
	private Color32 fireballAltColor = new Color32(255, 182, 103, 255);

	private List<GameObject> fireballsAll;
	private List<GameObject> fireballsCircle;

    public void MakeAngry()
    {       
        var shaper = particlesystem.shape;
        var lifetime = particlesystem.main;
        shaper.radius = 9;
        lifetime.duration = 0.35f;
        lifetime.startLifetime = 0.4f;
        angry = true;
    }

    private void Start()
    {
        targetPlayerPart = GameObject.Find("Head");
		fireballsCircle = new List<GameObject> ();
		fireballsAll = new List<GameObject> ();
		phoenix = transform.GetComponentInParent<Phoenix> ();
        particlesystem = GetComponent<ParticleSystem>();
        particlesystem.Stop();
    }

    private void Update()
    {
		if (phoenix.CheckInArea ()) {

            if ((shootTime==0 && shootAtPlayer)&&!firing) {
                particlesystem.Play();
                firing = true;
            }
            
            if (angry) {
				shootTime += (Time.deltaTime * 2);

				if (increaseCircleSpawnSpeed) {
					circleWaveTime += (Time.deltaTime * 2);
				} else {
					circleWaveTime += Time.deltaTime;
				}
			} else {
				shootTime += Time.deltaTime;
				circleWaveTime += Time.deltaTime;
			}

			if (shootTime >= shootEvery && shootAtPlayer) {
				Instantiate ();
				shootTime = 0;
			}

			if (circleWaves) {

				if (circleWaveTime >= circleWavesEvery) {
					ShootFireballsWave ();
					circleWaveTime = 0;
				}

				MoveFireballs ();
			}

			RotateToPlayer ();
		}
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
			GameObject newFireball;
			newFireball = Instantiate(fireBall, pos, transform.rotation);
			fireballsAll.Add (newFireball);
        }
    }

	private void ShootFireballsWave() {
		float radius = 0.5f;
		Vector3 pos = transform.position;
		float startAngle = 0;
		float endAngle = 360;

		for (var i = 0; i < amountOfBulletsCircle; i++) {
			GameObject newFireball;
			//.~.*~*majig*~*.~.
			float angle = i * Mathf.PI * 2 / amountOfBulletsCircle;
			//Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0) * radius;
			pos.x += Mathf.Cos (angle) * radius;
			pos.y += Mathf.Sin (angle) * radius;

			if (angle * (180 / Mathf.PI) >= startAngle && angle * (180 / Mathf.PI) <= endAngle) {
				newFireball = Instantiate (fireBall);
				newFireball.GetComponent<Fireball> ().ownMovingMethod = true;
				newFireball.GetComponent<SpriteRenderer> ().color = fireballAltColor;
				newFireball.GetComponent<SpriteRenderer> ().sortingOrder = -1;
				newFireball.GetComponent<ParticleSystemRenderer> ().sortingOrder = -1;
				newFireball.GetComponent<Fireball> ().damage = newFireball.GetComponent<Fireball> ().damage / 2f;
				newFireball.transform.localPosition = pos;
				newFireball.transform.rotation = transform.rotation;
				fireballsCircle.Add (newFireball);
				fireballsAll.Add (newFireball);
			}
		}
	}


	private void MoveFireballs() {
		for (int i = 0; i < fireballsCircle.Count; i++) {
			if (fireballsCircle [i] != null) { 
				float angle = i * Mathf.PI * 2 / amountOfBulletsCircle;
				//Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0) * radius;

				Vector3 pos = fireballsCircle [i].transform.position;
				pos.x += Mathf.Cos (angle) * Time.deltaTime * 5f;
				pos.y += Mathf.Sin (angle) * Time.deltaTime * 5f;

				fireballsCircle [i].transform.localPosition = pos;
			}
		}
	}

	public void DestroyAllFirballs() {
		foreach (GameObject listFireball in fireballsAll) {
			if (listFireball != null) {
				listFireball.GetComponent<Fireball> ().Explode ();
				Destroy (listFireball);
			}
		}
	}
}
