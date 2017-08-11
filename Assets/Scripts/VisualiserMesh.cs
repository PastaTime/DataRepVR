using System.Collections;
using UnityEngine;
using UnityEditor;

public class VisualiserMesh : MonoBehaviour
{
    private MeshFilter meshFilter;

    private int xPos;

    private int zPos;

    private int xVerts;

    private int zVerts;

    private Color[] vertexColours;

    private PolyMeshController controller;

    public void Init(PolyMeshController controller, int xPos, int zPos, int xVerts, int zVerts)
    {
        this.controller = controller;

        this.xPos = xPos;
        this.zPos = zPos;

        this.xVerts = xVerts;
        this.zVerts = zVerts;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = controller.renderMaterial;

        meshFilter.mesh = prepareMesh();
        colourAndDistortMesh();
        resizeMesh();
    }

    // Not an update method, only needs to be called on an actual change
    public IEnumerator setCrossSection(float m, float c, bool lessThan)
    {
        if (vertexColours != null)
        {
            Color[] colours = meshFilter.mesh.colors;
            Vector3[] verts = meshFilter.mesh.vertices;
            for (int z = 0; z < zVerts; z++)
            {
                for (int x = 0; x < xVerts; x++)
                {
                    Vector3 vert = transform.TransformPoint(verts[x + z * xVerts]);
					if ((lessThan && vert.z > (m * vert.x + c)) || (!lessThan && vert.z < (m * vert.x + c)))
                    {
                        colours[x + z * xVerts] = Color.black;
                    }
                    else if (colours[x + z * xVerts] == Color.black)
                    {
                        colours[x + z * xVerts] = vertexColours[x + z * xVerts];
                    }
                }
                yield return null;
            }
            meshFilter.mesh.colors = colours;
        }
    }

    private void resizeMesh()
    {
        Bounds bounds = meshFilter.mesh.bounds;
        float xScaling = ((float) xVerts / controller.totalXVerts) / bounds.size.x;
        float zScaling = ((float) zVerts / controller.totalZVerts) / bounds.size.z;
        meshFilter.transform.localScale = new Vector3(xScaling, 1f, zScaling);
        meshFilter.mesh.RecalculateBounds();
    }

    /// <summary>
    /// Creates a Mesh with the specified dimensions around the origin (0,0,0).
    /// The Mesh will be orientated along the XZ Plane.
    /// </summary>
    /// <param name="zVerts"> Number of rows of vertices in the mesh</param>
    /// <param name="xVerts"> Number of columns of vertices in the mesh</param>
    /// <returns> The created Mesh</returns>
    public Mesh prepareMesh()
    {
		if (!AssetDatabase.IsValidFolder ("Assets/Meshes")) {
			Debug.Log ("Created");
			AssetDatabase.CreateFolder ("Assets", "Meshes");
		}
        string meshName = "xVerts" + zVerts + "zVerts" + xVerts + ".asset";
        Mesh mesh = (Mesh) AssetDatabase.LoadAssetAtPath("Assets/Meshes/" + meshName, typeof(Mesh));
        if (mesh == null)
        {
            mesh = new Mesh();
            float meshWidth = (float) xVerts / controller.totalXVerts;
            float meshDepth = (float) zVerts / controller.totalZVerts;

            Vector3[] vertices = new Vector3[zVerts * xVerts];
            Vector2[] uvList = new Vector2[zVerts * xVerts];
            int index = 0;
            for (float i = 0f; i < zVerts; i++)
            {
                for (float j = 0f; j < xVerts; j++)
                {
                    // Generate Vertices
                    Vector3 vect = Vector3.zero;
                    vect.x = Mathf.Lerp(-meshWidth / 2f, meshWidth / 2f, j / xVerts);
                    vect.z = Mathf.Lerp(-meshDepth / 2f, meshDepth / 2f, i / zVerts);
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
            int numTriangles = (xVerts - 1) * (zVerts - 1) * 6;
            int[] triangles = new int[numTriangles * 2];

            for (int i = 0; i < zVerts - 1; i++)
            {
                for (int j = 0; j < xVerts - 1; j++)
                {
                    // Generate Triangle:
                    // (i,j) <---- (i,j+1)
                    //               ^
                    //               |
                    //               |
                    //            (i+1,j+1)
                    triangles[index++] = j + i * xVerts;
                    triangles[index++] = (j + 1) + (i + 1) * xVerts;
                    triangles[index++] = (j + 1) + i * xVerts;

                    // Generate Triangle:
                    //  (i,j)
                    //    |            
                    //    |
                    //    V         
                    // (i+1,j) -----> (i+1,j+1)
                    triangles[index++] = j + i * xVerts;
                    triangles[index++] = j + (i + 1) * xVerts;
                    triangles[index++] = (j + 1) + (i + 1) * xVerts;
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

    public void colourAndDistortMesh()
    {
        float[][] heightData = controller.getHeightData();
        float[][] colourData = controller.getColourData();

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
        meshFilter.mesh.vertices = newVertices;
        vertexColours = colours;
        meshFilter.mesh.colors = colours;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
    }
}