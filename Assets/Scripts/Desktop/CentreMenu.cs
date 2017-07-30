using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreMenu : Menu {

	public Menu aboveMenu;
	public Menu belowMenu;

	public Selectable[] menuItems;

	private int index = 0;

	void Start() {
		if (active)
			menuItems [index].Select ();
	}


	public override void OnActivation () {
		Debug.Log ("Table Menu Selected");
		manager.camPan.MoveTo (transform);
		menuItems [index].Select ();
	}

	protected override void moveUp () {
		Debug.Log ("Centre Menu Up");
		if (index == 0) {
			aboveMenu.activate ();
			deactivate ();
			return;
		}
		sort ();
		menuItems [index].Unselect ();
		index--;
		menuItems [index].Select ();
	}

	protected override void moveDown () {
		Debug.Log ("Centre Menu Down");
		if (index == menuItems.Length - 1) {
			Debug.Log ("Move Down a Menu");
			belowMenu.activate ();
			deactivate ();
			return;
		}
		sort ();
		menuItems [index].Unselect ();
		index++;
		menuItems [index].Select ();
	}

	protected override void moveLeft () {
		// Add Cross Section Menu
		return;
	}

	protected override void moveRight () {
		return;
	}

	public override void OnDeactivation () {
		menuItems [index].Unselect ();
	}
		

	private void sort() {
		//sort Menu items;
		Debug.Log("Unimplemented");
	}


}
