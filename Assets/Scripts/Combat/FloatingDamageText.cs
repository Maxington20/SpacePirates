using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.7f;
    [SerializeField] private float moveSpeed = 1.5f;

    private TextMesh textMesh;
    private Color startColor;
    private float elapsedTime;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        startColor = textMesh.color;
    }

    public void Initialize(int damageAmount)
    {
        textMesh.text = damageAmount.ToString();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / lifetime);
        Color color = startColor;
        color.a = alpha;
        textMesh.color = color;

        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}