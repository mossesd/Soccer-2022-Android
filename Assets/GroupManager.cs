using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupManager : MonoBehaviour
{
    public static GroupManager Instance;

    public List<Group> groups = new List<Group>();
    public TeamSelectionController teamSelectionController;
    public TeamNameController teamNameController;
    public GameObject groupUIPrefab;
    public Transform groupContainer;

    public Image matchFlag1;
    public Image matchFlag2;
    public TMP_Text matchTeamName1;
    public TMP_Text matchTeamName2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CreateGroups();
        DisplayGroups();
        MatchSelectedTeam();
    }

    void CreateGroups()
    {
        List<Team> teams = new List<Team>();
        for (int i = 0; i < teamSelectionController.teams.Length; i++)
        {
            teams.Add(new Team { Name = teamNameController.TeamNames[i], Points = 0, GoalDifference = 0, GoalsScored = 0 });
        }

        // Shuffle teams to create random groups
        for (int i = teams.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Team temp = teams[i];
            teams[i] = teams[j];
            teams[j] = temp;
        }

        // Create 8 groups with 4 teams each
        for (int i = 0; i < 8; i++)
        {
            Group group = new Group { GroupName = "Group " + (char)('A' + i), Teams = new List<Team>(), Matches = new List<Match>() };
            for (int j = 0; j < 4; j++)
            {
                group.Teams.Add(teams[i * 4 + j]);
            }
            group.Matches = GenerateFixtures(group.Teams);
            groups.Add(group);
        }
    }

    List<Match> GenerateFixtures(List<Team> teams)
    {
        List<Match> fixtures = new List<Match>();
        for (int i = 0; i < teams.Count; i++)
        {
            for (int j = i + 1; j < teams.Count; j++)
            {
                fixtures.Add(new Match { Team1 = teams[i], Team2 = teams[j], IsPlayed = false });
            }
        }
        return fixtures;
    }

    void DisplayGroups()
    {
        foreach (Group group in groups)
        {
            GameObject groupUI = Instantiate(groupUIPrefab, groupContainer);
            groupUI.GetComponent<GroupUI>().SetupGroup(group);
        }
    }

    void MatchSelectedTeam()
    {
        int selectedIndex = TeamSelectionController.teamIndex;
        Team selectedTeam = groups.SelectMany(g => g.Teams).FirstOrDefault(t => t.Name == teamNameController.TeamNames[selectedIndex]);

        if (selectedTeam != null)
        {
            Group selectedGroup = groups.First(g => g.Teams.Contains(selectedTeam));
            List<Group> otherGroups = groups.Where(g => g != selectedGroup).ToList();

            Group randomGroup = otherGroups[Random.Range(0, otherGroups.Count)];
            Team randomTeam = randomGroup.Teams[Random.Range(0, randomGroup.Teams.Count)];

            matchFlag1.sprite = ConvertTextureToSprite((Texture2D)teamSelectionController.teams[selectedIndex]);
            matchTeamName1.text = selectedTeam.Name;

            int randomTeamIndex = teamNameController.TeamNames.ToList().IndexOf(randomTeam.Name);
            matchFlag2.sprite = ConvertTextureToSprite((Texture2D)teamSelectionController.teams[randomTeamIndex]);
            matchTeamName2.text = randomTeam.Name;
        }
    }

    Sprite ConvertTextureToSprite(Texture2D texture)
    {
        if (texture == null) return null;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, pivot);
    }
}
