using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagPathCreator : MonoBehaviour {
	public int pathObjectsAmount;
	public float distanceBetweenPoints;
	public float wavelength;
	public bool horizontal;
	public bool flipVertical;

	private Vector3 prevPathObjectPosition;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < pathObjectsAmount; i++) {
			Vector3 position;

			if (i == 0) {
				position = transform.position;
			} else if (horizontal) {
				if (!flipVertical)
					position = new Vector3 (prevPathObjectPosition.x + distanceBetweenPoints, prevPathObjectPosition.y + wavelength, 0);
				else
					position = new Vector3 (prevPathObjectPosition.x - distanceBetweenPoints, prevPathObjectPosition.y + wavelength, 0);
			} else {
				if (!flipVertical)
					position = new Vector3 (prevPathObjectPosition.x + wavelength, prevPathObjectPosition.y + distanceBetweenPoints, 0);
				else
					position = new Vector3 (prevPathObjectPosition.x - wavelength, prevPathObjectPosition.y + distanceBetweenPoints, 0);
			}

			distanceBetweenPoints = -distanceBetweenPoints;

			GameObject pathObject = new GameObject ();
			pathObject.transform.parent = this.transform;
			pathObject.transform.position = position;
			prevPathObjectPosition = position;
		}
	}
}
