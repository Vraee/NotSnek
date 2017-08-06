using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPart : MonoBehaviour {

    /*private EnemySkeletonSnake parentScript;
    private int listIndex;
    public float bodyPartHP;
    private bool isHit;
    private GameObject enemy;*/

	private EnemySkeletonSnake parentScript;
	private int listIndex;
	private bool isHit;
	private bool takeDamage;
	private GameObject playerFire;

    public bool GetIsHit()
    {
        return isHit;
    }

    public void SetIsHit(bool newIsHit)
    {
        isHit = newIsHit;
    }

    public GameObject GetEnemy()
    {
		return playerFire;
    }

	public void SetEnemy(GameObject playerFire)
    {
		this.playerFire = playerFire;
    }

    // Use this for initialization
    void Start()
    {
        parentScript = GetComponentInParent<EnemySkeletonSnake>();
        isHit = false;
        StartCoroutine(Scale());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Fireball")
        {
			playerFire = collider.gameObject;
            parentScript.StartCoroutine("PlayerDamage", this);
        }
    }

    IEnumerator Scale()
    {
        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0), new Vector3(1f, 1f, 0), progress);
            progress += Time.deltaTime;
            yield return null;
        }
    }
    
}
