using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// Draws a thick, rounded quadratic Bezier "road" between two UI RectTransforms.
[ExecuteAlways]
public class UIBezierRoad : MaskableGraphic
{
    [Header("Endpoints (UI)")]
    public RectTransform from;          //LevelNode_1
    public RectTransform to;            // LevelNode_2
    public Canvas canvas;               // LevelSelect/canvas


    [Header("Shape")]
    [Range(8,256)] public int samples = 48;    // curve smoothness
    public float thickness = 16f;              // road width in pixels
    public float arcOffset = 100f;             // how much the curve bows
    public Vector2 arcDirection = Vector2.up;  // up/down/diagonal bow
    [Range(4,32)] public int capSegments = 12; // round endcaps smoothness

    readonly List<UIVertex> _verts = new List<UIVertex>(1024);
    readonly List<int> _tris = new List<int>(4096);

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        _verts.Clear();
        _tris.Clear();

        if (!from || !to || !canvas || samples < 2) return;

        var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        // Convert endpoints to SCREEN space (so layout doesn't matter)
        Vector2 aScreen = RectTransformUtility.WorldToScreenPoint(cam, from.TransformPoint(from.rect.center));
        Vector2 cScreen = RectTransformUtility.WorldToScreenPoint(cam, to  .TransformPoint(to  .rect.center));

        // Control point in SCREEN space
        Vector2 mid = (aScreen + cScreen) * 0.5f;
        Vector2 bScreen = mid + arcDirection.normalized * arcOffset;

        // Convert the sampled Bezier points into THIS RectTransform's local space
        RectTransform rt = rectTransform;
        Vector2 ToLocal(Vector2 screen)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screen, cam, out var local);
            return local;
        }

        // Sample centerline
        var centers = new Vector2[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = i / (samples - 1f);
            Vector2 pScreen = QuadBezier(aScreen, bScreen, cScreen, t);
            centers[i] = ToLocal(pScreen);
        }

        // Build thick strip with round endcaps
        float half = thickness * 0.5f;
       // int baseIndex = 0;

        // round cap at start
        AddRoundCap(centers[0], centers[1], half, true, capSegments);

        // body quads
        for (int i = 0; i < samples - 1; i++)
        {
            Vector2 p0 = centers[i];
            Vector2 p1 = centers[i + 1];
            Vector2 dir = (p1 - p0).normalized;
            Vector2 n = new Vector2(-dir.y, dir.x) * half;

            // quad verts
            int i0 = AddVert(p0 - n);
            int i1 = AddVert(p0 + n);
            int i2 = AddVert(p1 + n);
            int i3 = AddVert(p1 - n);

            AddQuad(i0, i1, i2, i3);
        }

        // round cap at end
        AddRoundCap(centers[samples - 1], centers[samples - 2], half, false, capSegments);

        // push to VertexHelper
        vh.AddUIVertexStream(_verts, _tris);

        // local helpers
        Vector2 QuadBezier(Vector2 A, Vector2 B, Vector2 C, float t)
        {
            float u = 1f - t;
            return u * u * A + 2f * u * t * B + t * t * C;
        }

        int AddVert(Vector2 pos)
        {
            var v = UIVertex.simpleVert;
            v.position = pos;
            v.color = color;
            _verts.Add(v);
            return _verts.Count - 1;
        }

        void AddQuad(int a, int b, int c, int d)
        {
            _tris.Add(a); _tris.Add(b); _tris.Add(c);
            _tris.Add(a); _tris.Add(c); _tris.Add(d);
        }

        void AddRoundCap(Vector2 tip, Vector2 next, float radius, bool start, int segments)
        {
            // direction of the edge
            Vector2 dir = (next - tip).normalized;
            if (start) dir = -dir; // flip for start cap
            // build a semicircle facing dir
            Vector2 n0 = new Vector2(-dir.y, dir.x);
            float angle0 = Mathf.Atan2(n0.y, n0.x); // left normal
            float delta = Mathf.PI / segments;

            int centerIdx = AddVert(tip);
            int prevIdx = AddVert(tip + new Vector2(Mathf.Cos(angle0), Mathf.Sin(angle0)) * radius);

            for (int i = 1; i <= segments; i++)
            {
                float a = angle0 + (start ? -1 : 1) * i * delta;
                int vi = AddVert(tip + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius);
                _tris.Add(centerIdx); _tris.Add(prevIdx); _tris.Add(vi);
                prevIdx = vi;
            }
        }
    }
}
