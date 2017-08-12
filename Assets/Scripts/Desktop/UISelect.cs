using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.InputModule;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UISelect : Selectable {

	public bool Togglable = true;

	public Color highlight = Color.red;
	public Color nonhighlight = new Color(0.3f,0.3f,0.3f);

    public InstanceColour matchNodeColour;
    private Color nonhighlightlabel;

	public Text label;
	public Text buttonLabel;
	public string buttonUpText = "Off";
	public string buttonDownText = "On";
	public AudioClip buttonUpSound;
	public AudioClip buttonDownSound;

	[System.Serializable]
	public class SelectEvent : UnityEvent<bool> {}

	[SerializeField]
	public SelectEvent onPress;

	private Controller control;
	// Records the position of the A button as the button is highlighted (this avoids rapidly selecting a button while moving)
	private bool controllerButton;

	private bool buttonState = false;

	void Start()
	{
		if (label != null)
		{
			nonhighlightlabel = label.color;
		}

        if (matchNodeColour != null)
        {
            highlight = matchNodeColour.color;
        }
		control = Controller.GetInstance ();

        //hi
	}

	public override void OnSelect () {
		buttonLabel.color = highlight;
		if (label != null)
		{
			label.color = highlight;
		}
		controllerButton =control.GetButtonDown (Controller.Button.A) || control.GetButtonDown(Controller.Button.LJ);
	}

	public override void WhileSelected () {
    if (!controllerButton && (control.GetButtonDown (Controller.Button.A) || control.GetButtonDown(Controller.Button.LJ))) {
            buttonState = !buttonState;
            onPress.Invoke(buttonState);
            if (Togglable) {
				PressAnimation ();
			} else {
				StartCoroutine (TapButton ());
			}
		}
		controllerButton = control.GetButtonDown (Controller.Button.A) || control.GetButtonDown(Controller.Button.LJ);
	}

	public override void OnUnselect () {
		buttonLabel.color = nonhighlight;
		if (label != null)
		{
			label.color = nonhighlightlabel;	
		}
	}

	private void PressAnimation() {
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

	IEnumerator TapButton() {
		GetComponent<CompressibleUI> ().Retract ();
        GetComponent<AudioSource>().PlayOneShot(buttonDownSound);
        yield return new WaitForSeconds(1f);
       
		
		GetComponent<CompressibleUI> ().Expand ();
		GetComponent<AudioSource> ().PlayOneShot (buttonUpSound);
	}
}
