using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionMenu : Menu {

	public UISelect[] menuItems;

	public Menu leftMenu;
	public Menu AboveMenu;


	private int index = 0;

	public override void OnActivation () {
		manager.camPan.MoveTo (transform);
		menuItems [index].Select ();
	}

	public override void OnDeactivation () {
		menuItems [index].Unselect ();
	}

	protected override void moveUp () {

	}

	protected override void moveDown () {

	}

	protected override void moveLeft () {
		if (index == 0) {
			leftMenu.activate ();
			deactivate ();
			return;
		}

		menuItems [index].Unselect ();
		index--;
		menuItems [index].Select ();
	}

	protected override void moveRight () {
		if (index == (menuItems.Length - 1)) {
			return;
		}

		menuItems [index].Unselect ();
		index++;
		menuItems [index].Select ();
	}


	// Update is called once per frame
	void Update () {
		
	}
}
