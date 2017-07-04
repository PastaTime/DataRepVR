using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcMotion : MonoBehaviour {


	public Vector3 centreOfRotation = Vector3.zero;

	private Vector3 initialPosition;
	public float distance = 10f;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position - centreOfRotation;
		// Height is not considered
		initialPosition.y = 0;
		distance = initialPosition.magnitude;

	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 currentPosition = transform.position - centreOfRotation;
		float height = transform.position.y;

		currentPosition.y = 0;
		currentPosition = Vector3.Normalize (currentPosition) * distance;

		float angleFromInitialPos = Vector3.Angle (currentPosition, initialPosition);
		Debug.Log (angleFromInitialPos);
		Vector3 rotation = Vector3.zero;
		rotation.y = -1 * angleFromInitialPos;
		transform.parent.Rotate(rotation);

		currentPosition = Vector3.Normalize (initialPosition) * distance;
		currentPosition.y = height;
		transform.position = currentPosition;
	}
}
