using UnityEngine;

public class StarfieldGenerator : MonoBehaviour
{
    [Header("Starfield")]
    [SerializeField] private int starCount = 300;
    [SerializeField] private float fieldRadius = 100f;
    [SerializeField] private Vector2 starSizeRange = new Vector2(0.03f, 0.08f);

    private void Awake()
    {
        GenerateStars();
    }

    private void GenerateStars()
    {
        for (int i = 0; i < starCount; i++)
        {
            GameObject star = GameObject.CreatePrimitive(PrimitiveType.Quad);
            star.name = "Star";
            star.transform.SetParent(transform);

            float x = Random.Range(-fieldRadius, fieldRadius);
            float y = Random.Range(-fieldRadius, fieldRadius);
            float size = Random.Range(starSizeRange.x, starSizeRange.y);

            star.transform.position = new Vector3(x, y, 0f);
            star.transform.localScale = new Vector3(size, size, 1f);

            Collider collider = star.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            MeshRenderer meshRenderer = star.GetComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            float brightness = Random.Range(0.6f, 1f);

            Color starColor = new Color(
                brightness,
                brightness,
                Random.Range(0.8f, 1f));

            meshRenderer.material.color = starColor;
                        meshRenderer.sortingOrder = -100;
                    }

        Debug.Log($"Generated {starCount} stars.");
    }
}