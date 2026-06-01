using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.35f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}