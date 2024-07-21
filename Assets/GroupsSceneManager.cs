using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class GroupsSceneManager : MonoBehaviour
{
    [Header("List of Classes for getting TEAM FLAG and TEAM NAME")]
    public TeamNameController teamNameController; // Assign your existing TeamNameController object
    public TeamSelectionController teamSelectionController; // Assign your existing TeamSelectionController object

    [Header("List of GAMEOBJECTS for NATIONAL TEAM GROUP")]
    public GameObject groupPrefab; // Prefab for the group GameObject
    public Transform groupsParent; // Parent GameObject to hold the groups
    public GameObject teamPrefab; // Prefab for the team GameObject with GUITexture component
    public GameObject scorePanel; // Prefab for the team GameObject with GUITexture component
    public ButtonAction ButtonAction;

    public Image [] teamFlag;
    public TMP_Text [] teamName;
    public TMP_Text groupName;

    [Header("List of GAMEOBJECTS for CLUB TEAM GROUP")]
    public GameObject matchPrefab;
    public Transform matchContainer;
    public TMP_Text roundText;
    public Button nextButton;
    public Button prevButton;

    public GameObject leagueTableRowPrefab;
    public Transform leagueTableContainer;
    public LeagueManager leagueManager;
    public LeagueStats leagueStats;

    [Header("List of GAMEOBJECTS for CLUB TEAM GROUP")]
    public GameObject nationalTeamPanel;
    public GameObject clubTeamPanel;

    public List<int> teamIndices = new List<int>();
    private Dictionary<int, string> shuffledToTeamNameMap = new Dictionary<int, string>();
    private DataManager dataManager;

    int SelectedTeamIndex;

    private int group = 0;
    private int currentRound = 1;
    private int totalRounds;
    string tornament; 
    void Awake()
    {
        // Load the selected team information when the script awakens
        //SelectedTeamIndex = PlayerPrefs.GetInt("SelectedTeamIndex");
        dataManager = FindObjectOfType<DataManager>();
    }

    void Start()
    {
        tornament = PlayerPrefs.GetString("tournament");
        Debug.Log(tornament);
        switch (tornament)
        {
            case "National":
                NationalTeamPanel();
                break;
            case "CreateYourOwnTeam":
                ClubPanel();
                break;
        }
    }

    void OnMouseUpAsButton()
    {
        groupsParent.gameObject.SetActive(false);
        scorePanel.SetActive(true);
        ButtonAction.buttonType = ButtonAction.Buttons.Next;

        switch (tornament)
        {
            case "National":
                TeamScoreTracking.Instance.SaveData();
                break;
            case "CreateYourOwnTeam":
                dataManager.SaveData();
                break;
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
        List<int> availableIndices = Enumerable.Range(0, 32).ToList(); // List of all team indices

        for (int i = 0; i < 8; i++)
        {
            GameObject group = Instantiate(groupPrefab, groupsParent);
            Transform groupGridLayout = group.transform.GetChild(0);

            TeamScoreTracking.Instance.groupAssignments[i] = new List<int>();
            group.GetComponent<Text>().text = $"Group {i + 1}";

            for (int j = 0; j < 4; j++)
            {
                int randomIndex = Random.Range(0, availableIndices.Count); // Pick a random index
                int teamIndex = availableIndices[randomIndex]; // Get the team index
                availableIndices.RemoveAt(randomIndex); // Remove the index to avoid reassigning

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
                int team1Index = matchTeams[0];
                int team2Index = matchTeams[1];               

                Debug.Log($"Original Indices: Team1: {team1Index}, Team2: {team2Index}");

                int score1 = TeamScoreTracking.Instance.GetTeamScore(team1Index);
                int score2 = TeamScoreTracking.Instance.GetTeamScore(team2Index);

                // Get team names
                string team1Name = teamNameController.TeamNames[team1Index];
                string team2Name = teamNameController.TeamNames[team2Index];

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
            Debug.Log(teamNameController.TeamNames[groupMembers[i]]);
            Texture2D flagTexture = (Texture2D)teamSelectionController.teams[groupMembers[i]];
            teamFlag[i].sprite = ConvertTextureToSprite(flagTexture);
            teamName[i].text = teamNameController.TeamNames[groupMembers[i]];          
        }

        groupName.text = $"GROUP {group + 1}";

        if(group < 7)
            group++;
    }

    void NationalTeamPanel()
    {
        nationalTeamPanel.SetActive(true);
        clubTeamPanel.SetActive(false);

        SelectedTeamIndex = PlayerPrefs.GetInt("SelectedTeamIndex");
        if (PlayerPrefs.HasKey("NationalTeamScoreTrackingData"))
        {
            groupsParent.gameObject.SetActive(false);
            ButtonAction.buttonType = ButtonAction.Buttons.Next;
            AssignTeamScoreValues();
            scorePanel.SetActive(true);
        }
        else
        {
            scorePanel.SetActive(false);
            LoadTeams();
            AssignTeamsToGroups();
            AssignTeamScoreValues();
            ButtonAction.buttonType = ButtonAction.Buttons.None;
        }
    }

    void ClubPanel()
    {
        nationalTeamPanel.SetActive(false);
        clubTeamPanel.SetActive(true);

        SelectedTeamIndex = PlayerPrefs.GetInt("CreateYourOwnTeamSelectedTeamIndex");
        //SelectedTeamIndex = PlayerPrefs.GetInt("SelectedTeamIndex");
        Debug.Log(teamNameController.TeamNames[SelectedTeamIndex]);
        UpdateRoundUI();
        Invoke("DisplayFixtures", 1); 
        Invoke("DisplayLeagueTable", 1);
        ButtonAction.buttonType = ButtonAction.Buttons.Next;
    }

    public List<int> GetRandomTeams(int numberOfTeams)
    {
        List<int> allIndices = new List<int>();
        for (int i = 0; i < teamNameController.TeamNames.Length; i++)
        {
            allIndices.Add(i);
        }

        List<int> selectedIndices = new List<int>();
        System.Random rng = new System.Random();
        int selectedCount = 0;

        // Ensure the selected team is included
        selectedIndices.Add(SelectedTeamIndex);
        allIndices.Remove(SelectedTeamIndex);

        while (selectedCount < numberOfTeams - 1)
        {
            int index = rng.Next(allIndices.Count);
            selectedIndices.Add(allIndices[index]);
            allIndices.RemoveAt(index);
            selectedCount++;
        }

        return selectedIndices;
    }

    public void NextRound()
    {
        if (currentRound < totalRounds)
        {
            currentRound++;
            UpdateRoundUI();
            DisplayFixtures();
        }
    }

    public void PrevRound()
    {
        if (currentRound > 1)
        {
            currentRound--;
            UpdateRoundUI();
            DisplayFixtures();
        }
    }

    private void UpdateRoundUI()
    {
        roundText.text = $"Round {currentRound}";
        prevButton.interactable = currentRound > 1;
    }

    private void DisplayFixtures()
    {
        foreach (Transform child in matchContainer)
        {
            Destroy(child.gameObject); // Clear existing matches
        }

        List<LeagueManager.Match> fixtures = leagueManager.GetFixturesByRound(currentRound);

        foreach (var match in fixtures)
        {
            GameObject matchObj = Instantiate(matchPrefab, matchContainer);
            SetupMatchUI(matchObj, match);
        }

        totalRounds = leagueManager.GetTotalRounds();
    }

    private void SetupMatchUI(GameObject matchObj, LeagueManager.Match match)
    {

        // Set Team1 details
        Transform team1Image = matchObj.transform.Find("Team1Image");
        Transform team1Text = matchObj.transform.Find("Team1Text");
        team1Text.GetComponent<TMP_Text>().text = teamNameController.TeamNames[match.TeamA];

        Texture2D flagTexture = (Texture2D)teamSelectionController.teams[match.TeamA];
        team1Image.GetComponent<Image>().sprite = ConvertTextureToSprite(flagTexture);

        // Set Team2 details
        Transform team2Image = matchObj.transform.Find("Team2Image");
        Transform team2Text = matchObj.transform.Find("Team2Text");
        team2Text.GetComponent<TMP_Text>().text = teamNameController.TeamNames[match.TeamB];

        Texture2D flagTexture2 = (Texture2D)teamSelectionController.teams[match.TeamB];
        team2Image.GetComponent<Image>().sprite = ConvertTextureToSprite(flagTexture2);
    }

    private void DisplayLeagueTable()
    {
        foreach (Transform child in leagueTableContainer)
        {
            Destroy(child.gameObject); // Clear existing table rows
        }

        List<KeyValuePair<int, LeagueStats.TeamStats>> sortedTeams = leagueStats.GetSortedTeams();
        Debug.Log(sortedTeams.Count);
        for (int i = 0; i < sortedTeams.Count; i++)
        {
            int teamIndex = sortedTeams[i].Key;
            LeagueStats.TeamStats stats = sortedTeams[i].Value;

            GameObject rowObj = Instantiate(leagueTableRowPrefab, leagueTableContainer);
            SetupLeagueTableRowUI(rowObj, i + 1, teamIndex, stats);
        }
    }

    private void SetupLeagueTableRowUI(GameObject rowObj, int position, int teamIndex, LeagueStats.TeamStats stats)
    {
        rowObj.transform.Find("No").GetComponent<TMP_Text>().text = position.ToString();
        rowObj.transform.Find("TeamName").GetComponent<TMP_Text>().text = teamNameController.TeamNames[teamIndex];
        rowObj.transform.Find("MatchesPlayed").GetComponent<TMP_Text>().text = stats.Played.ToString();
        rowObj.transform.Find("Won").GetComponent<TMP_Text>().text = stats.Won.ToString();
        rowObj.transform.Find("Loose").GetComponent<TMP_Text>().text = stats.Lost.ToString();
        rowObj.transform.Find("Draw").GetComponent<TMP_Text>().text = stats.Drawn.ToString();
        rowObj.transform.Find("Points").GetComponent<TMP_Text>().text = stats.Points.ToString();
        rowObj.transform.Find("Goal").GetComponent<TMP_Text>().text = stats.TotalGoals.ToString();

        Texture2D flagTexture = (Texture2D)teamSelectionController.teams[teamIndex];
        rowObj.transform.Find("TeamImage").GetComponent<Image>().sprite = ConvertTextureToSprite(flagTexture);
    }
}
