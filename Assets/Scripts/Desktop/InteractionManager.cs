﻿using UnityEngine;

public class InteractionManager : MonoBehaviour {

	public CameraPan camPan;

	public bool invertedNavigation = false;

	// Returns a Vector of filtered joystick data
	public Vector2 GetAxis (Controller.Joystick joy) {
		if (camPan.isPanning ())
			return Vector2.zero;
		
		Vector2 stick = Controller.GetInstance ().GetJoystickAxis (joy);

		int sign = 1;
		if (invertedNavigation)
			sign = -1;
		return sign * stick;
	}

}
