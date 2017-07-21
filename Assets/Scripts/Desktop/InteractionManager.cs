using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	public Selectable[] selectList;

	public UISelect[] menuList;

	public CameraPan camPan;

	public int firstSelected = 0;

	public int selected;

	public float coolDown = 2f;
	private float counter = 0f;


	// Use this for initialization
	void Start () {
		if (selectList.Length <= 0) {
			Debug.LogError ("Select List is not initialized, please initialize from Editor");
		} else if (selectList.Length <= firstSelected) {
			Debug.LogError ("Initial Select Index is larger than the total length of selectables");
			selected = 0;
		} else {
			selected = firstSelected;
		}

	}
	
	// Update is called once per frame
	void Update () {
		Controller controller = Controller.GetInstance ();

		Vector2 leftStick = controller.GetJoystickAxis (Controller.Joystick.Left);

		float vert = leftStick.y;
		if (counter >= coolDown)
			counter = 0f;

		// Up
		if (vert >= Controller.deadZone && counter == 0f) {
			currentSelection().Unselect ();
			incrementIndex ();
			currentSelection().Select ();
			counter += Time.deltaTime;
		} else if (vert >= Controller.deadZone) {
			counter += Time.deltaTime;
		}


		// Dead zone
		if (- Controller.deadZone < vert && vert < Controller.deadZone)
			counter = 0f;
		
		// Down
		if (vert <= - Controller.deadZone && counter == 0f) {
			currentSelection().Unselect ();
			decrementIndex ();
			currentSelection().Select ();
			counter += Time.deltaTime;
		} else if (vert <= - Controller.deadZone) {
			counter += Time.deltaTime;
		}

	}

	private Selectable currentSelection() {
		int index = selected;
		if (index < selectList.Length)
			return selectList [index];
		index -= selectList.Length;
		if (index >= menuList.Length)
			Debug.LogError ("Index out of range");
		return menuList [index];
	}

	private void incrementIndex() {
		selected++;
		if (selected == selectList.Length) {
			camPan.MoveTo (CameraPan.Position.B);
		} else if (selected == selectList.Length + menuList.Length) {
			camPan.MoveTo (CameraPan.Position.A);
			selected = 0;
			return;
		}
	}

	private void decrementIndex() {
		selected--;
		if (selected < 0) {
			selected = selectList.Length + menuList.Length - 1;
			camPan.MoveTo (CameraPan.Position.B);
		} else if (selected == selectList.Length - 1) {
			camPan.MoveTo (CameraPan.Position.A);
		}
	}
}
