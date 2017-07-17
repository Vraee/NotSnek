using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPart : EnemyController {

    private EnemySkeletonSnake parentScript;
    private int listIndex;
    public float bodyPartHP;
    private bool isHit;
    private GameObject enemy;
	private Vector3 startPos;
	private float index;
	private float startX;
	private float startY;

    public override void MoveEnemy()
    {
        base.MoveEnemy();
    }

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
        return enemy;
    }

    public void SetEnemy(GameObject newEnemy)
    {
        enemy = newEnemy;
    }

	public Vector3 GetStartPos() {
		return startPos;
	}

	public void SetStartPos(Vector3 startPos) {
		this.startPos = startPos;
	}

	public float GetIndex() {
		return index;
	}

	public void SetIndex(float index) {
		this.index = index;
	}

	public float GetStartX()
	{
		return startX;
	}

	public void SetStartX(float startX)
	{
		this.startX = startX;
	}

	public float GetStartY()
	{
		return startY;
	}

	public void SetStartY(float startY)
	{
		this.startY = startY;
	}

    // Use this for initialization
    new void Start()
    {
        parentScript = GetComponentInParent<EnemySkeletonSnake>();
        bodyPartHP = parentScript.GetBodyPartHP();
        isHit = false;
        StartCoroutine(Scale());
    }


    // Update is called once per frame
    new void Update () {
   
	}

    new void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Fireball")
        {
            enemy = collider.gameObject;
            parentScript.StartCoroutine("EnemyDamage", this);
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
