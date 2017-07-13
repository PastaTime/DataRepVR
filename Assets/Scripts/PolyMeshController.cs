using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PolyMeshController : MonoBehaviour {

	// Total number of verts to use in polymesh width. Defaults to 100.
	public int totalXVerts = 100;

	// Total number of verts to use in polymesh depth. Defaults to 100.
	public int totalZVerts = 100;

	public string colourDataPath = "";

	public string heightDataPath = "";

	public Color startColour =  Color.red;

	public Color endColour = Color.green;

	private float[][] colourData;

	private float[][] heightData;

	// Material to make all submeshes out of
	public Material renderMaterial;

	//Maximum number of verts permitted per side of submesh
	private int maxVertsPerMeshSide = 255;

	private float width;

	private float depth;

	// Use this for initialization
	void Start () {

		width = gameObject.transform.localScale.x;
		depth = gameObject.transform.localScale.z;

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

			int subXVerts = (totalXVerts / maxVertsPerMeshSide > i) ? maxVertsPerMeshSide : totalXVerts % maxVertsPerMeshSide;
			// xOffset is essentially number of verts from bottom-left corner to bottom-left corner of this mesh
			int xOffset = i * maxVertsPerMeshSide;

			for (int j = 0; j < zCount; j++) {
				
				int zOffset = j * maxVertsPerMeshSide;
				int subZVerts = (totalZVerts / maxVertsPerMeshSide > j) ? maxVertsPerMeshSide : totalZVerts % maxVertsPerMeshSide;

				GameObject subMesh = new GameObject ("subMesh");
				subMesh.AddComponent<VisualiserMesh>();

				//Set this submesh as a sub-component of the polymesh
				subMesh.transform.parent = gameObject.transform;

				// Calculate translated position for sub-mesh
				// Includes correction for edge sub-meshes which are smaller and so need to be translated slightly less
				// Also includes correction for mesh seams
				float xTranslate = (xOffset - i + 0.5f * subXVerts) / (float)totalXVerts;
				float zTranslate = (zOffset - j + 0.5f * subZVerts) / (float)totalZVerts;

				subMesh.transform.Translate (xTranslate - 0.5f, 0, zTranslate - 0.5f);

				subMesh.GetComponent<VisualiserMesh>().Init(this, i, j, subXVerts, subZVerts);
			}
		}
		// Scale polymesh up to original dimensions
		gameObject.transform.localScale = originalScale;
	}

	public float getWidth() {
		return width;
	}

	public float getDepth() { 
		return depth;
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
