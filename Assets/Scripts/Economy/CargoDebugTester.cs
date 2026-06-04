using UnityEngine;

public class CargoDebugTester : MonoBehaviour
{
    [SerializeField] private CargoItemDefinition testItem;

    private PlayerCargoHold cargoHold;

    private void Awake()
    {
        cargoHold = GetComponent<PlayerCargoHold>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            bool success = cargoHold.AddCargo(testItem, 1);

            Debug.Log(
                $"Add Cargo: {success} | " +
                $"Current Cargo: {cargoHold.CurrentCargo}/{cargoHold.CargoCapacity}");
        }
    }
}