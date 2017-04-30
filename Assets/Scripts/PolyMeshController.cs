using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyMeshController : MonoBehaviour {

	public int xVerts = 100;

	public int zVerts = 100;

	public float xFudgeFactor = 0.0005f;

	public float zFudgeFactor = 0.0005f;

	public string colourDataPath = "";

	public string heightDataPath = "";

	public Color startColour =  Color.red;

	public Color endColour = Color.green;

	private float[][] colourData;

	private float[][] heightData;

	public float heightScalar = 1f;

	public Material renderMaterial;

	private int maxVertsPerMeshSide = 255;

	private List<GameObject> subMeshes = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		heightData = LoadData.normaliseValues(LoadData.loadCSV(heightDataPath, false)); 
		colourData = LoadData.normaliseValues(LoadData.loadCSV(colourDataPath, false));

		gameObject.GetComponent<MeshRenderer> ().enabled = false;

		int xCount = xVerts / maxVertsPerMeshSide;

		xCount = (xVerts % maxVertsPerMeshSide == 0) ? xCount : xCount + 1;
		int zCount = zVerts / maxVertsPerMeshSide;
		zCount = (zVerts % maxVertsPerMeshSide == 0) ? zCount : zCount + 1;

		int xVertsRemaining = xVerts;
		for (int i = 0; i < xCount; i++) {
			int xOffset = i * maxVertsPerMeshSide;
			int zVertsRemaining = zVerts;
			int numCols = (xVertsRemaining > maxVertsPerMeshSide) ? maxVertsPerMeshSide : xVertsRemaining;
			for (int j = 0; j < zCount; j++) {
				
				int zOffset = j * maxVertsPerMeshSide;
				int numRows = (zVertsRemaining > maxVertsPerMeshSide) ? maxVertsPerMeshSide : zVertsRemaining;
				zVertsRemaining -= numRows;

				GameObject subMesh = new GameObject ("subMesh");
				subMesh.AddComponent<MeshController>();
				subMesh.transform.parent = gameObject.transform;
				float xTranslate = ((float)(xOffset + maxVertsPerMeshSide) - 0.5f* (float)(maxVertsPerMeshSide - numCols)) / (float)xVerts;
				float zTranslate = ((float)(zOffset + maxVertsPerMeshSide) - 0.5f* (float)(maxVertsPerMeshSide - numRows)) / (float)zVerts;
				subMesh.transform.Translate (xTranslate - xFudgeFactor * i, 0, zTranslate - zFudgeFactor * j); // TODO: add submesh scaling factor to transform
				subMesh.GetComponent<MeshController>().Init(xOffset, zOffset, numRows, numCols, zVerts, xVerts, colourData, heightData, heightScalar, startColour, endColour, renderMaterial);
				subMeshes.Add(subMesh);
			}
			xVertsRemaining -= numCols;
		}
	}
	
	// Update is called once per frame
	void Update () {}
}
