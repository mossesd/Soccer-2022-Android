using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GroupUI : MonoBehaviour
{
    public TMP_Text groupNameText;
    public Transform teamListContainer;
    public GameObject teamInfoPrefab;

    public void SetupGroup(Group group)
    {
        groupNameText.text = group.GroupName;

        foreach (Team team in group.Teams)
        {
            GameObject teamInfo = Instantiate(teamInfoPrefab, teamListContainer);
            teamInfo.GetComponent<TeamInfoUI>().SetupTeamInfo(team);
        }
    }
}
