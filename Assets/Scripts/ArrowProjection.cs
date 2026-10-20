using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArrowProjection : MonoBehaviour
{
    [Header("Components")]
    private LineRenderer lineRenderer;

    [Header("Arrowhead Settings")]
    [SerializeField] private GameObject arrowheadPrefab;
    private GameObject instantiatedArrowhead;

    [Header("Simulation Parameters")]
    [Tooltip("The launch point of the arrow (e.g., the bowstring position)")]
    [SerializeField] private Transform launchPoint;

    [Tooltip("How far into the future the path should calculate")]
    [SerializeField] int resolution = 30;

    [Tooltip("Time step between each simulated point (lower = smoother line)")]
    [SerializeField] float timeStep = 0.05f;

    [Header("Collision Layers")]
    [Tooltip("Select layers the arrow can collide with (e.g., Ground, Obstacles)")]
    [SerializeField] private LayerMask collisionLayers;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (arrowheadPrefab != null)
        {
            instantiatedArrowhead = Instantiate(arrowheadPrefab, transform);
        }
    }

    /// <summary>
    /// Call this from your Bow controller script every Frame (Update or FixedUpdate) while aiming.
    /// </summary>
    /// <param name="launchVelocity">The forward speed of the arrow based on bow drawback strength.</param>
    public void ShowTrajectory(float launchVelocity)
    {
        if (launchPoint == null) return;

        lineRenderer.enabled = true;
        if (instantiatedArrowhead != null) instantiatedArrowhead.SetActive(true);

        Vector3 startPosition = launchPoint.position;
        Vector3 startVelocity = launchPoint.forward * launchVelocity;

        // Use Unity's native gravity vector
        Vector3 gravity = Physics.gravity;

        // We will build a dynamic list of points until we hit something
        var points = new System.Collections.Generic.List<Vector3>();
        points.Add(startPosition);

        Vector3 currentPosition = startPosition;
        Vector3 currentVelocity = startVelocity;

        for (int i = 1; i < resolution; i++)
        {
            // Standard kinematic equation: Physics step simulation
            currentVelocity += gravity * timeStep;
            Vector3 nextPosition = currentPosition + currentVelocity * timeStep;

            // Check if the path segment hits an object
            RaycastHit hit;
            Vector3 segmentDirection = nextPosition - currentPosition;

            if (Physics.Raycast(currentPosition, segmentDirection.normalized, out hit, segmentDirection.magnitude, collisionLayers))
            {
                // Stop the path right at the collision surface
                points.Add(hit.point);

                // Position and orient the arrowhead to flat against the surface or facing it
                PositionArrowhead(hit.point, currentPosition);
                break;
            }

            points.Add(nextPosition);
            currentPosition = nextPosition;

            // If we reach the end of resolution without hitting anything
            if (i == resolution - 1)
            {
                PositionArrowhead(currentPosition, points[points.Count - 2]);
            }
        }

        // Render the calculated positions
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void PositionArrowhead(Vector3 position, Vector3 previousPosition)
    {
        if (instantiatedArrowhead == null) return;

        instantiatedArrowhead.transform.position = position;
        Vector3 direction = (position - previousPosition).normalized;
        if (direction != Vector3.zero)
        {
            instantiatedArrowhead.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void HideTrajectory()
    {
        lineRenderer.enabled = false;
        if (instantiatedArrowhead != null) instantiatedArrowhead.SetActive(false);
    }
}
