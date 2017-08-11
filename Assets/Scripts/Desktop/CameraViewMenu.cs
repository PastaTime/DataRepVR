using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewMenu : Menu {
//	public Menu belowMenu;

	public override void OnActivation () {
//		Debug.Log ("Top Down Menu Selected");
		manager.camPan.MoveTo (transform);
	}
}
