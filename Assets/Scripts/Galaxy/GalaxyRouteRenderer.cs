using UnityEngine;

public class GalaxyRouteRenderer : MonoBehaviour
{
    [SerializeField] private Transform fromSystem;
    [SerializeField] private Transform toSystem;
    [SerializeField] private float lineWidth = 0.04f;
    [SerializeField] private Color routeColor = new Color(0.2f, 0.8f, 1f, 0.65f);

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        ConfigureLineRenderer();
    }

    private void Start()
    {
        DrawRoute();
    }

    private void ConfigureLineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
        lineRenderer.sortingOrder = -10;

        Material material = new Material(Shader.Find("Sprites/Default"));
        material.color = routeColor;
        lineRenderer.material = material;

        lineRenderer.startColor = routeColor;
        lineRenderer.endColor = routeColor;
    }

    private void DrawRoute()
    {
        if (fromSystem == null || toSystem == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing route endpoints.");
            return;
        }

        lineRenderer.SetPosition(0, fromSystem.position);
        lineRenderer.SetPosition(1, toSystem.position);
    }
}