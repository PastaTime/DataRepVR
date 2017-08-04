using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class CentreMenu : Menu {

	public Menu aboveMenu;
	public Menu belowMenu;

	public GameObject nodes;

	public List<Selectable> menuItems = new List<Selectable>();

	private int index;

	void Start() {
		foreach (Transform child in nodes.transform)
		{
			Highlight item = child.GetComponent<Highlight> ();
			if (item != null)
			{
				menuItems.Add (item);
			}
		}
		index = menuItems.Count;
		if (active)
			menuItems [index].Select ();
	}

	public void removeItem(Selectable item) {
		menuItems.Remove (item);
	}

	public override void OnActivation () {
		Debug.Log ("Table Menu Selected");
		sort ();
		manager.camPan.MoveTo (transform);
		if (index >= menuItems.Count)
			moveUp ();
		else
			moveDown ();
	}

	private Selectable getNext() {
		bool found = false;
		int currentPos = index;
		Debug.Log ("Centre Menu Up");
		while (!found) {
			currentPos++;
			if (currentPos > menuItems.Count - 1) {
				return null;
			}
			if (menuItems [currentPos].gameObject.activeSelf) {
				index = currentPos;
				return menuItems[currentPos];
			}
			
		}
		return null;
	}

	private Selectable getPrevious() {
		
		int currentPos = index;
		bool found = false;
		while (!found) {
			currentPos--;
			if (currentPos < 0) {
				return null;
			}
			if (menuItems [currentPos].gameObject.activeSelf) {
				index = currentPos;
				return menuItems[currentPos];
			}
			
		}
		return null;
	}

	protected override void moveUp () {
		sort ();
		if (index >= 0 && index < menuItems.Count)
		{
			menuItems[index].Unselect();
		}
		Selectable prev = getPrevious();
		if (prev == null) 
		{
			index = -1;
			aboveMenu.activate ();
			deactivate ();
			return;
		} else {
			prev.Select ();
		}
		sort ();
	}

	protected override void moveDown () {
		sort ();
		if (index >= 0 && index < menuItems.Count)
		{
			menuItems[index].Unselect();
		}
		Selectable next = getNext();
		if (next == null)
		{
			index = menuItems.Count;
			belowMenu.activate ();
			deactivate ();
			return;
		} else {
			next.Select ();
		}
		sort ();
	}

	protected override void moveLeft () {
		// Add Cross Section Menu
		return;
	}

	protected override void moveRight () {
		return;
	}

	public override void OnDeactivation () {
//		menuItems [index].Unselect ();
	}
		

	private void sort() {
		//sort Menu items;

		menuItems.Sort (delegate(Selectable s1, Selectable s2) {
			return s2.transform.position.y.CompareTo(s1.transform.position.y);
		});
	} 


}
