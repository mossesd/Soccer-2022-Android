using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // Array to hold all the panels
    [SerializeField] private GameObject settingsPanel; // The settings panel

    string tournament = "tournament";

    string tournament1 = "CreateYourOwnTeam";
    string tournament2 = "National";
    string tournament3 = "Club";
    string tournament4 = "Frendly";

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
    public void LoadNationalTeamScene()
    {
        if (PlayerPrefs.HasKey("NationalTeamScoreTrackingData"))
            SceneManager.LoadScene("GroupsScene");
        else
            SceneManager.LoadScene("CreateTeam");

        PlayerPrefs.SetString(tournament, tournament2);
    }

    public void LoadCreateTeamScene()
    {
        if (PlayerPrefs.HasKey("ClubTeamScoreTrackingData"))
            SceneManager.LoadScene("GroupsScene");
        else
            SceneManager.LoadScene("CreateTeam");

        PlayerPrefs.SetString(tournament, tournament1);
    }

    // Add more methods to load other scenes as needed
    public void LoadAnotherScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
