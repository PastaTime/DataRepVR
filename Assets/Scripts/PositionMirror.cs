using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMirror : MonoBehaviour {

	public GameObject childObject;
	public bool constrainX = false;
	public bool constrainY = true;
	public bool constrainZ = false;

	public bool constrainRotation = true;

	public bool lockRotation = true;
	private Quaternion initialRotation;

	private float cornerAlignOffset;

	void Start () {
		if (lockRotation) {
			initialRotation = this.gameObject.transform.rotation;
		}
	}

	void Update () {
		if (lockRotation) {
			this.gameObject.transform.rotation = initialRotation;
		}
		Vector3 parentPosition = this.gameObject.transform.position;
		Vector3 childPosition = childObject.transform.position;
		if (constrainX) {
			childPosition.x = parentPosition.x;
		}
		if (constrainY) {
			Debug.Log(childObject.GetComponent<PolyMeshController>().getCornerVerticalOffset());
			childPosition.y = parentPosition.y + (childObject.GetComponent<PolyMeshController>().getCornerVerticalOffset());
		}
		if (constrainZ) {
			childPosition.z = parentPosition.z;
		}
		childObject.transform.position = childPosition;

		if (constrainRotation) {
			childObject.transform.eulerAngles = transform.parent.eulerAngles;
		}
	}

}
