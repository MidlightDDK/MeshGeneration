using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Triangle
{
    public Vector3 point1;
    public Vector3 point2;
    public Vector3 point3;
    public Color drawColor = Color.white;
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriangleMeshMono : MonoBehaviour
{
    [SerializeField] private bool m_Generate = false;
    [SerializeField] private List<Triangle> m_Triangles = new List<Triangle>();

    private void Start()
    {
        GenerateMesh();
    }

    private void Update()
    {
        if (m_Generate)
        {
            GenerateMesh();
        }
    }

    [ContextMenu("Generate Mesh")]
    public void GenerateMesh()
    {
        m_Generate = false;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.name = "Generated Triangle Mesh";

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        for (int i = 0; i < m_Triangles.Count; i++)
        {
            Triangle tri = m_Triangles[i];

            int startIndex = vertices.Count;

            vertices.Add(tri.point1);
            vertices.Add(tri.point2);
            vertices.Add(tri.point3);

            colors.Add(tri.drawColor);
            colors.Add(tri.drawColor);
            colors.Add(tri.drawColor);

            // Front face
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 2);

            // Back face
            triangles.Add(startIndex + 0);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetColors(colors);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}