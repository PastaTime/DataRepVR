using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSectionController : MonoBehaviour
{
	private List<PolyMeshController> displayMeshes = new List<PolyMeshController>();

	public GameObject rotator;

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

	public void setCrossSection(Vector2 p1, Vector2 p2, bool lessThan)
	{
		float yRotation = rotator.transform.eulerAngles.y;
		Debug.Log(yRotation);
		p1 = p1.Rotate(-1 * yRotation);
		p2 = p2.Rotate(-1 * yRotation);

		float gradient = (p2.y - p1.y) / (p2.x - p1.x);
		float intercept = p1.y - gradient * p1.x;
		
		bool tempLessThan = lessThan;
//		if ((gradient > 0 && yRotation > 135f && yRotation < 310f) || (gradient < 0 && (yRotation > 45f && yRotation < 220f)))
//		{
//			tempLessThan = !tempLessThan;
//		}

		setGradient (gradient);
		Debug.Log("grad:" + gradient);
		setIntercept (intercept);
		setSelectionSide (tempLessThan);
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
