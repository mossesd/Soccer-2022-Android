using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TeamScoreTracking : MonoBehaviour
{
    public Dictionary<int, List<int>> groupAssignments = new Dictionary<int, List<int>>();
    public Dictionary<int, int> teamScores = new Dictionary<int, int>();
    public Dictionary<int, List<int>> matchPairs = new Dictionary<int, List<int>>();
    public Dictionary<int, bool> matchResults = new Dictionary<int, bool>();
    public Dictionary<int, List<int>> groupMatches = new Dictionary<int, List<int>>();
    public Dictionary<int, List<int>> matches = new Dictionary<int, List<int>>();

    public static TeamScoreTracking Instance { get; private set; }

    private int matchCounter = 0;
    private const string fileName = "NationalTeamScoreTrackingData.json";

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
        if (PlayerPrefs.HasKey("NationalTeamScoreTrackingData"))
            LoadData();
    }

    /*private void OnApplicationQuit()
    {
        SaveData();
    }*/

    public void SaveData()
    {     
        TeamScoreTrackingData data = new TeamScoreTrackingData(
            groupAssignments,
            teamScores,
            matchPairs,
            matchResults,
            groupMatches,
            matches
        );

        string json = JsonUtility.ToJson(data);
        JsonUtilityHelper.SaveToFile(fileName, json);

        PlayerPrefs.SetString("NationalTeamScoreTrackingData", "json");
    }

    public void LoadData()
    {
        string json = JsonUtilityHelper.LoadFromFile(fileName);
        if (!string.IsNullOrEmpty(json))
        {
            TeamScoreTrackingData data = JsonUtility.FromJson<TeamScoreTrackingData>(json);

            if (data != null)
            {
                this.groupAssignments = data.GetGroupAssignments();
                this.teamScores = data.GetTeamScores();
                this.matchPairs = data.GetMatchPairs();
                this.matchResults = data.GetMatchResults();
                this.groupMatches = data.GetGroupMatches();
                this.matches = data.GetMatches();
            }
            else
            {
                Debug.LogError("Deserialized data is null.");
            }

            Debug.Log("Data loaded successfully.");
            //LogCurrentState();
        }
        else
        {
            Debug.Log("No saved data found.");
        }
    }

    private void LogCurrentState()
    {
        Debug.Log($"groupAssignments: {groupAssignments.Count}");
        Debug.Log($"teamScores: {teamScores.Count}");
        Debug.Log($"matchPairs: {matchPairs.Count}");
        Debug.Log($"matchResults: {matchResults.Count}");
        Debug.Log($"groupMatches: {groupMatches.Count}");
        Debug.Log($"matches: {matches.Count}");

        foreach (var group in groupAssignments)
        {
            Debug.Log($"Group {group.Key} has teams: {string.Join(", ", group.Value)}");
        }

        foreach (var score in teamScores)
        {
            Debug.Log($"Team {score.Key} has score: {score.Value}");
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
