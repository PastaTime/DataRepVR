using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyMeshController : MonoBehaviour {

	// Total number of verts to use in polymesh width. Defaults to 100.
	public int xVerts = 100;

	// Total number of verts to use in polymesh depth. Defaults to 100.
	public int zVerts = 100;

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

	private List<GameObject> subMeshes = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		
		heightData = LoadData.normaliseValues(LoadData.loadCSV(heightDataPath, false)); 
		colourData = LoadData.normaliseValues(LoadData.loadCSV(colourDataPath, false));

		//Hide parent object
		gameObject.GetComponent<MeshRenderer> ().enabled = false;

		Vector3 originalScale = gameObject.transform.localScale;
		// Scale down to a 1x1x1 cube while adding meshes
		gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);

		// Determine number of submeshes in x direction
		int xCount = xVerts / maxVertsPerMeshSide;
		xCount = (xVerts % maxVertsPerMeshSide == 0) ? xCount : xCount + 1;
		
		// Determine number of submeshes in z direction
		int zCount = zVerts / maxVertsPerMeshSide;
		zCount = (zVerts % maxVertsPerMeshSide == 0) ? zCount : zCount + 1;

		// xVertsRemaining is the number of verts that have not been allocated to a submesh in the x-direction
		int xVertsRemaining = xVerts;

		for (int i = 0; i < xCount; i++) {
			
			// xOffset is essentially number of verts from bottom-left corner to bottom-left corner of this mesh
			int xOffset = i * maxVertsPerMeshSide;
			int zVertsRemaining = zVerts;
			int numCols = (xVertsRemaining > maxVertsPerMeshSide) ? maxVertsPerMeshSide : xVertsRemaining;

			for (int j = 0; j < zCount; j++) {
				
				int zOffset = j * maxVertsPerMeshSide;
				int numRows = (zVertsRemaining > maxVertsPerMeshSide) ? maxVertsPerMeshSide : zVertsRemaining;
				zVertsRemaining -= numRows;

				GameObject subMesh = new GameObject ("subMesh");
				subMesh.AddComponent<SubMesh>();

				//Set this submesh as a sub-component of the polymesh
				subMesh.transform.parent = gameObject.transform;

				// Calculate translated position for sub-mesh
				// Includes correction for edge sub-meshes which are smaller and so need to be translated slightly less
				// Also includes correction for mesh seams
				float xTranslate = (xOffset - i + 0.5f * numCols) / (float)xVerts;
				float zTranslate = (zOffset - j + 0.5f * numRows) / (float)zVerts;

				subMesh.transform.Translate (xTranslate - 0.5f, 0, zTranslate - 0.5f);

				subMesh.GetComponent<SubMesh>().Init(this, xOffset, zOffset, numRows, numCols);
				subMeshes.Add(subMesh);
			}
			xVertsRemaining -= numCols;
		}
		// Scale polymesh up to original dimensions
		gameObject.transform.localScale = originalScale;
	}

	private class SubMesh : MonoBehaviour {

		private MeshFilter meshFilter;

		private int xOffset;

		private int zOffset;

		private int numRows;

		private int numCols;

		private PolyMeshController controller;

		public void Init(PolyMeshController controller, int xOffset, int zOffset, int numRows, int numCols) {

			this.controller = controller;

			this.xOffset = xOffset;
			this.zOffset = zOffset;

			this.numCols = numCols;
			this.numRows = numRows;

			meshFilter = gameObject.AddComponent<MeshFilter> ();
			gameObject.AddComponent<MeshRenderer> ();
			gameObject.GetComponent<MeshRenderer> ().material = controller.renderMaterial;

			meshFilter.mesh = prepareMesh((float)numCols / (float)controller.xVerts, (float)numRows / (float)controller.zVerts, numRows, numCols);
			colourAndDistortMesh (controller.heightData, controller.colourData);
		}

		/// <summary>
		/// Creates a Mesh with the specified dimensions around the origin (0,0,0).
		/// The Mesh will be orientated along the XZ Plane.
		/// </summary>
		/// <param name="meshWidth"> The width of the mesh in the game world (Along the X Axis).</param>
		/// <param name="meshDepth"> The depth of the mesh in the game world (Along the Z Axis).</param>
		/// <param name="numVertRows"> Number of rows of vertices in the mesh</param>
		/// <param name="numVertCols"> Number of columns of vertices in the mesh</param>
		/// <returns> The created Mesh</returns>
		public Mesh prepareMesh(float meshWidth, float meshDepth, int numVertRows, int numVertCols)
		{
			Debug.Assert(meshFilter != null, "Mesh Filter not delcared.");

			Mesh mesh = new Mesh();

			Vector3[] vertices = new Vector3[numVertRows * numVertCols];
			Vector2[] uvList = new Vector2[numVertRows * numVertCols];
			int index = 0;
			for (float i = 0f; i < numVertRows; i++)
			{
				for (float j = 0f; j < numVertCols; j++)
				{
					// Generate Vertices
					Vector3 vect = Vector3.zero;
					vect.x = Mathf.Lerp(-meshWidth / 2f, meshWidth / 2f, j / numVertCols);
					vect.z = Mathf.Lerp(-meshDepth / 2f, meshDepth / 2f, i / numVertRows);
					vertices[index] = vect;

					// Generate UVs
					Vector2 uv = Vector2.zero;
					uv.x = vect.x;
					uv.y = vect.y;
					uvList[index] = uv;
					index++;
				}
			}

			index = 0;
			// Number of triangles for one side of the mesh.
			int numTriangles = (numVertCols - 1) * (numVertRows - 1) * 6;
			int[] triangles = new int[numTriangles * 2];

			for (int i = 0; i < numVertRows - 1; i++)
			{
				for (int j = 0; j < numVertCols - 1; j++)
				{
					// Generate Triangle:
					// (i,j) <---- (i,j+1)
					//               ^
					//               |
					//               |
					//            (i+1,j+1)
					triangles[index++] = j + i * numVertCols;
					triangles[index++] = (j + 1) + (i + 1) * numVertCols;
					triangles[index++] = (j + 1) + i * numVertCols;

					// And for BackFace
					//triangles[index++] = j + i * numVertCols;
					//triangles[index++] = (j + 1) + i * numVertCols;
					//triangles[index++] = (j + 1) + (i + 1) * numVertCols;

					// Generate Triangle:
					//  (i,j)
					//    |            
					//    |
					//    V         
					// (i+1,j) -----> (i+1,j+1)
					triangles[index++] = j + i * numVertCols;
					triangles[index++] = j + (i + 1) * numVertCols;
					triangles[index++] = (j + 1) + (i + 1) * numVertCols;

					// And for BackFace
					//triangles[index++] = j + i * numVertCols;
					//triangles[index++] = (j + 1) + (i + 1) * numVertCols;
					//triangles[index++] = j + (i + 1) * numVertCols;
				}
			}

			mesh.vertices = vertices;
			mesh.uv = uvList;
			mesh.triangles = triangles;

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			return mesh;
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

			meshFilter.mesh.vertices = newVertices;
			meshFilter.mesh.colors = colours;
			meshFilter.mesh.RecalculateBounds();
			meshFilter.mesh.RecalculateNormals();
		}
	}
}
