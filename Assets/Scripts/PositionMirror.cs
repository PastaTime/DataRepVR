using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMirror : MonoBehaviour {

	public GameObject childObject;
	public bool constrainX = false;
	public bool constrainY = true;
	public bool constrainZ = false;

	public bool lockRotation = true;
	private Quaternion initialRotation;

	private Vector2 initialArcPosition; /// <summary>
	/// Heeeey Y = Z matey 
	/// </summary>

	void Start () {
		if (lockRotation) {
			initialRotation = this.gameObject.transform.rotation;
		}
			
		initialArcPosition = ArcPosition (this.gameObject.transform.position);
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

		//rotateArc ();
	}

	void rotateArc() {
		Vector2 currentArcPosition = ArcPosition (this.gameObject.transform.position);

		float angle = Vector2.Angle (currentArcPosition, initialArcPosition);
		Vector3 rotation = new Vector3 (0, angle, 0);
		this.gameObject.transform.parent.Rotate (rotation);
	}

	private Vector2 ArcPosition(Vector3 spherePosition) {
		Vector3 origin = Vector3.zero;
		Vector3 sphereArcOrientation = origin - spherePosition;

		Vector2 arcOrientationFlat;
		arcOrientationFlat.x = sphereArcOrientation.x;
		arcOrientationFlat.y = sphereArcOrientation.z;

		return arcOrientationFlat;
	}
		
}
