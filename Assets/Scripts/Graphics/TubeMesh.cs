using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TubeMesh : MonoBehaviour
{
    readonly int kRingDivisions = 12;
    readonly float kWidth = .5f;

    MeshFilter m_meshFilter;
    Mesh m_mesh;
    Material m_material;

    List<Transform> m_nodes;
    List<int> m_indices;
    List<Vector3> m_positions;
    List<Vector3> m_normals;
    List<Vector2> m_uvs;

    List<int> m_ringStripIndices;

    private void Start()
    {
        m_nodes = new List<Transform>();
        m_indices = new List<int>();
        m_normals = new List<Vector3>();
        m_positions = new List<Vector3>();
        m_uvs = new List<Vector2>();

        m_meshFilter = GetComponent<MeshFilter>();
        m_mesh = new Mesh();
        m_meshFilter.sharedMesh = m_mesh;
        SetupRingStripIndices();
    }

    private void ClayUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Transform> nodes = new List<Transform>();
            int numChildren = transform.childCount;
            int iChild = 0;
            while (iChild < numChildren)
            {
                nodes.Add(transform.GetChild(iChild));
                iChild++;
            }
            SetNodes(nodes);
        }
    }

    private void SetupRingStripIndices()
    {
        m_ringStripIndices = new List<int>();
        int iRingVert = 0;
        while (iRingVert < kRingDivisions)
        {
            int startIndex = iRingVert;
            int endIndex = 1 + iRingVert;
            endIndex = endIndex % kRingDivisions;
            m_ringStripIndices.Add(startIndex);
            m_ringStripIndices.Add(startIndex + kRingDivisions);
            m_ringStripIndices.Add(endIndex);

            m_ringStripIndices.Add(endIndex + kRingDivisions);
            m_ringStripIndices.Add(endIndex);
            m_ringStripIndices.Add(startIndex + kRingDivisions);
            
            
            iRingVert++;
        }
    }

    public void SetNodes(List<Transform> nodes)
    {
        m_nodes = nodes;
        RecalculateMesh();
    }

    private void RecalculateMesh()
    {
        m_indices.Clear();
        m_positions.Clear();
        m_normals.Clear();
        m_uvs.Clear();

        int iRing = 0;
        while (iRing < m_nodes.Count)
        {
            Transform node = m_nodes[iRing];

            // create the vertex ring
            int iRingVert = 0;
            while (iRingVert < kRingDivisions)
            {
                float rotationPercent = ((float)iRingVert / (float)(kRingDivisions));
                float rotationRads = rotationPercent * 360;

                Vector3 vertNormal = node.localRotation * Quaternion.Euler(0, 0, rotationRads) * Vector3.right;
                vertNormal.Normalize();
                Vector3 vertPos = node.localPosition + vertNormal * kWidth;

                m_positions.Add(vertPos);
                m_normals.Add(vertNormal);
                m_uvs.Add(new Vector2(rotationPercent, (float)iRing));
                iRingVert++;
            }

            // connect to previous vertices
            if (iRing < m_nodes.Count - 1)
            {
                foreach (int iStrip in m_ringStripIndices)
                {
                    m_indices.Add(iStrip + iRing * kRingDivisions);
                }
            }

            iRing++;
        }

        m_mesh.MarkDynamic();
        m_mesh.SetVertices(m_positions);
        m_mesh.SetNormals(m_normals);
        m_mesh.SetIndices(m_indices.ToArray(), MeshTopology.Triangles, 0);
        m_mesh.SetUVs(0, m_uvs);
        m_mesh.RecalculateTangents();
        m_mesh.RecalculateBounds();
    }
}
