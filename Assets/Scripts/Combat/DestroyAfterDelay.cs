using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.35f;
    [SerializeField] private float growthSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private float elapsedTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            startColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        transform.localScale += Vector3.one * growthSpeed * Time.deltaTime;

        if (spriteRenderer != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / lifetime);

            Color color = startColor;
            color.a = alpha;

            spriteRenderer.color = color;
        }

        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}