using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewMenu : Menu {
	public Menu belowMenu;

	public override void OnActivation () {
//		Debug.Log ("Top Down Menu Selected");
		manager.camPan.MoveTo (transform);
	}


	protected override void moveUp () {
		return;
	}

	protected override void moveDown () {
		if (belowMenu != null)
		{
			belowMenu.activate ();
			deactivate ();
		}
	}

	protected override void moveLeft () {
		// Add Cross Section Menu
		return;
	}

	protected override void moveRight () {
		return;
	}

	public override void OnDeactivation () {
		// Do Nothing
	}
}
