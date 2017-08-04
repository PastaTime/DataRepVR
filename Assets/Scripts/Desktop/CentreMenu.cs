using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreMenu : Menu {

	public Menu aboveMenu;
	public Menu belowMenu;

	public List<Selectable> menuItems = new List<Selectable>();

//	public Selectable[] menuItems;

	private int index = 0;

	void Start() {
		foreach (Transform child in transform)
		{
			Highlight item = child.GetComponent<Highlight> ();
			if (item != null)
			{
				menuItems.Add (item);
			}
		}
		if (active)
			menuItems [index].Select ();
	}

	public void removeItem(Selectable item) {
		menuItems.Remove (item);
	}

	public override void OnActivation () {
		Debug.Log ("Table Menu Selected");
		manager.camPan.MoveTo (transform);
		if (index > 0 && index < menuItems.Count - 1)
			moveUp ();
		else
			moveDown ();
	}

	private Selectable getNext() {
		bool found = false;
		int currentPos = index;
		Debug.Log ("Centre Menu Up");
		while (!found) {
			if (currentPos > menuItems.Count - 1) {
				return null;
			}
			if (menuItems [currentPos].gameObject.activeSelf) {
				index = currentPos;
				return menuItems[currentPos];
			}
			currentPos++;
		}
		return null;
	}

	private Selectable getPrevious() {
		int currentPos = index;
		bool found = false;
		while (!found) {
			if (currentPos < 0) {
				return null;
			}
			if (menuItems [currentPos].gameObject.activeSelf) {
				index = currentPos;
				return menuItems[currentPos];
			}
			currentPos--;
		}
		return null;
	}

	protected override void moveUp () {
		// Get the next item in the list and select it
		// If we're out of items activate the next menu up

		//highlighted object get pos
		//get next pos
		// if pos is invalid go up
		// else if pos is disabled keep going
		// until something is found then stop
		Selectable prev = getPrevious();
		if (prev == null) {
			Debug.Log ("Panning up from centre");
			aboveMenu.activate ();
			deactivate ();
			return;
		} else {
			prev.Select ();
		}
//		bool found = false;
//		int currentPos = index;
//		Debug.Log ("Centre Menu Up");
//		while (!found) {
//			if (currentPos < 0) {
//				Debug.Log ("Panning up from centre");
//				aboveMenu.activate ();
//				deactivate ();
//				return;
//			}
//			if (menuItems [currentPos].gameObject.activeSelf) {
//				Debug.Log ("Selecting " + currentPos);
//				menuItems [currentPos].Select ();
//				index = currentPos;
//				return;
//			}
//			currentPos--;
//		}
		sort ();
//		for (int i = 0; i < menuItems.Count; i++) {
//			if (menuItems [i].selected) {
//				index = i;
//				break;
//			}
//		}
//
//		menuItems [index].Unselect ();
//		if (menuItems [index].gameObject.activeSelf) {
//			index--;
//			menuItems [index].Select ();
//		}
	}

	protected override void moveDown () {
//
//		bool found = false;
//		int currentPos = index;
//		Debug.Log ("Centre Menu Up");
//		while (!found) {
//			if (currentPos >= menuItems.Count - 1) {
//				Debug.Log ("Panning down from centre");
//				Debug.Log ("Move Down a Menu");
//				belowMenu.activate ();
//				deactivate ();
//				return;
//			}
//			if (menuItems [currentPos].gameObject.activeSelf) {
//				menuItems [currentPos].Select ();
//				Debug.Log ("Selecting " + currentPos);
//				index = currentPos;
//				return;
//			}
//			currentPos++;
//		}
//		sort ();

		Selectable next = getNext();
		if (next == null) {
			Debug.Log ("Panning down from centre");
			Debug.Log ("Move Down a Menu");
			belowMenu.activate ();
			deactivate ();
			return;
		} else {
			next.Select ();
		}
		sort ();
//		Debug.Log ("Centre Menu Down");
//		if (index == menuItems.Length - 1) {
//			Debug.Log ("Move Down a Menu");
//			belowMenu.activate ();
//			deactivate ();
//			return;
//		}
//
//		sort ();
//		for (int i = 0; i < menuItems.Length; i++) {
//			if (menuItems [i].selected) {
//				index = i;
//				break;
//			}
//		}
//
//		menuItems [index].Unselect ();
//		if (menuItems [index].gameObject.activeSelf) {
//			index++;
//			menuItems [index].Select ();
//		}
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

//		Array.Sort (menuItems, delegate(Selectable s1, Selectable s2) {
//			return s2.transform.position.y.CompareTo(s1.transform.position.y);
//		});
	} 


}
