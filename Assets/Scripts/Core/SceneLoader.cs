using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string galaxyMapSceneName = "GalaxyMap";
    [SerializeField] private string systemSceneName = "SystemScene";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadGalaxyMap();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadSystemScene();
        }
    }

    public void LoadGalaxyMap()
    {
        SceneManager.LoadScene(galaxyMapSceneName);
    }

    public void LoadSystemScene()
    {
        SceneManager.LoadScene(systemSceneName);
    }
}