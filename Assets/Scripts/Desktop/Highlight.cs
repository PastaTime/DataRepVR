using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : Selectable {
	public Material def;
	public Material highlight;

	public float speed = 10f;

	public override void OnSelect () {
		MeshRenderer rend = gameObject.GetComponent<MeshRenderer> ();
		rend.material = highlight;
	}

	public override void WhileSelected () {
		Vector2 rightJoy = Controller.GetInstance ().GetJoystickAxis (Controller.Joystick.Right);

		Debug.Log (rightJoy);
		if (rightJoy.y <= -Controller.deadZone || Controller.deadZone <= rightJoy.y) {
			Debug.Log ("Moving");
			Vector3 pos = transform.position;
			pos.y += rightJoy.y * speed * Time.deltaTime;
			transform.position = pos;
		}
			


	}

	public override void OnUnselect () {
		MeshRenderer rend = gameObject.GetComponent<MeshRenderer> ();
		rend.material = def;
	}

}
