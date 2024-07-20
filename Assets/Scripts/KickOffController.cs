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
        List<int> matchIds = TeamScoreTracking.Instance.GetMatchesForSelectedTeam();

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

                Team1Flag.texture = (Texture2D)teamSelectionController.teams[shuffledTeam1Index];
                Team2Flag.texture = (Texture2D)teamSelectionController.teams[shuffledTeam2Index];

                Team1Name.text = teamNameController.TeamNames[shuffledTeam1Index];
                Team2Name.text = teamNameController.TeamNames[shuffledTeam2Index];

                Team1Material.mainTexture = teamSelectionController.textures[shuffledTeam1Index];
                Team2Material.mainTexture = teamSelectionController.textures[shuffledTeam2Index];
                Team1HDMaterial.mainTexture = teamSelectionController.HDTextures[shuffledTeam1Index];
                Team2HDMaterial.mainTexture = teamSelectionController.HDTextures[shuffledTeam2Index];

                //details += $"Match {matchId}: {team1Name} (Score: {score1}) vs {team2Name} (Score: {score2})\n";
            }
            else
            {
                //details += $"Match {matchId} does not have exactly two teams.\n";
            }
        }

        

		
        
		//Team1HDMaterial.mainTexture =  GameManager.SharedObject ().opponentTeamHDTexture;


	}
}
