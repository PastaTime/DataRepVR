using System.Collections.Generic;
using UnityEngine;

public class PolyMeshController : MonoBehaviour {

	// Total number of verts to use in polymesh width. Defaults to 100.
	public int totalXVerts = 100;

	// Total number of verts to use in polymesh depth. Defaults to 100.
	public int totalZVerts = 100;

	public string colourDataPath = "";

	public string heightDataPath = "";

	public Color startColour =  Color.red;

	public Color endColour = Color.green;

	public bool activeOnLoad = true;

	private float[][] colourData;

	private float[][] heightData;

	private List<VisualiserMesh> subMeshes = new List<VisualiserMesh>();

	// Material to make all submeshes out of
	public Material renderMaterial;

	//Maximum number of verts permitted per side of submesh
	private int maxVertsPerMeshSide = 255;

	// Use this for initialization
	void Start () {
		// If the same data source is being used for height and colour no need to load it twice from disk
		if (heightDataPath.Equals (colourDataPath)) {
			heightData = colourData = LoadData.normaliseValues(LoadData.loadCSV(heightDataPath, false)); 
		} else {
			heightData = LoadData.normaliseValues(LoadData.loadCSV(heightDataPath, false)); 
			colourData = LoadData.normaliseValues(LoadData.loadCSV(colourDataPath, false));
		}

		//Hide parent object
		gameObject.GetComponent<MeshRenderer> ().enabled = false;

		Vector3 originalScale = gameObject.transform.localScale;
		// Scale down to a 1x1x1 cube while adding meshes
		gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);

		// Determine number of submeshes in x direction
		int xCount = totalXVerts / maxVertsPerMeshSide;
		xCount = (totalXVerts % maxVertsPerMeshSide == 0) ? xCount : xCount + 1;
		
		// Determine number of submeshes in z direction
		int zCount = totalZVerts / maxVertsPerMeshSide;
		zCount = (totalZVerts % maxVertsPerMeshSide == 0) ? zCount : zCount + 1;

		for (int i = 0; i < xCount; i++) {

			// Determine the maximum number of verts to use for this sub-mesh while not exceeding the max verts possible or vertex count
			int subXVerts = (totalXVerts / maxVertsPerMeshSide > i) ? maxVertsPerMeshSide : totalXVerts % maxVertsPerMeshSide;
			// xOffset is essentially number of verts from bottom-left corner to bottom-left corner of this mesh
			int xOffset = i * maxVertsPerMeshSide;
			float xTranslate = (xOffset + 0.5f * subXVerts) / totalXVerts;
			
			for (int j = 0; j < zCount; j++) {
				
				int zOffset = j * maxVertsPerMeshSide;
				int subZVerts = (totalZVerts / maxVertsPerMeshSide > j) ? maxVertsPerMeshSide : totalZVerts % maxVertsPerMeshSide;
				float zTranslate = (zOffset + 0.5f * subZVerts) / totalZVerts;
				
				GameObject subMesh = new GameObject ("subMesh");
				subMesh.AddComponent<VisualiserMesh>();
				
				//Set this submesh as a sub-component of the polymesh
				subMesh.transform.parent = gameObject.transform;
				subMesh.transform.position = subMesh.transform.parent.position;
				subMesh.transform.Translate (xTranslate - 0.5f, 0, zTranslate - 0.5f);

				VisualiserMesh sub = subMesh.GetComponent<VisualiserMesh> ();
				sub.Init(this, i, j, subXVerts, subZVerts);
				subMeshes.Add(sub);
			}
		}
		// Scale polymesh up to original dimensions
		gameObject.transform.localScale = originalScale;
		gameObject.SetActive (activeOnLoad);
	}

	public void setCrossSection(float m, float c, bool lessThan)
	{
		foreach (VisualiserMesh subMesh in subMeshes)
		{
			subMesh.setCrossSection(m,c, lessThan);
		}
	}

	public float getWidth()
	{
		return gameObject.transform.localScale.x;
	}

	public float getDepth()
	{
		return gameObject.transform.localScale.z;
	}

	public int getMaxVerts() {
		return maxVertsPerMeshSide;
	}

	public float[][] getHeightData() {
		return heightData;
	}

	public float[][] getColourData() {
		return colourData;
	}
}