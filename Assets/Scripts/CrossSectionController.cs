using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionController : MonoBehaviour
{
	private List<PolyMeshController> displayMeshes = new List<PolyMeshController>();

	private float _gradient;
	private float _intercept;
	private bool _lessThan;

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
		setGradient (gradient);
		setIntercept (intercept);
		setSelectionSide (lessThan);
		refreshCrossSection ();
	}


	public void refreshCrossSection() {
		foreach (PolyMeshController mesh in displayMeshes)
		{
			mesh.setCrossSection(_gradient, _intercept, _lessThan);
		}
	}

	public void setGradient(float gradient) {
		_gradient = gradient;
	}

	public void setIntercept(float intercept) {
		_intercept = intercept;
	}

	public void setSelectionSide(bool lessThan) {
		_lessThan = lessThan;
	}
}
