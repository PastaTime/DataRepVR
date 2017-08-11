using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreMenu : Menu
{

//	public Menu topMenu;
//	public Menu leftMenu;
//	public Menu rightMenu;

//	public Selectable[] menuItems;

//	private int index = 0;

	void Start() {
		sort ();
		if (active)
			menuItems [index].Select ();
	}


	public override void OnActivation () {
//		Debug.Log ("Table Menu Selected");
		manager.camPan.MoveTo (transform);
		menuItems [index].Select ();
	}
//

//	protected override void moveUp () {
//		sort ();
//		if (index == 0) {
//			if (topMenu != null)
//			{
//				topMenu.activate ();
//				deactivate ();
//			}
//			return;
//		}
//
//		for (int i = 0; i < menuItems.Length; i++) {
//			if (menuItems [i].selected) {
//				index = i;
//				break;
//			}
//		}
//
//		menuItems [index].Unselect ();
//		index--;
//		menuItems [index].Select ();
//	}
//
//	protected override void moveDown () {
//		sort ();
//		if (index == menuItems.Length - 1) {
//			
//			return;
//		}
//
//		for (int i = 0; i < menuItems.Length; i++) {
//			if (menuItems [i].selected) {
//				index = i;
//				break;
//			}
//		}
//
//		menuItems [index].Unselect ();
//		index++;
//		menuItems [index].Select ();
//	}
//
//	protected override void moveLeft () {
//		Debug.Log("Left");
//		// Add Cross Section Menu
//		if (leftMenu != null)
//		{
//			leftMenu.activate();
//			deactivate();
//		}
//	}
//
//	protected override void moveRight () {
//		Debug.Log("Right");
//		if (rightMenu != null)
//		{
////				Debug.Log ("Move Down a Menu");
//			rightMenu.activate ();
//			deactivate ();
//		}
//		return;
//	}
//
//	public override void OnDeactivation () {
//		menuItems [index].Unselect ();
//	}
		
//
//	private void sort() {
//		//sort Menu items;
//		Selectable item = menuItems[index];
//		Array.Sort (menuItems, delegate(Selectable s1, Selectable s2) {
//			return s2.transform.position.y.CompareTo(s1.transform.position.y);
//		});
//		index = Array.IndexOf(menuItems, item);
//	} 

}
