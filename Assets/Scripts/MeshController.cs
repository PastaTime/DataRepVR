using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour {

    public MeshFilter meshFilter = null;

    // Public variables for test Mesh.
    public float meshWidth = 20f;
    public float meshHeight = 20f;
    public int numRows = 10;
    public int numCols = 10;

	public float heightScalar = 1f; 

    // Internal values for the current number of rows and columns of in the
    private int vertRows = 0;
    private int vertCols = 0;

    // Use this for initialization
    void Start () {
        Debug.Assert(meshFilter != null, "Mesh Filter not delcared.");
        meshFilter.mesh = initMesh(meshWidth, meshHeight, numRows, numCols);
		colourAndDistortMesh(LoadTempData.normaliseValues(LoadTempData.loadCSV("/Data/Temperature_surface.csv", false)));
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Creates a Mesh with the specified dimensions around the origin (0,0,0).
    /// The Mesh will be orientated along the XZ Plane.
    /// </summary>
    /// <param name="meshWidth"> The width of the mesh in the game world (Along the X Axis).</param>
    /// <param name="meshDepth"> The depth of the mesh in the game world (Along the Z Axis).</param>
    /// <param name="numRows"> Number of rows of vertices in the mesh</param>
    /// <param name="numCols"> Number of columns of vertices in the mesh</param>
    /// <returns> The created Mesh</returns>
    public Mesh initMesh(float meshWidth, float meshDepth, int numRows, int numCols)
    {
        Debug.Assert(meshFilter != null, "Mesh Filter not delcared.");

        vertRows = numRows;
        vertCols = numCols;

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[numRows * numCols];
        Vector2[] uvList = new Vector2[numRows * numCols];
        int index = 0;
        for (float i = 0f; i < numRows; i++)
        {
            for (float j = 0f; j < numCols; j++)
            {
                // Generate Vertices
                Vector3 vect = Vector3.zero;
                vect.x = Mathf.Lerp(-meshWidth / 2f, meshWidth / 2f, j / numCols);
                vect.z = Mathf.Lerp(-meshDepth / 2f, meshDepth / 2f, i / numRows);
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
        int numTriangles = (numCols - 1) * (numRows - 1) * 6;
        int[] triangles = new int[numTriangles * 2];

        for (int i = 0; i < numRows - 1; i++)
        {
            for (int j = 0; j < numCols - 1; j++)
            {
                // Generate Triangle:
                // (i,j) <---- (i,j+1)
                //               ^
                //               |
                //               |
                //            (i+1,j+1)
                triangles[index++] = j + i * numCols;
                triangles[index++] = (j + 1) + (i + 1) * numCols;
                triangles[index++] = (j + 1) + i * numCols;

                // And for BackFace
                triangles[index++] = j + i * numCols;
                triangles[index++] = (j + 1) + i * numCols;
                triangles[index++] = (j + 1) + (i + 1) * numCols;

                // Generate Triangle:
                //  (i,j)
                //    |            
                //    |
                //    V         
                // (i+1,j) -----> (i+1,j+1)
                triangles[index++] = j + i * numCols;
                triangles[index++] = j + (i + 1) * numCols;
                triangles[index++] = (j + 1) + (i + 1) * numCols;

                // And for BackFace
                triangles[index++] = j + i * numCols;
                triangles[index++] = (j + 1) + (i + 1) * numCols;
                triangles[index++] = j + (i + 1) * numCols;
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

	public void colourAndDistortMesh(float[][] data) {
		Debug.Assert(vertRows != 0, "Mesh has not been initialized yet, please call initMesh()");
		Debug.Assert(vertCols != 0, "Mesh has not been initialized yet, please call initMesh()");
		//Debug.Assert(data.Length == vertRows, "Height Map of incorrect row dimensions. cannot be loaded into mesh. ");
		//Debug.Assert(data[0].Length == vertCols, "Height Map of incorrect column dimensions. cannot be loaded into mesh");

		Vector3[] newVertices = meshFilter.mesh.vertices;
		Debug.Log (meshFilter.mesh.vertices.Length);
		Color[] colours = new Color[newVertices.Length];
		// Loading into Mesh
		for (int i = 0; i < vertRows; i++)
		{
			for (int j = 0; j < vertCols; j++)
			{
				newVertices[j + i * vertCols].y = data[i][j] * heightScalar;
				colours [j + i * vertCols] = Color.Lerp (Color.green, Color.red, data [i] [j]);
			}
		}

		meshFilter.mesh.vertices = newVertices;
		meshFilter.mesh.colors = colours;
		meshFilter.mesh.RecalculateBounds();
		meshFilter.mesh.RecalculateNormals();
	}
}
