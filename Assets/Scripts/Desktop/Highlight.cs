using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : Selectable {

	public Material highlight;

	private Material nonhighlighted;

	// Min and Max values to clamp mesh sliders too.
	public float maxY = 1.4f;
	public float minY = 0.75f;

	public float speed = 10f;
	
	public Rotation rotator;

	public Vector3 centerPoint = Vector3.zero;
	public float maxDegree = 45f;
	public float minDegree = 5f;
	public float panningSpeed = 30f;
	public bool panning = false;
	private Vector3 prePanPosition = Vector3.zero;
	private Vector3 prePanRotation = Vector3.zero;
	private CameraPan camPan;



	void Start() {
		camPan = GameObject.FindObjectOfType<CameraPan> ();
	}
 
    public void Awake() {
		nonhighlighted = GetComponent<Renderer> ().material;
	}

	public override void OnSelect () {
		MeshRenderer rend = GetComponent<MeshRenderer> ();
		rend.material = highlight;
	}

	public override void WhileSelected () {
		Controller control = Controller.GetInstance ();
		Vector2 rightJoy = control.GetJoystickAxis (Controller.Joystick.Right);

		if (control.GetButton (Controller.Button.RB)) {
			if (!panning) {
				startPan ();
			} else {
				CameraPan (rightJoy);
			}
			return;
		} else if (panning) {
			endPan ();
		}
			
		if (rightJoy.y != 0f) {
			Vector3 pos = transform.position;
			pos.y += rightJoy.y * speed * Time.deltaTime;
			pos.y = Mathf.Clamp (pos.y, minY, maxY);
			transform.position = pos;
		}
		if (rightJoy.x != 0f)
		{
			Rotation.Direction direction = (rightJoy.x < 0f) ? Rotation.Direction.LEFT : Rotation.Direction.RIGHT;
			rotator.rotate(direction);
		}
	}

	public override void OnUnselect () {
		MeshRenderer rend = GetComponent<MeshRenderer> ();
		rend.material = nonhighlighted;
	}

	private void startPan() {
		if (panning) {
			Debug.LogError ("Camera pan did not exit correctly previously");
			return;
		}
		prePanPosition = camPan.transform.position;
		prePanRotation = camPan.transform.eulerAngles;
		panning = true;
	}

	private void CameraPan(Vector2 rightjoy) {
		rightjoy = -rightjoy;
		Vector3 towardsOrigin = Vector3.zero - Camera.main.transform.position;
		if (camPan.transform.eulerAngles.x >= maxDegree) {
			rightjoy.y = Mathf.Clamp (rightjoy.y, 0f, 1f);
		}
		if (camPan.transform.eulerAngles.x <= minDegree) {
			rightjoy.y = Mathf.Clamp (rightjoy.y, -1f, 0f);
		}
		Vector3 directionX = Vector3.Cross (towardsOrigin, Vector3.up);
		directionX = rightjoy.y * Vector3.Normalize (directionX);

		Vector3 axis = Vector3.zero + directionX;
		axis += Vector3.up * rightjoy.x;

		camPan.transform.RotateAround (Vector3.zero, axis, panningSpeed * Time.deltaTime);
	}

	private void endPan() {
		camPan.transform.position = prePanPosition;
		camPan.transform.eulerAngles = prePanRotation;

		panning = false;
	}
}

