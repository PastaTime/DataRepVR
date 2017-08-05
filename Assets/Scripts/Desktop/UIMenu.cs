using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : Menu {
	public Menu aboveMenu;

	public UISelect[] menuItems;

	private int index = 0;

	void Start() {
		if (active)
			menuItems [index].Select ();
	}

	public override void OnActivation () {
		Debug.Log ("UI Menu Selected");
		manager.camPan.MoveTo (transform);
		index = 0;
		menuItems [index].Select ();
	}

	protected override void moveUp () {
		Debug.Log ("Up");
		if (index == 0) {
			Debug.Log ("Move to Table");
			aboveMenu.activate ();
			deactivate ();
			return;
		}

		menuItems [index].Unselect ();
		index--;
		menuItems [index].Select ();
	}

	protected override void moveDown () {
		Debug.Log ("Down");
		if (index == menuItems.Length - 1) {
			return;
		}

		menuItems [index].Unselect ();
		index++;
		menuItems [index].Select ();
	}

	protected override void moveLeft () {
		return;
	}

	protected override void moveRight () {
		return;
	}

	public override void OnDeactivation () {
		menuItems [index].Unselect ();
	}


}
