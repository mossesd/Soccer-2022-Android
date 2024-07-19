using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanelController : MonoBehaviour
{
    public GameObject ChooseTeamPanel;
    public GameObject ManageTeamsPanel;

    // Use this for initialization
    void Start()
    {
        // Display the Choose Team panel at the start of the game
        ChooseTeamPanel.SetActive(true);
        ManageTeamsPanel.SetActive(false);
    }

    // Method to be called when a team is chosen and the save button is clicked
    public void OnSaveButtonClicked()
    {
        // Display the Manage Teams panel
        ManageTeamsPanel.SetActive(true);

        // Hide the Choose Team panel
        ChooseTeamPanel.SetActive(false);
    }

    // Method to be called when the back button is clicked in Manage Teams panel
    public void OnBack1ButtonClicked()
    {
        // Display the Choose Team panel
        ChooseTeamPanel.SetActive(true);

        // Hide the Manage Teams panel
        ManageTeamsPanel.SetActive(false);
    }

    // Method to be called when the back button is clicked in Choose Team panel
   

    // Method to be called when the back button is clicked in the main menu panel
    public void OnBackButtonClicked()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
