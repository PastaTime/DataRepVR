using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

	public Transform startingPos;

	private Transform lastPos;
	private Transform destinationPos;

	public float panTime = 2f;

	private bool panning = false;
	private float counter = 0f;

	void Start () {
		// Set Camera to Position A
		lastPos = startingPos;
		transform.position = startingPos.position;
		transform.eulerAngles = startingPos.eulerAngles;
		transform.localScale = startingPos.localScale;
	}

	// Update is called once per frame
	void Update () {
		if (!panning) {
			return;
		}
//		Debug.Log ("Panning...");

		if (counter > panTime) {
			panning = false;
			lastPos = destinationPos;
			counter = 0f;
			Pan (1);
			return;
		}
			
		counter += Time.deltaTime;
		float t = counter / panTime;
		Pan (t);		
	}

	public bool isPanning()
	{
		return panning;
	}

	public void MoveTo(Transform destination) {
		if (panning)
			return;

//		Debug.Log ("Starting Pan");

		lastPos.position = transform.position;
		lastPos.eulerAngles = transform.eulerAngles;
		destinationPos = destination;

		panning = true;
		counter = 0f;
	}

	private void Pan(float fraction) {
		Vector3 position = Vector3.Lerp (lastPos.position, destinationPos.position, fraction);
		Vector3 eulers = Vector3.Lerp (lastPos.eulerAngles, destinationPos.eulerAngles, fraction);
		Vector3 scale = Vector3.Lerp (lastPos.localScale, destinationPos.localScale, fraction);

		transform.position = position;
		transform.eulerAngles = eulers;
		transform.localScale = scale;
	}
}
