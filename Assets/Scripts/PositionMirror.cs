using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMirror : MonoBehaviour {

	public GameObject childObject;
	public bool constrainX = false;
	public bool constrainY = true;
	public bool constrainZ = false;

	public bool contrainRotation = true;

	public bool lockRotation = true;
	private Quaternion initialRotation;

	void Start () {
		if (lockRotation) {
			initialRotation = this.gameObject.transform.rotation;
		}
	}

	void Update () {
		if (lockRotation && initialRotation != null) {
			this.gameObject.transform.rotation = initialRotation;
		}
		Vector3 parentPosition = this.gameObject.transform.position;
		Vector3 childPosition = childObject.transform.position;
		if (constrainX) {
			childPosition.x = parentPosition.x;
		}
		if (constrainY) {
			childPosition.y = parentPosition.y;
		}
		if (constrainZ) {
			childPosition.z = parentPosition.z;
		}
		childObject.transform.position = childPosition;


		if (contrainRotation) {
			childObject.transform.eulerAngles = transform.parent.eulerAngles;
		}
	}

}
