using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerformanceOverlay : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI label;

    [Header("Camera")]
    public Camera targetCamera; // If null, will try Camera.main

    [Header("Update")]
    [Tooltip("How often to refresh the numbers (seconds).")]
    public float updateInterval = 0.25f;

    [Tooltip("EMA smoothing for FPS (0 = no smoothing, 1 = infinite). 0.2–0.4 is good.")]
    [Range(0f, 0.95f)] public float fpsSmoothing = 0.3f;

    [Header("Misc")]
    [Tooltip("Toggle overlay with this key (Editor/desktop only).")]
    public KeyCode toggleKey = KeyCode.F1;

    private float _timeLeft;
    private float _unscaledFrameCount;
    private float _accumDelta;        // accumulative unscaled deltaTime across the interval
    private float _emaFps = -1f;
    private bool _visible = true;

    // cached to avoid GC alloc
    private readonly List<MeshFilter> _meshFilters = new List<MeshFilter>(1024);
    private readonly List<SkinnedMeshRenderer> _skinned = new List<SkinnedMeshRenderer>(256);
    private Plane[] _frustumPlanes;

    void Awake()
    {
        if (!label)
        {
            Debug.LogWarning("PerformanceOverlay: assign a TextMeshProUGUI label.");
        }
        if (!targetCamera) targetCamera = Camera.main;
        _timeLeft = updateInterval;
        _frustumPlanes = new Plane[6];

        // Prime caches once
        _meshFilters.AddRange(FindObjectsOfType<MeshFilter>(includeInactive: false));
        _skinned.AddRange(FindObjectsOfType<SkinnedMeshRenderer>(includeInactive: false));
    }

    void OnEnable()
    {
        _visible = true;
        if (label) label.gameObject.SetActive(true);
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(toggleKey))
        {
            _visible = !_visible;
            if (label) label.gameObject.SetActive(_visible);
        }
#endif
        if (!_visible || label == null || (targetCamera == null && (targetCamera = Camera.main) == null))
            return;

        // FPS accumulation (unscaled so it ignores timeScale quirks)
        _accumDelta += Time.unscaledDeltaTime;
        _unscaledFrameCount += 1f;
        _timeLeft -= Time.unscaledDeltaTime;

        if (_timeLeft <= 0f)
        {
            // Raw FPS over interval
            float intervalFps = _unscaledFrameCount / _accumDelta;
            if (_emaFps < 0f) _emaFps = intervalFps; // first sample
            _emaFps = Mathf.Lerp(intervalFps, _emaFps, fpsSmoothing);

            // Polygon (triangle) + vertex estimate
            int polyCount, vertCount;
            EstimateVisibleGeometry(targetCamera, out polyCount, out vertCount);

            // Display
            // Note: in Unity, polygons == triangles (meshes are fully triangulated)
            label.text = $"{_emaFps,0:0.0} FPS  |  ~{polyCount:n0} polys  |  ~{vertCount:n0} verts";

            // reset interval
            _timeLeft = updateInterval;
            _accumDelta = 0f;
            _unscaledFrameCount = 0f;
        }
    }

    /// <summary>
    /// Estimates visible polygons (triangles) and vertices in the camera frustum.
    /// </summary>
    void EstimateVisibleGeometry(Camera cam, out int polyCount, out int vertCount)
    {
        polyCount = 0;
        vertCount = 0;

        if (cam == null) return;

        GeometryUtility.CalculateFrustumPlanes(cam, _frustumPlanes);

        // MeshFilters (static/dynamic meshes)
        for (int i = 0; i < _meshFilters.Count; i++)
        {
            var mf = _meshFilters[i];
            if (mf == null) continue;

            var rend = mf.GetComponent<Renderer>();
            if (rend == null || !rend.enabled) continue;

            // quick reject: Unity's visibility + frustum test
            if (!rend.isVisible) continue;
            if (!GeometryUtility.TestPlanesAABB(_frustumPlanes, rend.bounds)) continue;

            var mesh = mf.sharedMesh;
            if (mesh == null) continue;

            // triangles.Length is 3 * triangles
            polyCount += mesh.triangles.Length / 3;
            vertCount += mesh.vertexCount;
        }

        // Skinned meshes
        for (int i = 0; i < _skinned.Count; i++)
        {
            var smr = _skinned[i];
            if (smr == null || !smr.enabled) continue;

            if (!smr.isVisible) continue;
            if (!GeometryUtility.TestPlanesAABB(_frustumPlanes, smr.bounds)) continue;

            var mesh = smr.sharedMesh;
            if (mesh == null) continue;

            polyCount += mesh.triangles.Length / 3;
            vertCount += mesh.vertexCount;
        }
    }

    // Optional: keep caches fresh when scenes load/unload additively
    void OnTransformChildrenChanged()
    {
        RefreshCaches();
    }

    public void RefreshCaches()
    {
        _meshFilters.Clear();
        _skinned.Clear();
        _meshFilters.AddRange(FindObjectsOfType<MeshFilter>(includeInactive: false));
        _skinned.AddRange(FindObjectsOfType<SkinnedMeshRenderer>(includeInactive: false));
    }
}
