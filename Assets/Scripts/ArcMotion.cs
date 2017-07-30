using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcMotion : MonoBehaviour {


	public Vector3 centreOfRotation = Vector3.zero;

	private Vector3 initialPosition;
	public float distance = 10f;

	// Use this for initialization
	void Start () {
		initialPosition = transform.localPosition - centreOfRotation;
		// Height is not considered
		initialPosition.y = 0;
		distance = initialPosition.magnitude;
//		Debug.Log ("Initial "  + initialPosition);
		Vector3 cross = Vector3.Cross (initialPosition, initialPosition);
//		Debug.Log ("Test " + cross);

	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 currentPosition = transform.localPosition - centreOfRotation;
		float height = transform.localPosition.y;

		currentPosition.y = 0;
		currentPosition = Vector3.Normalize (currentPosition) * distance;

		//Debug.Log ("Current" + currentPosition);

		int sign = Vector3.Cross(currentPosition, initialPosition).y < 0 ? -1 : 1;
		float angleFromInitialPos = sign * Vector3.Angle (currentPosition, initialPosition);


		//Debug.Log (sign);
		Vector3 rotation = Vector3.zero;
		rotation.y = -1 * angleFromInitialPos;
		transform.parent.Rotate(rotation);

		currentPosition = initialPosition;
		currentPosition.y = height;
		transform.localPosition = currentPosition;
	}
}
