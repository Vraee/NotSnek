using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagPathCreator : MonoBehaviour {
	public int pathObjectsAmount;
	public float distanceBetweenPoints;
	public float wavelength;
	public bool horizontal;
	public bool flipVertical;
	public bool flipHorizontal;

	private Vector3 prevPathObjectPosition;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < pathObjectsAmount; i++) {
			Vector3 position;
			float x;
			float y;

			if (i == 0) {
				position = transform.position;
			} else if (horizontal) {
				x = prevPathObjectPosition.x + distanceBetweenPoints;
				y = prevPathObjectPosition.y + wavelength;

				if (flipVertical) 
					x = prevPathObjectPosition.x - distanceBetweenPoints;
				if (flipHorizontal) 
					y = prevPathObjectPosition.y - wavelength;

				position = new Vector3 (x, y, 0);

			} else {
				x = prevPathObjectPosition.x + wavelength;
				y = prevPathObjectPosition.y + distanceBetweenPoints;

				if (flipVertical)
					y = prevPathObjectPosition.y - distanceBetweenPoints;
				if (flipHorizontal)
					x = prevPathObjectPosition.x - wavelength;

				position = new Vector3 (x, y, 0);
			}

			distanceBetweenPoints = -distanceBetweenPoints;

			GameObject pathObject = new GameObject ();
			pathObject.transform.parent = this.transform;
			pathObject.transform.position = position;
			prevPathObjectPosition = position;
		}
	}
}
