using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour {

	public InteractionManager manager;

	protected bool seenZero = true;

	public bool active = false;

	public void activate () {
		OnActivation ();
		active = true;
	}

	public abstract void OnActivation ();

	protected abstract void moveUp ();

	protected abstract void moveDown ();

	protected abstract void moveLeft ();

	protected abstract void moveRight ();

	public void deactivate () {

		OnDeactivation ();
		active = false;
	}

	public abstract void OnDeactivation ();

	void Update() {
		if (active) {
			Vector2 leftStick = manager.GetAxis (Controller.Joystick.Left);

			if (leftStick.y == 0 && leftStick.x == 0) {
				seenZero = true;
				return;
			} else if (!seenZero) {
				return;
			}
			seenZero = false;

			if (leftStick.y > 0) {
				moveUp ();
			} else if (leftStick.y < 0) {
				moveDown ();
			} else if (leftStick.x > 0) {
				moveLeft ();
			} else if (leftStick.x < 0) {
				moveRight ();
			}
		}
	}
}
