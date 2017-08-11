using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionMenu : Menu
{

//	private int horizontalIndex = 0;
//	private int verticalIndex = 0;

	public override void OnActivation () {
		manager.camPan.MoveTo (transform);
		menuItems [index].Select ();
	}

	public override void OnDeactivation () {
		menuItems [index].Unselect ();
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
	
	protected override void sort() {
		if (menuItems != null && menuItems.Length > 0)
		{
			Selectable item = menuItems[index];
			Array.Sort(menuItems, (s1, s2) => s2.transform.position.x.CompareTo(s1.transform.position.x));
			index = Array.IndexOf(menuItems, item);
		}
	} 
}
