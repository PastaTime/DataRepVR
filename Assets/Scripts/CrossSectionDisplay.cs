using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossSectionDisplay : MonoBehaviour {

	public GameObject leftSlider;
	public GameObject rightSlider;

	public GameObject meshSet;

	private LineRenderer lineRenderer;

	public float leftSliderLength = 0f;
	public float rightSliderLength = 0f;

	// Use this for initialization
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer> ();
		
		LeftSliderPositionChanged(0);
		RightSliderPositionChanged(0);
	}

	public void SliderPosChange(float value, GameObject slider, float length, int lineIndex)
	{
		Vector3 rotations = slider.transform.eulerAngles;
		Vector3 handlePos = slider.transform.position + Quaternion.Euler(-rotations.x, 0, 0) * new Vector3 (0, (value - 0.5f) * length, 0);
		lineRenderer.SetPosition (lineIndex, handlePos);
	}

	public void LeftSliderPositionChanged(float value) {
		SliderPosChange(value, leftSlider, leftSliderLength, 0);
	}

	public void RightSliderPositionChanged(float value) {
		SliderPosChange(value, rightSlider, rightSliderLength, 1);
	}

	public void OnSlideEnd()
	{
		float leftVal = leftSlider.GetComponent<Slider>().value;
		float rightVal = rightSlider.GetComponent<Slider>().value;

		Vector3 leftPos = lineRenderer.GetPosition(0);
		Vector3 rightPos = lineRenderer.GetPosition(1);

		Vector3 leftSliderPos = leftSlider.transform.position;
		Vector3 rightSliderPos = rightSlider.transform.position;

		Vector3 LT =  Quaternion.Euler(31, 0, 0) * (leftSliderPos - leftPos);
		Debug.Log(LT);
		
		meshSet.GetComponent<CrossSectionController>().setCrossSection(0,0);
	}
}
