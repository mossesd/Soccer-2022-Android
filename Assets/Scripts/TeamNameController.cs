using UnityEngine;
using System.Collections;

public class TeamNameController : MonoBehaviour
{
	public string[] TeamNames;

	// Use this for initialization
	void Start () {
		if(GetComponent<GUIText>())
			GetComponent<GUIText>().fontSize = Screen.height / 16;
	}
	
	// Update is called once per frame
	void Update () 
	{
		int index = 0;

		if(Application.loadedLevelName == "1stTeamSelection")
		{
			index = TeamSelectionController.teamIndex;
			if(index > 31) index = 0;
			if(index < 0) index = 31;

			GameManager.SharedObject().playerTeamName = TeamNames[index];
			GameManager.SharedObject().playerTeamShortName = GameManager.SharedObject().playerTeamName.Substring(0,3).ToUpper();
		}
		else if(Application.loadedLevelName == "2ndTeamSelection" || Application.loadedLevelName == "MatchesScene")
		{
			index = TeamSelectionController2.teamIndex;
			if(index > 31) index = 0;
			if(index < 0) index = 31;
			
			GameManager.SharedObject().opponentTeamName = TeamNames[index];
			GameManager.SharedObject().opponentTeamShortName = GameManager.SharedObject().opponentTeamName.Substring(0,3).ToUpper();

			//
			int index2 = TeamSelectionController.teamIndex;
			if(index2 > 31) index2 = 0;
			if(index2 < 0) index2 = 31;
			
			GameManager.SharedObject().playerTeamName = TeamNames[index2];
			GameManager.SharedObject().playerTeamShortName = GameManager.SharedObject().playerTeamName.Substring(0,3).ToUpper();
		}

		if(GetComponent<GUIText>())
			GetComponent<GUIText>().text = TeamNames[index];
	}
}
