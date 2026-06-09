using UnityEngine;

public class FlareController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode flareKey = KeyCode.Q;

    private ShipLoadout shipLoadout;
    private float nextFlareTime;

    private void Awake()
    {
        shipLoadout = GetComponent<ShipLoadout>();
    }

    private void Update()
    {
        if (PlayerState.IsDocked)
        {
            return;
        }

        if (!Input.GetKeyDown(flareKey))
        {
            return;
        }

        TryDeployFlare();
    }

    private void TryDeployFlare()
    {
        FlareDefinition flare = shipLoadout != null
            ? shipLoadout.FlareDefinition
            : null;

        if (flare == null)
        {
            GameMessageUI.Instance?.ShowMessage("No flares equipped.");
            return;
        }

        if (Time.time < nextFlareTime)
        {
            GameMessageUI.Instance?.ShowMessage("Flares recharging.");
            return;
        }

        DeployFlare(flare);
        nextFlareTime = Time.time + flare.Cooldown;
    }

    private void DeployFlare(FlareDefinition flare)
    {
        HomingMissile[] missiles =
            FindObjectsByType<HomingMissile>(FindObjectsSortMode.None);

        int affectedCount = 0;

        foreach (HomingMissile missile in missiles)
        {
            if (missile == null)
            {
                continue;
            }

            if (missile.Owner == gameObject)
            {
                continue;
            }

            float distance = Vector2.Distance(
                transform.position,
                missile.transform.position);

            if (distance > flare.FlareRadius)
            {
                continue;
            }

            if (Random.value > flare.MissileSpoofChance)
            {
                continue;
            }

            missile.LoseLock();
            affectedCount++;
        }

        if (flare.FlareEffectPrefab != null)
        {
            Instantiate(flare.FlareEffectPrefab, transform.position, Quaternion.identity);
        }

        GameMessageUI.Instance?.ShowMessage(
            affectedCount > 0
                ? $"{flare.FlareName} deployed! {affectedCount} missile(s) spoofed."
                : $"{flare.FlareName} deployed.");
    }
}