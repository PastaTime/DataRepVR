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

	public float transitionDelay = 2f;
	private float firstActive = 0f;

	public void activate () {
		OnActivation ();
		active = true;
		seenZero = false;
		firstActive = transitionDelay;
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

	void Update() {
		
		if (manager.camPan.isPanning ()) {
			return;
		}

		if (Controller.GetInstance().GetButton(Controller.Button.RB) || Controller.GetInstance().GetButtonUp(Controller.Button.RB))
		{
			return;
		}
			
		if (active) {
			Vector2 leftStick = manager.GetAxis (Controller.Joystick.Left);
			if (!seenZero && leftStick.y == 0 && leftStick.x == 0) {
				seenZero = true;
				return;
			} else if (!seenZero) {
				return;
			}

			if (leftStick.y != 0 || leftStick.x != 0) {
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
