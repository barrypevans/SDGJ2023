using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TubeMesh : MonoBehaviour
{
    readonly int kRingDivisions = 12;
    readonly float kWidth = .45f;
    readonly int kNumNodesToUpdate = 5;

    MeshFilter m_meshFilter;
    Mesh m_mesh;
    Material m_material;

    List<Transform> m_nodes;
    List<int> m_indices;
    List<Vector3> m_positions;
    List<Vector3> m_normals;
    List<Vector2> m_uvs;

    List<int> m_ringStripIndices;

    bool needsUpdate = false;
    bool canUpdate = true;

    private void Awake()
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
        canUpdate = true;
    }

    private void UpdateMesh()
    {

            m_mesh.MarkDynamic();
             m_mesh.Clear();
            m_mesh.vertices = m_positions.ToArray();
            m_mesh.normals = m_normals.ToArray();
            m_mesh.uv = m_uvs.ToArray();
            m_mesh.SetIndices(m_indices.ToArray(), MeshTopology.Triangles, 0);
            m_mesh.RecalculateTangents();
            m_mesh.RecalculateBounds();


    }


    private void OnRenderObject()
    {
        if (canUpdate && needsUpdate)
        {
            UpdateMesh();
            needsUpdate = false;
            canUpdate = false;
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


    public void PopNode()
    {
        if (m_nodes.Count <= 1)
        {
            m_nodes.Clear();
            m_indices.Clear();
            m_positions.Clear();
            m_normals.Clear();
            m_uvs.Clear();
        }
        else
        {
            m_nodes.RemoveAt(m_nodes.Count - 1);
            int indicesToRemove = (6 * kRingDivisions);
            int vertsToRemove = kRingDivisions;
            m_indices.RemoveRange((m_indices.Count - indicesToRemove), indicesToRemove);
            m_positions.RemoveRange((m_positions.Count - vertsToRemove), vertsToRemove);
            m_normals.RemoveRange((m_normals.Count - vertsToRemove), vertsToRemove);
            m_uvs.RemoveRange((m_uvs.Count - vertsToRemove), vertsToRemove);
        }
        needsUpdate = true;
    }

    public void PushNode(Transform node)
    {
        m_nodes.Add(node);
        needsUpdate = false;
        int iRing = m_nodes.Count - 1;
        // while (iRing < m_nodes.Count)
        {
            // create the vertex ring
            int iRingVert = 0;
            while (iRingVert < kRingDivisions)
            {
                float rotationPercent = ((float)iRingVert / (float)(kRingDivisions));
                float rotationRads = rotationPercent * 360;

                Vector3 vertNormal = (node.localRotation * Quaternion.Euler(0, 0, rotationRads) * Vector3.right);
                vertNormal.Normalize();
                Vector3 vertPos = (node.localPosition + vertNormal * kWidth);

                m_positions.Add(vertPos);
                m_normals.Add(vertNormal);
                m_uvs.Add(new Vector2(rotationPercent, ((float)iRing) / 20.0f));
                iRingVert++;
            }

            // connect to previous vertices
            if (iRing > 0)
            {
                foreach (int iStrip in m_ringStripIndices)
                {
                    m_indices.Add(iStrip + (iRing - 1) * kRingDivisions);
                }
            }

        }

        needsUpdate = true;
    }
}
