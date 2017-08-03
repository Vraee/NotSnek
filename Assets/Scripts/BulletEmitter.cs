using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour {

    public GameObject fireBall;
	public bool shootAtPlayer;
	public bool circleWaves;
    public float shootEvery;
	public float circleWavesEvery;

    private float shootTime;
	private float circleWaveTime;
    private bool angry = false;
    private GameObject targetPlayerPart;
	private Phoenix phoenix;

	private List<GameObject> fireballsAll;
	private List<GameObject> fireballsCircle;

    public void MakeAngry()
    {
        angry = true;
    }

    private void Start()
    {
        targetPlayerPart = GameObject.Find("Head");
		fireballsCircle = new List<GameObject> ();
		fireballsAll = new List<GameObject> ();
		phoenix = transform.GetComponentInParent<Phoenix> ();
    }

    private void Update()
    {
		if (phoenix.CheckInArea ()) {
			if (angry) {
				shootTime += (Time.deltaTime * 2);
				circleWaveTime += (Time.deltaTime * 2);
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
		float radius = 0f;
		Vector3 pos = transform.position;
		float startAngle = 0;
		float endAngle = 360;

		for (var i = 0; i < 30; i++) {
			GameObject newFireball;
			//.~.*~*majig*~*.~.
			float angle = i * Mathf.PI * 2 / 30;
			//Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0) * radius;
			pos.x += Mathf.Cos (angle) * radius;
			pos.y += Mathf.Sin (angle) * radius;

			if (angle * (180 / Mathf.PI) >= startAngle && angle * (180 / Mathf.PI) <= endAngle) {
				newFireball = Instantiate (fireBall);
				newFireball.GetComponent<Fireball> ().ownMovingMethod = true;
				newFireball.transform.localPosition = pos;
				newFireball.transform.rotation = transform.rotation;
				fireballsCircle.Add (newFireball);
				fireballsAll.Add (newFireball);
			}
		}
	}


	private void MoveFireballs() {
		float radius = 0.5f;
		for (int i = 0; i < fireballsCircle.Count; i++) {
			if (fireballsCircle [i] != null) { 
				float angle = i * Mathf.PI * 2 / 30;
				//Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0) * radius;

				Vector3 pos = fireballsCircle [i].transform.position;
				pos.x += Mathf.Cos (angle) * Time.deltaTime * 5f;
				pos.y += Mathf.Sin (angle) * Time.deltaTime * 5f;

				fireballsCircle [i].transform.localPosition = pos;
			}
		}
	}
}
