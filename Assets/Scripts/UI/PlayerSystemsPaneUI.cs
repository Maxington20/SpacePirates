using UnityEngine;

public class PlayerSystemsPanelUI : MonoBehaviour
{
    [SerializeField] private ShipSystemDamage systemDamage;

    [SerializeField] private SystemStatusRowUI enginesRow;
    [SerializeField] private SystemStatusRowUI weaponsRow;
    [SerializeField] private SystemStatusRowUI shieldsRow;

    private void Start()
    {
        if (systemDamage == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                systemDamage = player.GetComponent<ShipSystemDamage>();
            }
        }

        if (systemDamage != null)
        {
            systemDamage.SystemsChanged += Refresh;
        }

        Refresh();
    }

    private void OnDestroy()
    {
        if (systemDamage != null)
        {
            systemDamage.SystemsChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        if (systemDamage == null)
        {
            enginesRow?.SetStatus("ENG", ShipSystemState.Operational);
            weaponsRow?.SetStatus("WPN", ShipSystemState.Operational);
            shieldsRow?.SetStatus("SHD", ShipSystemState.Operational);
            return;
        }

        enginesRow?.SetStatus("ENG", systemDamage.EnginesState);
        weaponsRow?.SetStatus("WPN", systemDamage.WeaponsState);
        shieldsRow?.SetStatus("SHD", systemDamage.ShieldsState);
    }
}