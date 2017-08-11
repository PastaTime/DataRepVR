using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour {

	public InteractionManager manager;

	protected bool seenZero = true;

	public bool active = false;

	protected int index = 0;

	public GameObject itemsParent;

//	public List<Selectable> menuItems = new List<Selectable>();

	public Selectable[] menuItems;

	public Menu topMenu;

	public Menu bottomMenu;

	public Menu leftMenu;

	public Menu rightMenu;

	public void activate () {
		OnActivation ();
		active = true;
	}

//	void Start()
//	{
////		populateItems();
//
//	}

//	protected void populateItems()
//	{
//		if (itemsParent != null)
//		{
//			foreach (Transform transform in itemsParent.transform)
//			{
//				Selectable s = transform.GetComponent<Selectable>();
//				if (s != null)
//				{
//					menuItems.Add(s);
//				}
//			}
//		}
//	}

	public abstract void OnActivation ();

	protected void moveUp () {
		sort ();
		if (index == 0) {
			if (topMenu != null)
			{
				topMenu.activate ();
				deactivate ();
			}
			return;
		}

//		for (int i = 0; i < menuItems.Length; i++) {
//			if (menuItems [i].selected) {
//				index = i;
//				break;
//			}
//		}

		menuItems [index].Unselect ();
		index--;
		menuItems [index].Select ();
	}

	protected void moveDown () {
		sort ();
		if (index == menuItems.Length - 1) {
			if (bottomMenu != null)
			{
					bottomMenu.activate();
					deactivate();
			}
			return;
		}

//		for (int i = 0; i < menuItems.Length; i++) {
//			if (menuItems [i].selected) {
//				index = i;
//				break;
//			}
//		}

		menuItems [index].Unselect ();
		index++;
		menuItems [index].Select ();
	}

	protected void moveLeft () {
		if (leftMenu != null)
		{
			leftMenu.activate();
			deactivate();
		}
	}

	protected void moveRight () {
		if (rightMenu != null)
		{
			rightMenu.activate ();
			deactivate ();
		}
	}

	public void deactivate () {
		OnDeactivation ();
		active = false;
	}
	
	public void OnDeactivation () {
		menuItems [index].Unselect ();
	}
	
	protected void sort() {
		if (menuItems != null && menuItems.Length > 0)
		{
			//sort Menu items;
			Selectable item = menuItems[index];
//			Array.Sort(menuItems, (s1, s2) => s2.transform.position.y.CompareTo(s1.transform.position.y));
		Array.Sort (menuItems,  delegate(Selectable s1, Selectable s2) {
			return s2.transform.position.y.CompareTo(s1.transform.position.y);
		});
			index = Array.IndexOf(menuItems, item);
//			index = menuItems.IndexOf(item, 0, menuItems.Count -1);
		}
	} 

	void Update() {
		if (active) {
			Vector2 leftStick = manager.GetAxis (Controller.Joystick.Left);

			if (leftStick.y == 0 && leftStick.x == 0) {
				seenZero = true;
				return;
			} else if (!seenZero) {
				return;
			}
			seenZero = false;

			if (leftStick.y > 0) {
				moveUp ();
			} else if (leftStick.y < 0) {
				moveDown ();
			} else if (leftStick.x < 0) {
				moveLeft ();
			} else if (leftStick.x > 0) {
				moveRight ();
			}
		}
	}
}
