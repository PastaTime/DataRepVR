using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionController : MonoBehaviour
{
	private List<GameObject> displayMeshes = new List<GameObject>();
	
	// Use this for initialization
	void Start () {
		foreach (Transform child in transform)
		{
			GameObject childObject = child.gameObject;
			if (childObject.GetComponent<PolyMeshController>() != null)
			{
				displayMeshes.Add(child.gameObject);
			}
		}
	}

	public void setCrossSection(float gradient, float intercept)
	{
		foreach (GameObject obj in displayMeshes)
		{
			obj.GetComponent<PolyMeshController>().setCrossSection(gradient, intercept);
		}
	}
}
