using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundLine : MonoBehaviour {

	public GameObject leftSlider;
	public GameObject rightSlider;

	private LineRenderer lineRenderer;

	public float leftSliderLength = 0f;
	public float rightSliderLength = 0f;

	// Use this for initialization
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer> ();
		Vector3 Lrotations = leftSlider.transform.eulerAngles;
		Vector3 LHandlePos = leftSlider.transform.position + Quaternion.Euler(-Lrotations.x, 0, 0) * new Vector3 (0, (0f - 0.5f) * leftSliderLength, 0);
		lineRenderer.SetPosition (0, LHandlePos);

		Vector3 Rrotations = rightSlider.transform.eulerAngles;
		Vector3 RHandlePos = rightSlider.transform.position + Quaternion.Euler(-Rrotations.x, 0, 0) * new Vector3 (0, (0f - 0.5f) * rightSliderLength, 0);
		lineRenderer.SetPosition (1, RHandlePos);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LeftSliderPositionChanged(float value) {
		Vector3 Lrotations = leftSlider.transform.eulerAngles;
		Vector3 handlePos = leftSlider.transform.position + Quaternion.Euler(-Lrotations.x, 0, 0) * new Vector3 (0, (0f - 0.5f) * leftSliderLength, 0);
		lineRenderer.SetPosition (0, handlePos);
	}

	public void RightSliderPositionChanged(float value) {
		Vector3 Rrotations = rightSlider.transform.eulerAngles;
		Vector3 handlePos = rightSlider.transform.position + Quaternion.Euler(-Rrotations.x, 0, 0) * new Vector3 (0, (0f - 0.5f) * rightSliderLength, 0);
		lineRenderer.SetPosition (1, handlePos);
	}
}
