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

	public void Start() {
		nonhighlighted = GetComponent<Renderer> ().material;
	}

	public override void OnSelect () {
		MeshRenderer rend = GetComponent<MeshRenderer> ();
		rend.material = highlight;
	}

	public override void WhileSelected () {
		Vector2 rightJoy = Controller.GetInstance ().GetJoystickAxis (Controller.Joystick.Right);
		
		if (rightJoy.y != 0f) {
			Vector3 pos = transform.position;
			pos.y += rightJoy.y * speed * Time.deltaTime;
			pos.y = Mathf.Clamp (pos.y, minY, maxY);
			transform.position = pos;
		}
	}

	public override void OnUnselect () {
		MeshRenderer rend = GetComponent<MeshRenderer> ();
		rend.material = nonhighlighted;
	}

}
