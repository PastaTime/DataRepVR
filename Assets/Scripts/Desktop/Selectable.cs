using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour {

	protected bool selected = false;

	public void Select() {
		selected = true;
		OnSelect ();
	}

	public void Unselect() {
		selected = false;
		OnUnselect ();
	}

	public abstract void OnSelect (); 

	public abstract void WhileSelected ();

	public abstract void OnUnselect ();





	void Update()
	{
		if (selected) 
		{
			WhileSelected ();
		}
	}
}
