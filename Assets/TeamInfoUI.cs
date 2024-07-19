using UnityEngine;
using TMPro;

public class TeamInfoUI : MonoBehaviour
{
    public TMP_Text teamNameText;
    public TMP_Text pointsText;
    public TMP_Text goalDifferenceText;
    public TMP_Text goalsScoredText;

    public void SetupTeamInfo(Team team)
    {
        teamNameText.text = team.Name;
        pointsText.text = "Points: " + team.Points;
        goalDifferenceText.text = "GD: " + team.GoalDifference;
        goalsScoredText.text = "Goals: " + team.GoalsScored;
    }
}
