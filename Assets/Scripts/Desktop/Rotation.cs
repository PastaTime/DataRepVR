using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	public float minSpeed = 10f;
	public float maxSpeed = 100f;

	private float counter = 0f;
	public float speedUpTime = 2f;



	private Controller controller;
	// Use this for initialization
	void Start () {
		controller = Controller.GetInstance ();
	}
	
	// Update is called once per frame
	void Update () {

		float direction = controller.GetTriggerValue (Controller.Trigger.Left) - controller.GetTriggerValue (Controller.Trigger.Right);
		Debug.Log (direction);
		if (-Controller.deadZone <= direction && direction <= Controller.deadZone) {
			counter = 0f;
			return;
		}

		Vector3 rotation = Vector3.zero;
		rotation.y = direction * getSpeed() * Time.deltaTime;

		transform.Rotate (rotation);
		
	}




	private float getSpeed() {
		counter += Time.deltaTime;
		return Mathf.Lerp (minSpeed, maxSpeed, Mathf.Clamp01(counter / speedUpTime));
	}
}
