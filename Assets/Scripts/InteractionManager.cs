using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

	public Selectable[] selectList;
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
			selectList [selected].Unselect ();
			selected++;
			if (selected >= selectList.Length)
				selected = 0;
			selectList [selected].Select ();
			counter += Time.deltaTime;
		} else if (vert >= Controller.deadZone) {
			counter += Time.deltaTime;
		}


		// Dead zone
		if (- Controller.deadZone < vert && vert < Controller.deadZone)
			counter = 0f;
		
		// Down
		if (vert <= - Controller.deadZone && counter == 0f) {
			selectList [selected].Unselect ();
			selected--;
			if (selected < 0)
				selected = selectList.Length - 1;
			selectList [selected].Select ();
			counter += Time.deltaTime;
		} else if (vert <= - Controller.deadZone) {
			counter += Time.deltaTime;
		}

	}
}
