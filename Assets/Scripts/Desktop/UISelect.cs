﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.InputModule;

public class UISelect : Selectable {
	
	public GameObject Mesh;

	public Color highlight = Color.red;
	public Color def = new Color (0.3f, 0.3f, 0.3f, 1f);

	public Text label;
	public Text buttonLabel;
	public string buttonUpText = "Off";
	public string buttonDownText = "On";
	public AudioClip buttonUpSound;
	public AudioClip buttonDownSound;



	private Controller control;
	// Records the position of the A button as the button is highlighted (this avoids rapidly selecting a button while moving)
	private bool controllerButton;

	private bool buttonState = false;

	void Start() {
		control = Controller.GetInstance ();
		Mesh.SetActive (buttonState);
	}

	public override void OnSelect () {
		buttonLabel.color = highlight;
		label.color = highlight;
		controllerButton = control.GetButtonDown (Controller.Button.A);
	}

	public override void WhileSelected () {
		if (!controllerButton && control.GetButtonDown (Controller.Button.A)) {
			PressButton ();
		}
		controllerButton = control.GetButtonDown (Controller.Button.A);
	}

	public override void OnUnselect () {
		buttonLabel.color = def;
		label.color = def;
	}

	private void PressButton() {
		buttonState = !buttonState;
		Mesh.SetActive (buttonState);
		if (buttonState) {
			GetComponent<CompressibleUI> ().Retract ();
			buttonLabel.text = buttonDownText;
			GetComponent<AudioSource> ().PlayOneShot (buttonDownSound);
		} else {
			GetComponent<CompressibleUI> ().Expand ();
			buttonLabel.text = buttonUpText;
			GetComponent<AudioSource> ().PlayOneShot (buttonUpSound);
		}

	}

}