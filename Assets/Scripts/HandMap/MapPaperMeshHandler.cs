using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지도의 종이 메쉬 연산
/// </summary>
public class MapPaperMeshHandler
{
    private readonly float halfHeight;
    private readonly int SEG_HORZ = 5;
    private readonly int SEG_VERT = 5;

    public MapPaperMeshHandler(HandMapController controller, Transform handleLeft, Transform handleRight,
        float paperHeight, int segHorz, int segVert)
    {
        SEG_HORZ = segHorz; SEG_VERT = segVert;
        halfHeight = paperHeight * 0.5f;
        this.controller = controller;
        this.handleLeft = handleLeft;
        this.handleRight = handleRight;

        paperObject = new GameObject("MapPaper");
        paperObject.transform.SetParent(this.controller.transform);
        paperObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // create two rope: top and bottom
        ropeTop = new Rope(this.handleLeft.localPosition + this.handleLeft.up * halfHeight, this.handleRight.localPosition + this.handleRight.up * halfHeight, SEG_HORZ);
        ropeBtm = new Rope(this.handleLeft.localPosition - this.handleLeft.up * halfHeight, this.handleRight.localPosition - this.handleRight.up * halfHeight, SEG_HORZ);

        // initialize mesh
        points = new Vector3[SEG_HORZ, SEG_VERT];
        mesh = new Mesh() { name = "MapPaperMesh" };
        var meshFilter = paperObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        renderer = paperObject.AddComponent<MeshRenderer>();
        InitializeMesh();
    }

    private readonly HandMapController controller;

    private readonly Transform handleLeft;
    private readonly Transform handleRight;

    private readonly Rope ropeTop;
    private readonly Rope ropeBtm;

    private readonly GameObject paperObject;
    private readonly Mesh mesh;
    private readonly MeshRenderer renderer;
    private readonly Vector3[,] points;

    public MeshRenderer Renderer() => renderer;

    private void InitializeMesh()
    {
        List<int> tris = new();
        int i = 0;
        for (int u = 0; u < SEG_HORZ - 1; ++u)
            for (int v = 0; v < SEG_VERT - 1; ++v)
            {
                tris.Add(i + 0); tris.Add(i + 1); tris.Add(i + 2);
                tris.Add(i + 4); tris.Add(i + 3); tris.Add(i + 5);
                i += 6;
            }
        Update(); // this sets vertices and uvs
        mesh.SetTriangles(tris, 0);
    }

    /// <summary>
    /// 지도의 메쉬 업데이트
    /// </summary>
    public void Update()
    {
        Debug.DrawLine(handleLeft.position, handleRight.position);

        // Lerp b/w two ropes to get vertices
        #region GetVertices
        var dists = new float[SEG_HORZ, SEG_VERT];
        var vertsTop = ropeTop.GetSegmentsPos();
        for (int i = 0; i < SEG_HORZ; ++i) points[i, 0] = vertsTop[i];
        var vertsBtm = ropeBtm.GetSegmentsPos();
        for (int i = 0; i < SEG_HORZ; ++i) points[i, SEG_VERT - 1] = vertsBtm[i];
        for (int j = 0; j < SEG_VERT; ++j)
        {
            float ratio = (float)j / (SEG_VERT - 1);
            for (int i = 0; i < SEG_HORZ; ++i)
            {
                if (j > 0 && j < SEG_VERT - 1) points[i, j] = Vector3.Lerp(points[i, 0], points[i, SEG_VERT - 1], ratio);
                if (i > 0) dists[i, j] = dists[i - 1, j] + Vector3.Distance(points[i - 1, j], points[i, j]) / (halfHeight * 2f);
            }
        }
        #endregion GetVertices

        // update vertices position
        List<Vector3> verts = new();
        List<Vector2> uvs = new();
        for (int u = 0; u < SEG_HORZ - 1; ++u)
        {
            for (int v = 0; v < SEG_VERT - 1; ++v)
            {
                verts.Add(points[u, v]); verts.Add(points[u + 1, v]); verts.Add(points[u, v + 1]);
                verts.Add(points[u + 1, v]); verts.Add(points[u, v + 1]); verts.Add(points[u + 1, v + 1]);

                float um0 = dists[SEG_HORZ - 1, v] * 0.5f, um1 = dists[SEG_HORZ - 1, v + 1] * 0.5f,
                    u00 = dists[u, v] - um0, u01 = dists[u + 1, v] - um0,
                    u10 = dists[u, v + 1] - um1, u11 = dists[u + 1, v + 1] - um1;
                float v0 = 1f - (float)v / (SEG_VERT - 1), v1 = 1f - (float)(v + 1) / (SEG_VERT - 1);
                uvs.Add(new(u00, v0)); uvs.Add(new(u01, v0)); uvs.Add(new(u10, v1));
                uvs.Add(new(u01, v0)); uvs.Add(new(u10, v1)); uvs.Add(new(u11, v1));
            }
        }
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();
    }

    /// <summary>
    /// 로프 수동 물리 연산
    /// </summary>
    public void FixedUpdate()
    {
        // update two rope: top and bottom
        ropeTop.Simulate(handleLeft.localPosition + handleLeft.up * halfHeight, handleRight.localPosition + handleRight.up * halfHeight);
        ropeBtm.Simulate(handleLeft.localPosition - handleLeft.up * halfHeight, handleRight.localPosition - handleRight.up * halfHeight);
    }


    private class Rope
    {
        public Rope(Vector3 posLeft, Vector3 posRight, int segments)
        {
            segLength = Vector3.Distance(posLeft, posRight) / (segments - 1);
            this.segments = new Segment[segments];
            for (int i = 0; i < segments; ++i)
                this.segments[i] = new Segment(Vector3.Lerp(posLeft, posRight, (float)i / (segments - 1)));
        }

        private readonly Segment[] segments;
        private float segLength;

        public void Reset(Vector3 posLeft, Vector3 posRight)
        {
            segLength = Vector3.Distance(posLeft, posRight) / (segments.Length - 1);
            for (int i = 0; i < segments.Length; ++i)
            {
                segments[i].pos = Vector3.Lerp(posLeft, posRight, (float)i / (segments.Length - 1));
                segments[i].lastPos = segments[i].pos;
            }
        }

        public void Simulate(Vector3 posLeft, Vector3 posRight)
        {
            // TODO: simulate rope physics
            Reset(posLeft, posRight); // temp
        }

        public Vector3[] GetSegmentsPos()
            => Array.ConvertAll(segments, x => x.pos);

        private struct Segment
        {
            public Segment(Vector3 pos)
            {
                this.pos = pos;
                this.lastPos = pos;
            }

            public Vector3 pos, lastPos;
        }
    }
}