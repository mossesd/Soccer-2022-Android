using System.Collections.Generic;

[System.Serializable]
public class LeagueData
{
    public List<TeamData> teams;
    public List<MatchData> matches;

    public LeagueData()
    {
        teams = new List<TeamData>();
        matches = new List<MatchData>();
    }
}

[System.Serializable]
public class TeamData
{
    public int teamIndex;
    public int played;
    public int won;
    public int lost;
    public int drawn;
    public int points;
    public int goalsFor;
    public int goalsAgainst;
    public int totalGoals;

    public TeamData(int index, LeagueStats.TeamStats stats)
    {
        teamIndex = index;
        played = stats.Played;
        won = stats.Won;
        lost = stats.Lost;
        drawn = stats.Drawn;
        points = stats.Points;
        goalsFor = stats.GoalsFor;
        goalsAgainst = stats.GoalsAgainst;
        totalGoals = stats.TotalGoals;
    }
}

[System.Serializable]
public class MatchData
{
    public int teamA;
    public int teamB;
    public int round;

    public MatchData(int a, int b, int round)
    {
        teamA = a;
        teamB = b;
        this.round = round;
    }
}
