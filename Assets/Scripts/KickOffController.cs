using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class KickOffController : MonoBehaviour
{
	public GUITexture Team1Flag, Team2Flag;
	public GUIText Team1Name, Team2Name;
	public Material Team1Material, Team2Material,Team1HDMaterial, Team2HDMaterial;
    public TeamNameController teamNameController;
    public TeamSelectionController teamSelectionController;
    // Use this for initialization

    
    void Start()
	{
        string tornament = PlayerPrefs.GetString("tournament");
        switch (tornament)
        {
            case "National":
                NationalTeamPanel();
                break;
            case "CreateYourOwnTeam":
                ClubPanel();
                break;
        } 	        
		//Team1HDMaterial.mainTexture =  GameManager.SharedObject ().opponentTeamHDTexture;
	}

    void NationalTeamPanel()
    {
        List<int> matchIds = TeamScoreTracking.Instance.GetMatchesForSelectedTeam();

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

                Team1Flag.texture = (Texture2D)teamSelectionController.teams[team1Index];
                Team2Flag.texture = (Texture2D)teamSelectionController.teams[team2Index];

                Team1Name.text = teamNameController.TeamNames[team1Index];
                Team2Name.text = teamNameController.TeamNames[team2Index];

                Team1Material.mainTexture = teamSelectionController.textures[team1Index];
                Team2Material.mainTexture = teamSelectionController.textures[team2Index];
                Team1HDMaterial.mainTexture = teamSelectionController.HDTextures[team1Index];
                Team2HDMaterial.mainTexture = teamSelectionController.HDTextures[team2Index];

                //details += $"Match {matchId}: {team1Name} (Score: {score1}) vs {team2Name} (Score: {score2})\n";
            }
            else
            {
                //details += $"Match {matchId} does not have exactly two teams.\n";
            }
        }
    }

    void ClubPanel()
    {
        LeagueManager.Match match = LeagueManager.Instance.GetMatchForTeamInFirstRound(PlayerPrefs.GetInt("CreateYourOwnTeamSelectedTeamIndex"), 1);
        if (match != null) 
        {
            string team1Name = teamNameController.TeamNames[match.TeamA];
            string team2Name = teamNameController.TeamNames[match.TeamB];

            Team1Flag.texture = (Texture2D)teamSelectionController.teams[match.TeamA];
            Team2Flag.texture = (Texture2D)teamSelectionController.teams[match.TeamB];

            Team1Name.text = teamNameController.TeamNames[match.TeamA];
            Team2Name.text = teamNameController.TeamNames[match.TeamB];

            Team1Material.mainTexture = teamSelectionController.textures[match.TeamA];
            Team2Material.mainTexture = teamSelectionController.textures[match.TeamB];
            Team1HDMaterial.mainTexture = teamSelectionController.HDTextures[match.TeamA];
            Team2HDMaterial.mainTexture = teamSelectionController.HDTextures[match.TeamB];
        }
    }
}
