using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class GroupsSceneManager : MonoBehaviour
{
    public TeamNameController teamNameController; // Assign your existing TeamNameController object
    public TeamSelectionController teamSelectionController; // Assign your existing TeamSelectionController object
    public Image selectedTeamFlag; // Assign the Image UI element for displaying the selected team's flag
    public Text selectedTeamName; // Assign the Text UI element for displaying the selected team's name
    public GameObject groupPrefab; // Prefab for the group GameObject
    public Transform groupsParent; // Parent GameObject to hold the groups
    public GameObject teamPrefab; // Prefab for the team GameObject with GUITexture component
    public GameObject scorePanel; // Prefab for the team GameObject with GUITexture component
    public ButtonAction ButtonAction;

    public Image [] teamFlag;
    public TMP_Text [] teamName;
    public TMP_Text groupName;

    public List<int> teamIndices = new List<int>();
    private Dictionary<int, string> shuffledToTeamNameMap = new Dictionary<int, string>();

    

    int group = 0;
    void Awake()
    {
        // Load the selected team information when the script awakens
        //LoadSelectedTeam();
        
    }

    void Start()
    {
        LoadTeams();
        AssignTeamsToGroups();
        AssignTeamScoreValues();
    }

    void OnMouseUpAsButton()
    {
        groupsParent.gameObject.SetActive(false);
        scorePanel.SetActive(true);
        ButtonAction.buttonType = ButtonAction.Buttons.Next;
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

    private void LoadTeams()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedTeamIndex");

        Debug.Log($"Selected Team Index: {selectedIndex}");

        for (int i = 0; i < teamNameController.TeamNames.Length; i++)
        {          
            teamIndices.Add(i);            
        }
    }

    Sprite ConvertTextureToSprite(Texture2D texture)
    {
        if (texture == null) return null;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, pivot);
    }

    private void AssignTeamsToGroups()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject group = Instantiate(groupPrefab, groupsParent);
            Transform groupGridLayout = group.transform.GetChild(0);

            TeamScoreTracking.Instance.groupAssignments[i] = new List<int>();
            group.GetComponent<Text>().text = $"Group {i + 1}";

            for (int j = 0; j < 4; j++)
            {
                int teamIndex = teamIndices[i * 4 + j];
                string teamName = teamNameController.TeamNames[teamIndex];
                GameObject teamObject = Instantiate(teamPrefab, groupGridLayout.transform);
                AssignTeamData(teamObject, teamIndex);
                TeamScoreTracking.Instance.groupAssignments[i].Add(teamIndex);
                TeamScoreTracking.Instance.teamScores[teamIndex] = 0; // Initialize team score
            }
        }

        foreach (var group in TeamScoreTracking.Instance.groupAssignments)
        {
            Debug.Log($"Group {group.Key} has teams: {string.Join(", ", group.Value)}");
        }


        TeamScoreTracking.Instance.SetupMatches(); // Setup matches after teams are assigned
    }

    private void AssignTeamData(GameObject teamObject, int teamIndex)
    {
        Texture2D flagTexture = (Texture2D)teamSelectionController.teams[teamIndex];

        // Update the UI elements with the selected team information
        teamObject.GetComponent<Image>().sprite = ConvertTextureToSprite(flagTexture);
    }

    private void Shuffle<T>(List<T> list)
    {
        Debug.Log("Before Shuffle: " + string.Join(", ", list));
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        Debug.Log("After Shuffle: " + string.Join(", ", list));
    }


    public void DisplayMatchDetails()
    {
        string details = "";
        List<int> matchIds = TeamScoreTracking.Instance.GetMatchesForSelectedTeam();

        Debug.Log($"Match IDs: {string.Join(", ", matchIds)}");

        foreach (int matchId in matchIds)
        {
            List<int> matchTeams = TeamScoreTracking.Instance.GetMatchTeams(matchId);
            Debug.Log($"Match {matchId} Teams Count: {matchTeams.Count}");

            if (matchTeams.Count == 2)
            {
                int shuffledTeam1Index = matchTeams[0];
                int shuffledTeam2Index = matchTeams[1];

                Debug.Log($"Shuffled Indices: Team1: {shuffledTeam1Index}, Team2: {shuffledTeam2Index}");

                // Resolve original indices from shuffled indices
                int team1Index = TeamScoreTracking.Instance.originalToShuffledIndexMap.FirstOrDefault(x => x.Value == shuffledTeam1Index).Key;
                int team2Index = TeamScoreTracking.Instance.originalToShuffledIndexMap.FirstOrDefault(x => x.Value == shuffledTeam2Index).Key;

                Debug.Log($"Original Indices: Team1: {team1Index}, Team2: {team2Index}");

                int score1 = TeamScoreTracking.Instance.GetTeamScore(shuffledTeam1Index);
                int score2 = TeamScoreTracking.Instance.GetTeamScore(shuffledTeam2Index);

                // Get team names
                string team1Name = teamNameController.TeamNames[shuffledTeam1Index];
                string team2Name = teamNameController.TeamNames[shuffledTeam2Index];

                details += $"Match {matchId}: {team1Name} (Score: {score1}) vs {team2Name} (Score: {score2})\n";
            }
            else
            {
                details += $"Match {matchId} does not have exactly two teams.\n";
            }
        }

        Debug.Log($"Match Details: {details}");
        // Assuming you have a Text UI element to display match details:
        // matchDetailsText.text = details; // Un-comment and assign matchDetailsText in your script
    }

    public void AssignTeamScoreValues()
    {
        List<int> groupMembers = TeamScoreTracking.Instance.GetGroupMembers(group);

        for (int i = 0; i < groupMembers.Count; i++)
        {
            Texture2D flagTexture = (Texture2D)teamSelectionController.teams[groupMembers[i]];
            teamFlag[i].sprite = ConvertTextureToSprite(flagTexture);
            teamName[i].text = teamNameController.TeamNames[groupMembers[i]];          
        }

        groupName.text = $"GROUP {group + 1}";

        if(group < 7)
            group++;
    }
}
