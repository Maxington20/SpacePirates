using UnityEngine;

public class FireArcIndicatorController : MonoBehaviour
{
    [SerializeField] private GameObject indicatorRoot;
    [SerializeField] private KeyCode showKey = KeyCode.Space;

    private void Update()
    {
        if (indicatorRoot == null)
        {
            return;
        }

        bool shouldShow = !PlayerState.IsDocked && Input.GetKey(showKey);
        indicatorRoot.SetActive(shouldShow);
    }
}