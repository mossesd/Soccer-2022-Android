using UnityEngine;
using UnityEngine.UI;

public class GroupsSceneManager : MonoBehaviour
{
    public TeamNameController teamNameController; // Assign your existing TeamNameController object
    public TeamSelectionController teamSelectionController; // Assign your existing TeamSelectionController object
    public Image selectedTeamFlag; // Assign the Image UI element for displaying the selected team's flag
    public Text selectedTeamName; // Assign the Text UI element for displaying the selected team's name

    void Start()
    {
        LoadSelectedTeam();
    }

    void LoadSelectedTeam()
    {
        if (PlayerPrefs.HasKey("SelectedTeamIndex"))
        {
            int selectedIndex = PlayerPrefs.GetInt("SelectedTeamIndex");

            // Get the selected team's name and flag
            string teamName = teamNameController.TeamNames[selectedIndex];
            Texture2D flagTexture = (Texture2D)teamSelectionController.teams[selectedIndex];

            // Update the UI elements with the selected team information
            selectedTeamFlag.sprite = ConvertTextureToSprite(flagTexture);
            selectedTeamName.text = teamName;

            Debug.Log("Selected team in GroupsScene: " + teamName);
        }
    }

    Sprite ConvertTextureToSprite(Texture2D texture)
    {
        if (texture == null) return null;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, pivot);
    }
}
