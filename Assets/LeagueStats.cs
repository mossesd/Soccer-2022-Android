using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeagueStats : MonoBehaviour
{
    public class TeamStats
    {
        public int Played { get; set; } = 0;
        public int Won { get; set; } = 0;
        public int Lost { get; set; } = 0;
        public int Drawn { get; set; } = 0;
        public int Points { get; set; } = 0;
        public int GoalsFor { get; set; } = 0;
        public int GoalsAgainst { get; set; } = 0;
        public int TotalGoals { get; set; } = 0;
    }

    private Dictionary<int, TeamStats> teamStats = new Dictionary<int, TeamStats>();

    public void InitializeTeams(List<int> teams)
    {
        foreach (int team in teams)
        {
            if (!teamStats.ContainsKey(team))
            {
                teamStats[team] = new TeamStats();
            }
            else
            {
                Debug.LogWarning($"Duplicate team index detected: {team}");
            }
        }
       
    }


    public void UpdateStats(int teamIndex, bool won, bool drawn, int goalsFor, int goalsAgainst, int totalGoals)
    {
        if (!teamStats.ContainsKey(teamIndex)) return;

        var stats = teamStats[teamIndex];
        stats.Played++;
        stats.GoalsFor += goalsFor;
        stats.GoalsAgainst += goalsAgainst;
        stats.TotalGoals += totalGoals;
        
        if (won)
        {
            stats.Won++;
            stats.Points += 3;
        }
        else if (drawn)
        {
            stats.Drawn++;
            stats.Points += 1;
        }
        else
        {
            stats.Lost++;
        }

        teamStats[teamIndex] = stats;
    }

    public TeamStats GetTeamStats(int teamIndex)
    {
        if (teamStats.ContainsKey(teamIndex))
        {
            return teamStats[teamIndex];
        }
        return null;
    }

    public Dictionary<int, TeamStats> GetAllTeamStats()
    {
        return new Dictionary<int, TeamStats>(teamStats);
    }

    public void SetAllTeamStats(Dictionary<int, TeamStats> stats)
    {
        teamStats = new Dictionary<int, TeamStats>(stats);
    }

    public List<KeyValuePair<int, TeamStats>> GetSortedTeams()
    {
        return teamStats.OrderByDescending(t => t.Value.Points)
                        .ThenByDescending(t => t.Value.TotalGoals)
                        .ToList();
    }
}
