using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.InputModule;
using UnityEngine.UI;

public class UISlider : Selectable {

//	public SliderVolume slider;
	private Slider slider;
	public Image handle;

	public float speed = 1f;

	public Color highlight = new Color(0.3f,0.3f,0.3f);
	private Color nonhighlight;

	// Use this for initialization
	void Start () {
		nonhighlight = handle.color;
		slider = GetComponent<Slider>();
	}

	public override void OnSelect () {
		// Highlight Slider
		handle.color = highlight;
	}

	public override void WhileSelected () {
		
		Vector2 rightJoy = Controller.GetInstance ().GetJoystickAxis (Controller.Joystick.Right);

		if (rightJoy.y != 0f) {
			float currentValue = slider.value;
			currentValue += rightJoy.y * speed * Time.deltaTime;
			currentValue = Mathf.Clamp01(currentValue);
			slider.value = currentValue;
		}
	}

	public override void OnUnselect () {
		// Unhighlight Slider
		handle.color = nonhighlight;
	}

}
