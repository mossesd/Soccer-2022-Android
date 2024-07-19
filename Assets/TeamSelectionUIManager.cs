using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamSelectionUIManager : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your TeamButtonPrefab here
    public Transform contentPanel; // Assign the Content object of the Scroll View
    public TeamSelectionController teamSelectionController; // Assign your existing TeamSelectionController object
    public TeamNameController teamNameController; // Assign your existing TeamNameController object

    public Image selectedTeamFlag; // Assign the Image UI element for displaying the selected team's flag
    public TMP_Text selectedTeamName; // Assign the TMP_Text UI element for displaying the selected team's name
    public Button saveButton; // Assign the Save Button UI element
    public Button saveAndGoToGroupsButton; // Assign the new Save and Go to Groups Button UI element

    public TeamInfoDisplayManager teamInfoDisplayManager; // Assign the TeamInfoDisplayManager script here

    private string currentTeamName; // Store the currently selected team name

    void Start()
    {
        PopulateTeamButtons();
        saveButton.onClick.AddListener(SaveSelectedTeam);
        saveAndGoToGroupsButton.onClick.AddListener(SaveSelectedTeamAndGoToGroups);
        LoadSelectedTeam();
    }

    void PopulateTeamButtons()
    {
        for (int i = 0; i < teamSelectionController.teams.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentPanel);
            Image flagImage = newButton.transform.Find("Flag").GetComponent<Image>();
            flagImage.sprite = ConvertTextureToSprite((Texture2D)teamSelectionController.teams[i]);
            newButton.transform.Find("CountryName").GetComponent<Text>().text = teamNameController.TeamNames[i];

            // Add button click event to set the team
            int index = i; // Capture the current value of i
            newButton.GetComponent<Button>().onClick.AddListener(() => OnTeamButtonClick(index));
        }
    }

    Sprite ConvertTextureToSprite(Texture2D texture)
    {
        if (texture == null) return null;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, pivot);
    }

    void OnTeamButtonClick(int index)
    {
        TeamSelectionController.teamIndex = index;
        Debug.Log("Selected Team: " + teamNameController.TeamNames[index]);

        // Clear previous players and team information when a new country is selected
        teamInfoDisplayManager.ClearPreviousPlayers();

        // Update the selected team's flag and name UI elements
        selectedTeamFlag.sprite = ConvertTextureToSprite((Texture2D)teamSelectionController.teams[index]);
        selectedTeamName.text = teamNameController.TeamNames[index];

        // Display the selected team's information
        teamInfoDisplayManager.DisplayTeamInformation(teamNameController.TeamNames[index]);

        // Update the current team name
        currentTeamName = teamNameController.TeamNames[index];
    }

    void SaveSelectedTeam()
    {
        if (selectedTeamFlag.sprite != null)
        {
            string newTeamName = selectedTeamName.text;

            // Save the selected team name and flag texture path
            PlayerPrefs.SetString("SelectedTeamName", newTeamName);
            PlayerPrefs.SetInt("SelectedTeamIndex", TeamSelectionController.teamIndex);

            // Convert the sprite to a texture and then to a byte array for saving
            Texture2D texture = MakeTextureReadable(selectedTeamFlag.sprite.texture);
            byte[] textureBytes = texture.EncodeToPNG();
            string textureBase64 = System.Convert.ToBase64String(textureBytes);
            PlayerPrefs.SetString("SelectedTeamFlag", textureBase64);

            PlayerPrefs.Save();
            Debug.Log("Selected team saved: " + newTeamName);

            // Clear previous players and display the new team's information
            teamInfoDisplayManager.ClearPreviousPlayers();
            teamInfoDisplayManager.DisplayTeamInformation(newTeamName);

            // Update the current team name
            currentTeamName = newTeamName;
        }
    }

    void SaveSelectedTeamAndGoToGroups()
    {
        SaveSelectedTeam();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GroupsScene");
    }

    void LoadSelectedTeam()
    {
        if (PlayerPrefs.HasKey("SelectedTeamName") && PlayerPrefs.HasKey("SelectedTeamFlag"))
        {
            string teamName = PlayerPrefs.GetString("SelectedTeamName");
            string textureBase64 = PlayerPrefs.GetString("SelectedTeamFlag");

            // Decode the base64 string to a byte array and then to a texture
            byte[] textureBytes = System.Convert.FromBase64String(textureBase64);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(textureBytes);

            // Update the UI elements with the loaded values
            selectedTeamFlag.sprite = ConvertTextureToSprite(texture);
            selectedTeamName.text = teamName;

            Debug.Log("Loaded selected team: " + teamName);

            // Load and display team information from JSON
            teamInfoDisplayManager.DisplayTeamInformation(teamName);

            // Update the current team name
            currentTeamName = teamName;
        }
    }

    private Texture2D MakeTextureReadable(Texture texture)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
            texture.width,
            texture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(texture, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        Texture2D readableTexture = new Texture2D(texture.width, texture.height);
        readableTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        return readableTexture;
    }
}
