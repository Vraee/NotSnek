using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Vector3 originalPos;
    public static CameraShake _instance;
    private bool shaking;

    // Use this for initialization
    void Start () {
        shaking = false;
        originalPos = this.transform.localPosition;
        _instance = this;
    }

    public void SetShake(bool set)
    {
        shaking = set;
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
        float timer = 0;
        //float endTime = Time.time + duration;
        while(timer < duration)
        {
            if (shaking) {
                transform.localPosition = originalPos + Random.insideUnitSphere * amount;
                timer += Time.deltaTime;
            }
            yield return null;
        }

        transform.localPosition = originalPos;
        shaking = false;
    }
}