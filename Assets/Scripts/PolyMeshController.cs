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

	private float[][] colourData;

	private float[][] heightData;

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
				subMesh.GetComponent<VisualiserMesh>().Init(this, i, j, subXVerts, subZVerts);
				
				//Set this submesh as a sub-component of the polymesh
				subMesh.transform.parent = gameObject.transform;
				subMesh.transform.position = subMesh.transform.parent.position;
				subMesh.transform.Translate (xTranslate - 0.5f, 0, zTranslate - 0.5f);
			}
		}
		// Scale polymesh up to original dimensions
		gameObject.transform.localScale = originalScale;
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
  
	public void colourAndDistortMesh(float[][] heightData, float[][] colourData) {
			Vector3[] newVertices = meshFilter.mesh.vertices;
			Color[] colours = new Color[newVertices.Length];

			// Loading into Mesh
			for (int z = 0; z < numRows; z++)
			{
				for (int x = 0; x < numCols; x++)
				{
					if (heightData != null) {
						// -0.5f puts mesh in middle of polymesh cuboid vertically
						// While this is running polymesh is 1x1x1 cube
						newVertices[x + z * numCols].y = heightData[heightData.Length * (z + zOffset) / controller.zVerts][heightData[0].Length * (x + xOffset) / controller.xVerts] + 0.375f;
					}
					/*
					if (colourData != null) {
						colours [x + z * numCols] = Colorx.Slerp (controller.startColour, controller.endColour, colourData [colourData.Length * (z + zOffset) / controller.zVerts][colourData[0].Length * (x + xOffset) / controller.xVerts]);
					} else {
						colours [x + z * numCols] = Color.grey;
					}
					*/
				}
			}


	public float[][] getColourData() {
		return colourData;
	}
}
