using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionMenu : Menu {

	public override void OnActivation () {
		manager.camPan.MoveTo (transform);
		menuItems [index].Select ();
	}

	public override void OnDeactivation () {
		menuItems [index].Unselect ();
	}

	protected  void moveUp () {

	}

	protected  void moveDown () {

	}

	protected void moveLeft () {
		if (index == 0) {
			leftMenu.activate ();
			deactivate ();
			return;
		}

		menuItems [index].Unselect ();
		index--;
		menuItems [index].Select ();
	}

	protected void moveRight () {
		if (index == (menuItems.Length - 1)) {
			return;
		}

		menuItems [index].Unselect ();
		index++;
		menuItems [index].Select ();
	}
}
