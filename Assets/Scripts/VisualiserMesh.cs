using System;
using UnityEngine;
using UnityEditor;


public class VisualiserMesh : MonoBehaviour
{
    private MeshFilter meshFilter;

    private int xPos;

    private int zPos;

    private int xVerts;

    private int zVerts;

    private float meshWidth;

    private float meshDepth;

    private PolyMeshController controller;

    public void Init(PolyMeshController controller, int xPos, int zPos, int xVerts, int zVerts)
    {
        //			Stopwatch timer = new Stopwatch ();
        //			timer.Start ();
        this.controller = controller;

        this.xPos = xPos;
        this.zPos = zPos;

        this.xVerts = xVerts;
        this.zVerts = zVerts;

        this.meshWidth = controller.getWidth() * xVerts / controller.totalXVerts;
        this.meshDepth = controller.getDepth() * zVerts / controller.totalZVerts;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = controller.renderMaterial;

        meshFilter.mesh = prepareMesh(xVerts, zVerts);
        colourAndDistortMesh(controller.getHeightData(), controller.getColourData());
        //			timer.Stop ();
        //			UnityEngine.Debug.Log ("Init time: " + timer.ElapsedMilliseconds);
    }

    /// <summary>
    /// Creates a Mesh with the specified dimensions around the origin (0,0,0).
    /// The Mesh will be orientated along the XZ Plane.
    /// </summary>
    /// <param name="numVertRows"> Number of rows of vertices in the mesh</param>
    /// <param name="numVertCols"> Number of columns of vertices in the mesh</param>
    /// <returns> The created Mesh</returns>
    public Mesh prepareMesh(int numVertCols, int numVertRows)
    {
        string meshName = "Rows" + numVertRows + "Cols" + numVertCols + ".asset";
        Mesh mesh = (Mesh) AssetDatabase.LoadAssetAtPath("Assets/Meshes/" + meshName, typeof(Mesh));
        if (mesh == null)
        {
            mesh = new Mesh();

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

                    // Generate Triangle:
                    //  (i,j)
                    //    |            
                    //    |
                    //    V         
                    // (i+1,j) -----> (i+1,j+1)
                    triangles[index++] = j + i * numVertCols;
                    triangles[index++] = j + (i + 1) * numVertCols;
                    triangles[index++] = (j + 1) + (i + 1) * numVertCols;
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uvList;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            AssetDatabase.CreateAsset(mesh, "Assets/Meshes/" + meshName);
            AssetDatabase.SaveAssets();
        }
        return mesh;
    }

    public void colourAndDistortMesh(float[][] heightData, float[][] colourData)
    {
        Vector3[] newVertices = meshFilter.mesh.vertices;
        Color[] colours = new Color[newVertices.Length];
        int xOffset = xPos * controller.getMaxVerts();
        int zOffset = zPos * controller.getMaxVerts();
        // Loading into Mesh
        if (heightData != null)
        {
            for (int z = 0; z < zVerts; z++)
            {
                for (int x = 0; x < xVerts; x++)
                {
                    newVertices[x + z * xVerts].y =
                        heightData[heightData.Length * (z + zOffset) / controller.totalZVerts][
                            heightData[0].Length * (x + xOffset) / controller.totalXVerts] + 0.375f;
                }
            }
        }
        //			Stopwatch colourTimer = new Stopwatch();
        //			colourTimer.Start ();
        if (colourData != null)
        {
            for (int z = 0; z < zVerts; z++)
            {
                for (int x = 0; x < xVerts; x++)
                {
                    colours[x + z * xVerts] = Colorx.Slerp(controller.startColour, controller.endColour,
                        colourData[colourData.Length * (z + zOffset) / controller.totalZVerts][
                            colourData[0].Length * (x + xOffset) / controller.totalXVerts]);
                }
            }
        }
        //			colourTimer.Stop ();
        //			UnityEngine.Debug.Log ("colournDistortcolour: " + colourTimer.ElapsedMilliseconds);
		meshFilter.mesh.vertices = newVertices;
        meshFilter.mesh.colors = colours;
        //			meshFilter.mesh.RecalculateBounds();
        //			meshFilter.mesh.RecalculateNormals();
    }
}