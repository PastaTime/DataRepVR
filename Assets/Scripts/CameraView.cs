using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : Selectable {

	public Transform cameraFocus;
	public Transform returnFocus;
	public CameraPan camPan;

	public override void OnSelect ()
	{
		camPan.MoveTo (cameraFocus);
	}

	public override void WhileSelected ()
	{
		// Do Nothing
	}

	public override void OnUnselect ()
	{
		camPan.MoveTo (returnFocus);
	}
}
