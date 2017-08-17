using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossSectionDisplay : MonoBehaviour {

	public Slider leftSlider;
	public Slider rightSlider;

	public CrossSectionController meshSet;
	
	private LineRenderer UILine;

	public LineRenderer worldSpaceLine;

	public float leftSliderLength = 0f;
	public float rightSliderLength = 0f;

	private bool lessThan = true;

	public float sliderHeight = 0.24f;

	// Use this for initialization
	void Start () {
		UILine = this.GetComponent<LineRenderer> ();
		
		LeftSliderPositionChanged(leftSlider.GetComponent<Slider>().value);
		RightSliderPositionChanged(rightSlider.GetComponent<Slider>().value);
	}

	public void SliderPosChange(float value, Slider slider, float length, int lineIndex)
	{
		Vector3 rotations = slider.transform.eulerAngles;
		Vector3 handlePos = slider.transform.position + Quaternion.Euler(-rotations.x, 0, 0) * new Vector3 (0, (value - 0.5f) * length, 0);
		UILine.SetPosition (lineIndex, handlePos);
		float xPos = (slider == leftSlider) ? -0.5f : 0.5f;
		Vector3 worldPos = new Vector3(xPos, sliderHeight, value - 0.5f);
		worldSpaceLine.SetPosition(lineIndex, worldPos);
	}

	public void LeftSliderPositionChanged(float value) {
		SliderPosChange(value, leftSlider, leftSliderLength, 0);
	}

	public void RightSliderPositionChanged(float value) {
		SliderPosChange(value, rightSlider, rightSliderLength, 1);
	}

	public void applyCrossSection()
	{
		Vector2 leftPoint = new Vector2(0.5f, 0.5f - leftSlider.value);
		Vector2 rightPoint = new Vector2(-0.5f, 0.5f - rightSlider.value);
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
