using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // Array to hold all the panels
    [SerializeField] private GameObject settingsPanel; // The settings panel
    
    // Method to show the settings panel and hide all other panels
    public void ShowSettingsPanel()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        settingsPanel.SetActive(true);
    }

    // Method to hide the settings panel and show all other panels
    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
        foreach (GameObject panel in panels)
        {
            panel.SetActive(true);
        }
    }

    // Method to load the CreateTeam scene
    public void LoadCreateTeamScene()
    {
        SceneManager.LoadScene("CreateTeam");
    }

    // Add more methods to load other scenes as needed
    public void LoadAnotherScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
