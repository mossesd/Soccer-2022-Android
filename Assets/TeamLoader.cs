using UnityEngine;
using System;
using System.Collections.Generic;

public class TeamLoader : MonoBehaviour
{
    public TextAsset jsonFile; // Assign your JSON file here in the inspector

    private TeamData teamData;

    [System.Serializable]
    public class Player
    {
        public string name;
        public int age;
        public int number;
        public string position;
    }

    [System.Serializable]
    public class Team
    {
        public string country;
        public string jerseyColor;
        public Player[] players;
    }

    [System.Serializable]
    public class TeamData
    {
        public Team[] teams;
    }

    void Start()
    {
        LoadTeams();
        string selectedCountry = PlayerPrefs.GetString("SelectedTeamName", "");
        if (!string.IsNullOrEmpty(selectedCountry))
        {
            PrintTeamInfo(selectedCountry);
        }
        else
        {
            Debug.LogError("No country selected.");
        }
    }

    void LoadTeams()
    {
        if (jsonFile != null)
        {
            teamData = JsonUtility.FromJson<TeamData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("JSON file not assigned in the inspector.");
        }
    }

    void PrintTeamInfo(string country)
    {
        Team selectedTeam = Array.Find(teamData.teams, team => team.country == country);
        if (selectedTeam != null)
        {
            Debug.Log($"Team: {selectedTeam.country}");
            Debug.Log($"Jersey Color: {selectedTeam.jerseyColor}");
            foreach (Player player in selectedTeam.players)
            {
                Debug.Log($"Player Name: {player.name}, Age: {player.age}, Number: {player.number}, Position: {player.position}");
            }
        }
        else
        {
            Debug.LogError($"Team for country {country} not found.");
        }
    }
}
