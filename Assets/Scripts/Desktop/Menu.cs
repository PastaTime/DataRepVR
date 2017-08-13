using System;
using UnityEngine;

public abstract class Menu : MonoBehaviour {

	public InteractionManager manager;

	public bool seenZero = false;

	public bool active = false;

	public int index = 0;

	public GameObject itemsParent;

	public Selectable[] menuItems;

	public Menu topMenu;

	public Menu bottomMenu;

	public Menu leftMenu;

	public Menu rightMenu;

	public void activate () {
		OnActivation ();
		active = true;
		seenZero = false;
		firstActive = true;
	}

	public abstract void OnActivation ();

	protected virtual void moveUp () {
		sort ();
		if (index == 0) {
			if (topMenu != null)
			{
				topMenu.activate ();
				deactivate ();
			}
			return;
		}

		menuItems [index].Unselect ();
		index--;
		menuItems [index].Select ();
	}

	protected virtual void moveDown () {
		sort ();
		if (index == menuItems.Length - 1) {
			if (bottomMenu != null)
			{
					bottomMenu.activate();
					deactivate();
			}
			return;
		}

		menuItems [index].Unselect ();
		index++;
		menuItems [index].Select ();
	}

	protected virtual void moveLeft () {
		if (leftMenu != null)
		{
			leftMenu.activate();
			deactivate();
		}
	}

	protected virtual void moveRight () {
		Debug.Log ("Moving Right");
		if (rightMenu != null)
		{
			rightMenu.activate ();
			deactivate ();
			Debug.Log ("Menus Seen: " +  rightMenu.seenZero);
		}
	}

	public void deactivate () {
		OnDeactivation ();
		active = false;
		seenZero = false;
	}
	
	public virtual void OnDeactivation () {
		menuItems [index].Unselect ();
	}
	
	protected virtual void sort() {
		if (menuItems != null && menuItems.Length > 0)
		{
			Selectable item = menuItems[index];
			Array.Sort(menuItems, (s1, s2) => s2.transform.position.y.CompareTo(s1.transform.position.y));
			index = Array.IndexOf(menuItems, item);
		}
	} 
	private bool firstActive = false;
	void Update() {
		Vector2 leftStick = manager.GetAxis (Controller.Joystick.Left);
		if (firstActive && leftStick.y == 0 && leftStick.x == 0) {
			// Returning dud values for the first part of the transition (very weird)
			return;
		} else if (firstActive) {
			firstActive = false;
		}
			
		if (active) {

			if (!seenZero && leftStick.y == 0 && leftStick.x == 0) {
				Debug.Log ("Left Stick: " + leftStick);
				seenZero = true;
				return;
			} else if (!seenZero) {
				Debug.Log ("block");
				return;
			}

			if (leftStick.y != 0 || leftStick.x != 0) {
				Debug.Log ("Movement");
				seenZero = false;
			}

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
