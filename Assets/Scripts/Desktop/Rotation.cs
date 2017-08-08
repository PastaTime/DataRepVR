using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Rotation : MonoBehaviour {

	public enum Direction
	{
		LEFT=1, RIGHT=-1
	}

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
		if (direction == 0f) {
			counter = 0f;
			return;
		}

		Vector3 rotation = Vector3.zero;
		rotation.y = direction * getSpeed() * Time.deltaTime;

		transform.Rotate (rotation);
	}

	public void rotate(Direction direction)
	{
		Vector3 rotation = Vector3.zero;
		rotation.y = (float)direction * maxSpeed * Time.deltaTime;
			transform.Rotate(rotation);
	}

	private float getSpeed() {
		counter += Time.deltaTime;
		return Mathf.Lerp (minSpeed, maxSpeed, Mathf.Clamp01(counter / speedUpTime));
	}
}
