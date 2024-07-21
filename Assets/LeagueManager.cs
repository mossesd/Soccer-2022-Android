using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeagueManager : MonoBehaviour
{
    private GroupsSceneManager groupsSceneManager;
    private LeagueStats leagueStats;
    private DataManager dataManager;
    private List<int> selectedTeams;
    private List<Match> fixtures = new List<Match>();

    public static LeagueManager Instance {  get;  set; }
    void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }

        groupsSceneManager = FindObjectOfType<GroupsSceneManager>();
        leagueStats = FindObjectOfType<LeagueStats>();
        dataManager = FindObjectOfType<DataManager>();

        if (PlayerPrefs.HasKey("ClubTeamScoreTrackingData"))
            dataManager.LoadData();    
    }

    void Start()
    {
        string tornament = PlayerPrefs.GetString("tournament");

        switch (tornament)
        {
            case "CreateYourOwnTeam":
                ClubPanel();
                break;
        }         
    }

    void ClubPanel()
    {
        if (!PlayerPrefs.HasKey("ClubTeamScoreTrackingData"))
        {
            selectedTeams = groupsSceneManager.GetRandomTeams(10);
            leagueStats.InitializeTeams(selectedTeams);
            GenerateFixtures();
        }
    }

    /*private void OnApplicationQuit()
    {
        dataManager.SaveData();
    }*/

    private void GenerateFixtures()
    {
        int teamCount = selectedTeams.Count;

        for (int round = 0; round < teamCount - 1; round++)
        {
            for (int i = 0; i < teamCount / 2; i++)
            {
                int teamA = selectedTeams[i];
                int teamB = selectedTeams[teamCount - i - 1];
                fixtures.Add(new Match(teamA, teamB, round + 1));
            }

            // Rotate the teams for the next round
            int last = selectedTeams[teamCount - 1];
            for (int i = teamCount - 1; i > 0; i--)
            {
                selectedTeams[i] = selectedTeams[i - 1];
            }
            selectedTeams[0] = last;
        }
    }

    public List<Match> GetFixturesByRound(int round)
    {
        return fixtures.FindAll(match => match.Round == round);
    }

    public List<MatchData> GetMatchData()
    {
        return fixtures.Select(f => new MatchData(f.TeamA, f.TeamB, f.Round)).ToList();
    }

    public Match GetMatchForTeamInFirstRound(int teamIndex, int round)
    {
        return fixtures.FirstOrDefault(match => match.Round == round &&
                                                (match.TeamA == teamIndex || match.TeamB == teamIndex));
    }

    public void SetFixturesFromMatchData(List<MatchData> matchData)
    {
        fixtures = matchData.Select(m => new Match(m.teamA, m.teamB, m.round)).ToList();
    }

    public int GetTotalRounds()
    {
        return fixtures.Max(m => m.Round);
    }

    public void RecordMatchResult(int matchIndex, int goalsForTeamA, int goalsForTeamB)
    {
        var match = fixtures[matchIndex];
        bool teamAWon = goalsForTeamA > goalsForTeamB;
        bool draw = goalsForTeamA == goalsForTeamB;

        leagueStats.UpdateStats(match.TeamA, teamAWon, draw, goalsForTeamA, goalsForTeamB, goalsForTeamA);
        leagueStats.UpdateStats(match.TeamB, !teamAWon && !draw, draw, goalsForTeamB, goalsForTeamA, goalsForTeamB);
    }

    public class Match
    {
        public int TeamA { get; }
        public int TeamB { get; }
        public int Round { get; }

        public Match(int teamA, int teamB, int round)
        {
            TeamA = teamA;
            TeamB = teamB;
            Round = round;
        }
    }
}
