using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ShowCurrentMatches : MonoBehaviour
{
    public GameObject flag1, flag2, flag3, flag4; // UI Image components to display team flags
    public Text teamName1, teamName2, teamName3, teamName4; // UI Text components to display team names

    public TeamNameController teamNameController; // Assign your existing TeamNameController object
    public TeamSelectionController teamSelectionController; // Assign your existing TeamSelectionController object

    void Start()
    {
        ShowMatchesForSelectedTeam();
    }

    void ShowMatchesForSelectedTeam()
    {
        int selectedGroup = TeamScoreTracking.Instance.GetSelectedTeamGroup();

        if (selectedGroup == -1)
        {
            Debug.LogWarning("Selected team is not assigned to any group.");
            return;
        }

        // Get the teams in the selected group
        List<int> teamsInGroup = TeamScoreTracking.Instance.GetGroupMembers(selectedGroup);

        // Ensure there are exactly 4 teams in the group
        if (teamsInGroup.Count != 4)
        {
            Debug.LogWarning($"Group {selectedGroup} does not have exactly 4 teams.");
            return;
        }

        // Get the matches for the selected group
        List<int> matchIds = TeamScoreTracking.Instance.GetMatchesForGroup(selectedGroup);
        
        if (matchIds.Count < 1)
        {
            Debug.LogWarning($"No matches found for group {selectedGroup}.");
            return;
        }

        // Display match details (assuming 2 matches for simplicity)
        if (matchIds.Count > 0)
        {
            DisplayMatch(matchIds[0], flag1, teamName1, flag2, teamName2);
        }

        if (matchIds.Count > 1)
        {
            DisplayMatch(matchIds[1], flag3, teamName3, flag4, teamName4);
        }
    }

    void DisplayMatch(int matchId, GameObject flag1, Text teamName1, GameObject flag2, Text teamName2)
    {
        List<int> matchTeams = TeamScoreTracking.Instance.GetMatchTeams(matchId);

        if (matchTeams.Count == 2)
        {
            int team1Index = matchTeams[0];
            int team2Index = matchTeams[1];

            // Get team names and flags
            string team1Name = teamNameController.TeamNames[team1Index];
            string team2Name = teamNameController.TeamNames[team2Index];
            Texture2D flag1Texture = (Texture2D)teamSelectionController.teams[team1Index];
            Texture2D flag2Texture = (Texture2D)teamSelectionController.teams[team2Index];

            flag1.GetComponent<GUITexture>().texture = flag1Texture;
            flag2.GetComponent<GUITexture>().texture = flag2Texture;
            // Assign the team names and flags to the UI elements
            /*teamName1.text = team1Name;
            flag1.sprite = ConvertTextureToSprite(flag1Texture);

            teamName2.text = team2Name;
            flag2.sprite = ConvertTextureToSprite(flag2Texture);*/
        }
        else
        {
            Debug.LogWarning($"Match {matchId} does not have exactly two teams.");
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
