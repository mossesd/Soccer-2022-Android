using System.Collections.Generic;
using UnityEngine;

public class TeamScoreTracking : MonoBehaviour
{
    public Dictionary<int, List<int>> groupAssignments = new Dictionary<int, List<int>>(); // Group ID to list of team indices
    public Dictionary<int, int> teamScores = new Dictionary<int, int>(); // Team index to score
    public Dictionary<int, List<int>> matchPairs = new Dictionary<int, List<int>>(); // Match ID to list of team indices
    public Dictionary<int, bool> matchResults = new Dictionary<int, bool>(); // Match ID to whether the match is played
    public Dictionary<int, int> originalToShuffledIndexMap = new Dictionary<int, int>();
    public Dictionary<int, List<int>> groupMatches = new Dictionary<int, List<int>>(); // Group ID to list of match IDs
    public Dictionary<int, List<int>> matches = new Dictionary<int, List<int>>(); // Match ID to list of team indices

    public static TeamScoreTracking Instance { get; private set; }

    private int matchCounter = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTeamScore(int teamIndex, int score)
    {
        if (teamScores.ContainsKey(teamIndex))
        {
            teamScores[teamIndex] = score;
        }
    }

    public int GetTeamScore(int teamIndex)
    {
        if (teamScores.ContainsKey(teamIndex))
        {
            return teamScores[teamIndex];
        }
        return 0;
    }

    public int GetSelectedTeamGroup()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedTeamIndex");
        foreach (var group in groupAssignments)
        {
            if (group.Value.Contains(selectedIndex))
            {
                return group.Key;
            }
        }
        return -1; // If not found
    }

    public List<int> GetGroupMembers(int groupIndex)
    {
        if (groupAssignments.ContainsKey(groupIndex))
        {
            return groupAssignments[groupIndex];
        }
        return new List<int>();
    }

    public void SetupMatches()
    {
        // Reset matchCounter and match data to ensure no old data interferes
        matchCounter = 0;
        matches.Clear();
        groupMatches.Clear();
        matchPairs.Clear();

        foreach (var group in groupAssignments)
        {
            List<int> teams = group.Value;
            List<int> groupMatchIds = new List<int>();

            Debug.Log($"Setting up matches for Group {group.Key} with {teams.Count} teams.");

            // Ensure teams are paired correctly within each group
            for (int i = 0; i < teams.Count; i += 2)
            {
                if (i + 1 < teams.Count)
                {
                    int matchId = matchCounter++;
                    matches[matchId] = new List<int> { teams[i], teams[i + 1] };
                    groupMatchIds.Add(matchId);
                    matchPairs[matchId] = new List<int> { teams[i], teams[i + 1] };

                    Debug.Log($"Match {matchId} created for Group {group.Key}: Team {teams[i]} vs Team {teams[i + 1]}");
                }
                else
                {
                    Debug.Log($"Skipping team {teams[i]} in Group {group.Key} due to an odd number of teams.");
                }
            }

            groupMatches[group.Key] = groupMatchIds;
            Debug.Log($"Group {group.Key} matches: {string.Join(", ", groupMatchIds)}");
        }
    }

    public void MarkMatchAsPlayed(int matchId, bool result)
    {
        if (matchResults.ContainsKey(matchId))
        {
            matchResults[matchId] = result;
        }
        else
        {
            matchResults.Add(matchId, result);
        }
    }

    public bool IsMatchPlayed(int matchId)
    {
        return matchResults.ContainsKey(matchId) && matchResults[matchId];
    }

    public List<int> GetMatchTeams(int matchId)
    {
        if (matches.ContainsKey(matchId))
        {
            return matches[matchId];
        }
        return new List<int>();
    }

    public List<int> GetMatchesForSelectedTeam()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedTeamIndex");

        List<int> selectedTeamMatches = new List<int>();

        foreach (var match in matches)
        {
            if (match.Value.Contains(selectedIndex))
            {
                selectedTeamMatches.Add(match.Key);
            }
        }
        return selectedTeamMatches;
    }

    public List<int> GetMatchesForGroup(int groupIndex)
    {
        if (groupMatches.ContainsKey(groupIndex))
        {
            return groupMatches[groupIndex];
        }
        return new List<int>();
    }
}
