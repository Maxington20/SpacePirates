using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalaxySystemSelector : MonoBehaviour
{
    [SerializeField] private List<SystemNode> systems = new List<SystemNode>();
    [SerializeField] private GameObject selectionIndicatorPrefab;
    [SerializeField] private string systemSceneName = "SystemScene";
    [SerializeField] private Vector3 indicatorOffset = Vector3.zero;

    private int selectedIndex;
    private GameObject activeIndicator;

    private void Start()
    {
        if (systems.Count == 0)
        {
            systems.AddRange(FindObjectsByType<SystemNode>(FindObjectsSortMode.None));
        }

        systems.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        SelectSystem(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPrevious();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNext();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            TravelToSelectedSystem();
        }
    }

    private void SelectPrevious()
    {
        if (systems.Count == 0)
        {
            return;
        }

        selectedIndex--;

        if (selectedIndex < 0)
        {
            selectedIndex = systems.Count - 1;
        }

        SelectSystem(selectedIndex);
    }

    private void SelectNext()
    {
        if (systems.Count == 0)
        {
            return;
        }

        selectedIndex++;

        if (selectedIndex >= systems.Count)
        {
            selectedIndex = 0;
        }

        SelectSystem(selectedIndex);
    }

    private void SelectSystem(int index)
    {
        if (systems.Count == 0 || index < 0 || index >= systems.Count)
        {
            return;
        }

        selectedIndex = index;

        if (activeIndicator == null && selectionIndicatorPrefab != null)
        {
            activeIndicator = Instantiate(selectionIndicatorPrefab);
        }

        if (activeIndicator != null)
        {
            activeIndicator.transform.position = systems[selectedIndex].transform.position + indicatorOffset;
        }

        Debug.Log($"Selected system: {systems[selectedIndex].SystemDefinition.SystemName}");
    }

    private void TravelToSelectedSystem()
    {
        if (systems.Count == 0)
        {
            return;
        }

        SystemNode selectedSystem = systems[selectedIndex];

        if (selectedSystem.SystemDefinition != null)
        {
            GameState.SetCurrentSystem(selectedSystem.SystemDefinition);
            Debug.Log($"Travelling to {selectedSystem.SystemDefinition.SystemName}");
        }

        SceneManager.LoadScene(systemSceneName);
    }
}