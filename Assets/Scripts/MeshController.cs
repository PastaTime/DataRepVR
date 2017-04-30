using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour {

    private MeshFilter meshFilter = null;

	private MeshRenderer meshRenderer = null;

	private int xOffset;

	private int zOffset;

	private int totalRows;

	private int totalCols;

    private int numRows;

    private int numCols;

	private Color startColour;

	private Color endColour;

	private float[][] colourData;

	private float[][] heightData;

	private float heightScalar = 1f; 

    // Internal values for the current number of rows and columns of in the
    private int vertRows = 0;

    private int vertCols = 0;

	public void Init(int xOffset, int zOffset, int numRows, int numCols, int totalRows, int totalCols, float[][] colourData, float[][] heightData, float heightScalar, Color startColour, Color endColour, Material renderMaterial) {
		this.xOffset = xOffset;
		this.zOffset = zOffset;

		this.numCols = numCols;
		this.numRows = numRows;

		this.totalRows = totalRows;
		this.totalCols = totalCols;

		this.colourData = colourData;
		this.heightData = heightData;

		this.heightScalar = heightScalar;

		this.startColour = startColour;
		this.endColour = endColour;

		meshFilter = gameObject.AddComponent<MeshFilter> ();
		meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		gameObject.GetComponent<MeshRenderer>().material = renderMaterial; 

		gameObject.AddComponent<MeshCollider> ();
		meshFilter.mesh = initMesh((float)numCols / (float)totalCols, (float)numRows / (float)totalRows, numRows, numCols);
		colourAndDistortMesh (heightData, colourData);
	}

    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update() {}

    /// <summary>
    /// Creates a Mesh with the specified dimensions around the origin (0,0,0).
    /// The Mesh will be orientated along the XZ Plane.
    /// </summary>
    /// <param name="meshWidth"> The width of the mesh in the game world (Along the X Axis).</param>
    /// <param name="meshDepth"> The depth of the mesh in the game world (Along the Z Axis).</param>
    /// <param name="numVertRows"> Number of rows of vertices in the mesh</param>
    /// <param name="numVertCols"> Number of columns of vertices in the mesh</param>
    /// <returns> The created Mesh</returns>
    public Mesh initMesh(float meshWidth, float meshDepth, int numVertRows, int numVertCols)
    {
        Debug.Assert(meshFilter != null, "Mesh Filter not delcared.");

        vertRows = numVertRows;
        vertCols = numVertCols;

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
                triangles[index++] = j + i * numVertCols;
                triangles[index++] = (j + 1) + i * numVertCols;
                triangles[index++] = (j + 1) + (i + 1) * numVertCols;

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
                triangles[index++] = j + i * numVertCols;
                triangles[index++] = (j + 1) + (i + 1) * numVertCols;
                triangles[index++] = j + (i + 1) * numVertCols;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvList;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    /// <summary>
    /// Updates the heights of vertices in the mesh based on a 2D Height Map of floats.
    /// The height map must have the same dimensions as the vertices in the Mesh otherwise an assertion error is thrown.
    /// </summary>
    /// <param name="heightMap"></param>
    public void updateMesh(float[][] heightMap)
    {
        Debug.Assert(vertRows != 0, "Mesh has not been initialized yet, please call initMesh()");
        Debug.Assert(vertCols != 0, "Mesh has not been initialized yet, please call initMesh()");
        Debug.Assert(heightMap.Length == vertRows, "Height Map of incorrect row dimensions. cannot be loaded into mesh. ");
        Debug.Assert(heightMap[0].Length == vertCols, "Height Map of incorrect column dimensions. cannot be loaded into mesh");

        Vector3[] newVertices = meshFilter.mesh.vertices;

        // Loading into Mesh
        for (int i = 0; i < vertRows; i++)
        {
            for (int j = 0; j < vertCols; j++)
            {
                newVertices[j + i * vertCols].y = heightMap[i][j];
            }
        }

        meshFilter.mesh.vertices = newVertices;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
    }

	public void colourAndDistortMesh(float[][] heightData, float[][] colourData) {
		Debug.Assert(vertRows != 0, "Mesh has not been initialized yet, please call initMesh()");
		Debug.Assert(vertCols != 0, "Mesh has not been initialized yet, please call initMesh()");

		Vector3[] newVertices = meshFilter.mesh.vertices;
		Color[] colours = new Color[newVertices.Length];

		// Loading into Mesh
		for (int z = 0; z < vertRows; z++)
		{
			for (int x = 0; x < vertCols; x++)
			{
				if (heightData != null) {
					newVertices[x + z * vertCols].y = heightData[heightData.Length * (z + zOffset) / totalRows][heightData[0].Length * (x + xOffset) / totalCols] * heightScalar;
				}
				if (colourData != null) {
					colours [x + z * vertCols] = Colorx.Slerp (startColour, endColour, colourData [colourData.Length * (z + zOffset) / totalRows][colourData[0].Length * (x + xOffset) / totalCols]);
				} else {
					colours [x + z * vertCols] = Color.magenta;
				}
			}
		}

		meshFilter.mesh.vertices = newVertices;
		meshFilter.mesh.colors = colours;
		meshFilter.mesh.RecalculateBounds();
		meshFilter.mesh.RecalculateNormals();
		//GetComponent<MeshCollider> ().sharedMesh = meshFilter.mesh;
	}
}
