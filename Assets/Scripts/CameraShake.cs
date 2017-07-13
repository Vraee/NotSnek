using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Vector3 originalPos;
    public static CameraShake _instance;

    // Use this for initialization
    void Start () {

        originalPos = this.transform.localPosition;
        _instance = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shake(float duration, float amount)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.cShake(duration, amount));

    }

    private IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;
        while(Time.time < endTime)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * amount;
            duration -= Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}