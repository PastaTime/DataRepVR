using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Leap.Unity.InputModule {
public class Rotate : MonoBehaviour {
	public void setRotation(float sliderposition) {
		float angle = Mathf.LerpAngle (0, 180, sliderposition);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}
}
}