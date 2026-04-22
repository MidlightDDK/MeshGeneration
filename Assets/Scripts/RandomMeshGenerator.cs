using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RandomMeshGenerator : MonoBehaviour
{
    [SerializeField] private bool m_Generate = false;
    [Header("Generation Settings")]
    [SerializeField] private int vertexCount = 30;
    [SerializeField] private int triangleCount = 20;
    [SerializeField] private float radius = 3f;

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
        
        Mesh mesh = new Mesh();
        mesh.name = "Random Outward Mesh";

        Vector3[] vertices = GenerateVertices();
        Vector3 center = ComputeCenter(vertices);

        List<int> triangles = new List<int>();

        for (int i = 0; i < triangleCount; i++)
        {
            int a = Random.Range(0, vertexCount);
            int b = Random.Range(0, vertexCount);
            int c = Random.Range(0, vertexCount);

            while (b == a)
                b = Random.Range(0, vertexCount);

            while (c == a || c == b)
                c = Random.Range(0, vertexCount);

            Vector3 v0 = vertices[a];
            Vector3 v1 = vertices[b];
            Vector3 v2 = vertices[c];

            Vector3 triangleCenter = (v0 + v1 + v2) / 3f;
            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;
            Vector3 outwardDirection = (triangleCenter - center).normalized;

            // If the normal points inward, flip the triangle
            if (Vector3.Dot(normal, outwardDirection) < 0f)
            {
                int temp = b;
                b = c;
                c = temp;
            }

            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private Vector3[] GenerateVertices()
    {
        Vector3[] vertices = new Vector3[vertexCount];

        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 dir = Random.onUnitSphere;
            float dist = Random.Range(radius * 0.4f, radius);
            vertices[i] = dir * dist;
        }

        return vertices;
    }

    private Vector3 ComputeCenter(Vector3[] vertices)
    {
        Vector3 center = Vector3.zero;

        for (int i = 0; i < vertices.Length; i++)
            center += vertices[i];

        return center / vertices.Length;
    }
}