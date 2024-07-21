using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public LeagueManager leagueManager;
    public LeagueStats leagueStats;

    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "leagueData.json");
    }

    public void SaveData()
    {
        LeagueData data = new LeagueData();

        // Save team stats
        foreach (var kvp in leagueStats.GetAllTeamStats())
        {
            data.teams.Add(new TeamData(kvp.Key, kvp.Value));
        }

        // Save match fixtures
        data.matches = leagueManager.GetMatchData();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to " + filePath);
        PlayerPrefs.SetString("ClubTeamScoreTrackingData", "json");
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LeagueData data = JsonUtility.FromJson<LeagueData>(json);

            // Load team stats
            Dictionary<int, LeagueStats.TeamStats> teamStats = new Dictionary<int, LeagueStats.TeamStats>();
            foreach (var teamData in data.teams)
            {
                var stats = new LeagueStats.TeamStats
                {
                    Played = teamData.played,
                    Won = teamData.won,
                    Lost = teamData.lost,
                    Drawn = teamData.drawn,
                    Points = teamData.points,
                    GoalsFor = teamData.goalsFor,
                    GoalsAgainst = teamData.goalsAgainst,
                    TotalGoals = teamData.totalGoals
                };
                teamStats[teamData.teamIndex] = stats;
            }
            leagueStats.SetAllTeamStats(teamStats);

            // Load match fixtures
            leagueManager.SetFixturesFromMatchData(data.matches);

            Debug.Log("Data loaded from " + filePath);
        }
        else
        {
            Debug.LogWarning("No save file found at " + filePath);
        }
    }
}
