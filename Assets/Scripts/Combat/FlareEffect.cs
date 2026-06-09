using UnityEngine;

public class FlareEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.8f;
    [SerializeField] private float startScale = 0.8f;
    [SerializeField] private float endScale = 4f;

    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private float elapsed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = Vector3.one * startScale;

        if (spriteRenderer != null)
        {
            startColor = spriteRenderer.color;
            startColor.a = 1f;
            spriteRenderer.color = startColor;
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        float t = elapsed / lifetime;

        transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, t);

        if (spriteRenderer != null)
        {
            Color color = startColor;
            color.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = color;
        }

        if (elapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}