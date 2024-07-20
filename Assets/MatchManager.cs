using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public void RecordMatchResult(int matchId, int winningTeamIndex)
    {
        if (TeamScoreTracking.Instance.IsMatchPlayed(matchId))
        {
            Debug.Log("Match already played.");
            return;
        }

        TeamScoreTracking.Instance.MarkMatchAsPlayed(matchId, true);

        // Update scores for the winning team
        int losingTeamIndex = TeamScoreTracking.Instance.GetMatchTeams(matchId).Find(team => team != winningTeamIndex);
        TeamScoreTracking.Instance.UpdateTeamScore(winningTeamIndex, TeamScoreTracking.Instance.GetTeamScore(winningTeamIndex) + 3); // Example: 3 points for a win
        TeamScoreTracking.Instance.UpdateTeamScore(losingTeamIndex, TeamScoreTracking.Instance.GetTeamScore(losingTeamIndex)); // No points for a loss
    }

    public void DisplayMatchResults()
    {
        foreach (var match in TeamScoreTracking.Instance.matchPairs)
        {
            if (TeamScoreTracking.Instance.IsMatchPlayed(match.Key))
            {
                var teams = match.Value;
                Debug.Log($"Match {match.Key} - Team {teams[0]} vs Team {teams[1]}");
                Debug.Log($"Score: Team {teams[0]} - {TeamScoreTracking.Instance.GetTeamScore(teams[0])}, Team {teams[1]} - {TeamScoreTracking.Instance.GetTeamScore(teams[1])}");
            }
        }
    }
}
