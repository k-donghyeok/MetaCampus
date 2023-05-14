using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지도의 종이 메쉬 연산
/// </summary>
public class MapPaperMeshHandler
{
    private readonly int SEG_HORZ = 5;
    private readonly int SEG_VERT = 5;

    [Serializable]
    public struct PaperInfo
    {
        [Range(2, 50)]
        public int segHorz;
        [Range(2, 50)]
        public int segVert;
        [Range(0.5f, 1.5f)]
        public float loseness;
        [Range(0f, 1f)]
        public float tension;
        [Range(0f, 1f)]
        public float gravity;
        [Range(0f, 10f)]
        public float velClamp;
    }

    public MapPaperMeshHandler(HandMapController controller, Transform handleLeft, Transform handleRight, PaperInfo info)
    {
        SEG_HORZ = info.segHorz; SEG_VERT = info.segVert;
        Rope.tension = info.tension; Rope.loseness = info.loseness; Rope.gravity = info.gravity / SEG_HORZ; Rope.velClamp = info.velClamp;
        this.controller = controller;
        this.handleLeft = handleLeft;
        this.handleRight = handleRight;

        // 종이가 들어갈 GameObject 생성
        paperObject = new GameObject("MapPaper");
        //paperObject.transform.SetParent(this.controller.transform);
        paperObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // 위와 아래 로프 생성
        ropeTop = new Rope(this.handleLeft.position, this.handleRight.position, SEG_HORZ);
        ropeBtm = new Rope(this.handleLeft.position, this.handleRight.position, SEG_HORZ);

        // 메쉬 초기화
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

    public void SetMaterial(Material material) => renderer.material = material;

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
        Update(0f); // this sets vertices and uvs
        mesh.SetTriangles(tris, 0);
    }

    /// <summary>
    /// 지도의 메쉬 업데이트
    /// </summary>
    public void Update(float height)
    {
        // Debug.DrawLine(handleLeft.position, handleRight.position);

        // 가운데 구간 버텍스의 위치를 선형 보간으로 구하기
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
                if (i > 0) dists[i, j] = dists[i - 1, j] + Vector3.Distance(points[i - 1, j], points[i, j]) / height;
            }
        }
        #endregion GetVertices

        // 메쉬의 버텍스와 UV를 갱신
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
    public void FixedUpdate(float height)
    {
        float halfHeight = height * 0.5f;
        ropeTop.Simulate(handleLeft.position + handleLeft.up * halfHeight, handleRight.position + handleRight.up * halfHeight);
        ropeBtm.Simulate(handleLeft.position - handleLeft.up * halfHeight, handleRight.position - handleRight.up * halfHeight);
    }


    /// <summary>
    /// 로프 시뮬레이션
    /// (참조: https://youtu.be/FcnvwtyxLds)
    /// </summary>
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
        public static float loseness = 1f;
        public static float tension = 1f;
        public static float gravity = 1f;
        public static float velClamp = 1f;

        /// <summary>
        /// 로프의 위치를 선형 보간으로 리셋
        /// </summary>
        public void Reset(Vector3 posLeft, Vector3 posRight)
        {
            segLength = loseness * Vector3.Distance(posLeft, posRight) / (segments.Length - 1);
            for (int i = 0; i < segments.Length; ++i)
            {
                segments[i].pos = Vector3.Lerp(posLeft, posRight, (float)i / (segments.Length - 1));
                segments[i].lastPos = segments[i].pos;
            }
        }

        /// <summary>
        /// 로프 물리 기반 시뮬레이션
        /// </summary>
        public void Simulate(Vector3 posLeft, Vector3 posRight)
        {
            //Reset(posLeft, posRight); return;
            segLength = loseness * Vector3.Distance(posLeft, posRight) / (segments.Length - 1);
            var grav = gravity * Time.fixedDeltaTime * Physics.gravity;
            for (int i = 0; i < segments.Length; ++i)
            {
                var vel = segments[i].pos - segments[i].lastPos;
                segments[i].lastPos = segments[i].pos;
                vel = Vector3.ClampMagnitude(vel + grav, velClamp * segLength);
                segments[i].pos += vel;
            }

            // constraints
            segments[0].pos = posLeft;
            segments[^1].pos = posRight;
            for (int i = 0; i < segments.Length - 1; ++i)
            {
                float dist = (segments[i].pos - segments[i + 1].pos).magnitude;
                float error = Mathf.Abs(dist - segLength);

                var tensionDir = Vector3.zero;
                if (dist > segLength)
                    tensionDir = (segments[i].pos - segments[i + 1].pos).normalized;
                else if(dist < segLength)
                    tensionDir = (segments[i + 1].pos - segments[i].pos).normalized;

                var tensionVel = error * tension * tensionDir;
                if (i == 0)
                    segments[i + 1].pos += tensionVel;
                else if (i == segments.Length - 1)
                    segments[i].pos -= tensionVel;
                else
                {
                    tensionVel *= 0.8f;
                    segments[i].pos -= tensionVel;
                    segments[i + 1].pos += tensionVel;
                }
            }
        }

        /// <summary>
        /// 로프 구간의 각 위치를 배열로 반환
        /// </summary>
        public Vector3[] GetSegmentsPos()
            => Array.ConvertAll(segments, x => x.pos);

        private struct Segment
        {
            public Segment(Vector3 pos)
            {
                this.pos = pos;
                lastPos = pos;
            }

            public Vector3 pos, lastPos;
        }
    }
}