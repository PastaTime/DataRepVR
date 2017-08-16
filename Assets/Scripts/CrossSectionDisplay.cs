using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossSectionDisplay : MonoBehaviour {

	public Slider leftSlider;
	public Slider rightSlider;

	public CrossSectionController meshSet;
	
	private LineRenderer lineRenderer;

	public float leftSliderLength = 0f;
	public float rightSliderLength = 0f;

	private bool lessThan = true;

	// Use this for initialization
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer> ();
		
		LeftSliderPositionChanged(leftSlider.GetComponent<Slider>().value);
		RightSliderPositionChanged(rightSlider.GetComponent<Slider>().value);
	}

	public void SliderPosChange(float value, Slider slider, float length, int lineIndex)
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

	public void applyCrossSection()
	{
		Vector2 leftPoint = new Vector2(-0.5f, leftSlider.value - 0.5f);
		Vector2 rightPoint = new Vector2(0.5f, rightSlider.value - 0.5f);
		meshSet.setCrossSection(leftPoint, rightPoint, lessThan);
	}

	public void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			applyCrossSection();
		}
	}

	public void invertSelection()
	{
		lessThan = !lessThan;
	}
}
