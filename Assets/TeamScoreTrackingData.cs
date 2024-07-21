using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeamScoreTrackingData
{
    public List<GroupAssignment> groupAssignments;
    public List<TeamScore> teamScores;
    public List<MatchPair> matchPairs;
    public List<MatchResult> matchResults;
    public List<GroupMatch> groupMatches;
    public List<Match> matches;

    public TeamScoreTrackingData(
        Dictionary<int, List<int>> groupAssignments,
        Dictionary<int, int> teamScores,
        Dictionary<int, List<int>> matchPairs,
        Dictionary<int, bool> matchResults,
        Dictionary<int, List<int>> groupMatches,
        Dictionary<int, List<int>> matches)
    {
        this.groupAssignments = new List<GroupAssignment>();
        foreach (var group in groupAssignments)
        {
            this.groupAssignments.Add(new GroupAssignment(group.Key, group.Value));
        }

        this.teamScores = new List<TeamScore>();
        foreach (var score in teamScores)
        {
            this.teamScores.Add(new TeamScore(score.Key, score.Value));
        }

        this.matchPairs = new List<MatchPair>();
        foreach (var pair in matchPairs)
        {
            this.matchPairs.Add(new MatchPair(pair.Key, pair.Value));
        }

        this.matchResults = new List<MatchResult>();
        foreach (var result in matchResults)
        {
            this.matchResults.Add(new MatchResult(result.Key, result.Value));
        }

        this.groupMatches = new List<GroupMatch>();
        foreach (var groupMatch in groupMatches)
        {
            this.groupMatches.Add(new GroupMatch(groupMatch.Key, groupMatch.Value));
        }

        this.matches = new List<Match>();
        foreach (var match in matches)
        {
            this.matches.Add(new Match(match.Key, match.Value));
        }
    }

    public Dictionary<int, List<int>> GetGroupAssignments()
    {
        var dictionary = new Dictionary<int, List<int>>();
        foreach (var groupAssignment in groupAssignments)
        {
            dictionary[groupAssignment.key] = groupAssignment.value;
        }
        return dictionary;
    }

    public Dictionary<int, int> GetTeamScores()
    {
        var dictionary = new Dictionary<int, int>();
        foreach (var teamScore in teamScores)
        {
            dictionary[teamScore.key] = teamScore.value;
        }
        return dictionary;
    }

    public Dictionary<int, List<int>> GetMatchPairs()
    {
        var dictionary = new Dictionary<int, List<int>>();
        foreach (var matchPair in matchPairs)
        {
            dictionary[matchPair.key] = matchPair.value;
        }
        return dictionary;
    }

    public Dictionary<int, bool> GetMatchResults()
    {
        var dictionary = new Dictionary<int, bool>();
        foreach (var matchResult in matchResults)
        {
            dictionary[matchResult.key] = matchResult.value;
        }
        return dictionary;
    }

    public Dictionary<int, List<int>> GetGroupMatches()
    {
        var dictionary = new Dictionary<int, List<int>>();
        foreach (var groupMatch in groupMatches)
        {
            dictionary[groupMatch.key] = groupMatch.value;
        }
        return dictionary;
    }

    public Dictionary<int, List<int>> GetMatches()
    {
        var dictionary = new Dictionary<int, List<int>>();
        foreach (var match in matches)
        {
            dictionary[match.key] = match.value;
        }
        return dictionary;
    }

    [Serializable]
    public struct GroupAssignment
    {
        public int key;
        public List<int> value;

        public GroupAssignment(int key, List<int> value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public struct TeamScore
    {
        public int key;
        public int value;

        public TeamScore(int key, int value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public struct MatchPair
    {
        public int key;
        public List<int> value;

        public MatchPair(int key, List<int> value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public struct MatchResult
    {
        public int key;
        public bool value;

        public MatchResult(int key, bool value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public struct GroupMatch
    {
        public int key;
        public List<int> value;

        public GroupMatch(int key, List<int> value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
    public struct Match
    {
        public int key;
        public List<int> value;

        public Match(int key, List<int> value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
