using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMirror : MonoBehaviour {

	public GameObject childObject;
	public bool constrainX = false;
	public bool constrainY = true;
	public bool constrainZ = false;

	// Use this for initialization
	void Start () {
		//Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
