using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionController : MonoBehaviour
{
	private List<PolyMeshController> displayMeshes = new List<PolyMeshController>();

	void Start () {
		foreach (Transform child in transform)
		{
			PolyMeshController mesh = child.GetComponent<PolyMeshController> ();
			if (mesh != null)
			{
				displayMeshes.Add(mesh);
			}
		}
	}

	public void setCrossSection(float gradient, float intercept, bool lessThan)
	{
		foreach (PolyMeshController mesh in displayMeshes)
		{
			mesh.setCrossSection(gradient, intercept, lessThan);
		}
	}
}
