using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePathCreator : MonoBehaviour {
	public int pathObjectsAmount = 10;
	public float radius = 5;
	public float startAngle;
	public float endAngle;

	// Use this for initialization
	void Start () {
		if (endAngle == 0) {
			endAngle = 360;
		}
		
		for (var i = 0; i < pathObjectsAmount; i++) {
			//.~.*~*majig*~*.~.
			float angle = i * Mathf.PI * 2 / pathObjectsAmount;
			Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0) * radius;

			if (angle * (180 / Mathf.PI) >= startAngle && angle * (180 / Mathf.PI) <= endAngle) {
				GameObject pathObject = new GameObject ();
				pathObject.transform.parent = this.transform;
				pathObject.transform.localPosition = position;
			}
		}
	}
}