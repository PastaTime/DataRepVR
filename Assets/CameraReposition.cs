using UnityEngine;
using UnityEngine.VR;

public class CameraReposition : MonoBehaviour {
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C) || Controller.GetInstance().GetButtonDown(Controller.Button.Start))
		{
			InputTracking.Recenter();
		}
	}
}
