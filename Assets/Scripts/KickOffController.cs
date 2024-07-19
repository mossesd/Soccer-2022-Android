using UnityEngine;
using System.Collections;

public class KickOffController : MonoBehaviour
{
	public GUITexture Team1Flag, Team2Flag;
	public GUIText Team1Name, Team2Name;
	public Material Team1Material, Team2Material,Team1HDMaterial, Team2HDMaterial;

	// Use this for initialization
	void Start()
	{


		Team1Flag.texture = GameManager.SharedObject ().playerTeamFlag;
		Team2Flag.texture = GameManager.SharedObject ().opponentTeamFlag;

		Team1Name.text = GameManager.SharedObject ().playerTeamName;
		Team2Name.text = GameManager.SharedObject ().opponentTeamName;

		Team1Material.mainTexture =  GameManager.SharedObject ().playerTeamTexture;
		Team2Material.mainTexture =  GameManager.SharedObject ().opponentTeamTexture;

		Team1HDMaterial.mainTexture =  GameManager.SharedObject ().playerTeamHDTexture;
		Team2HDMaterial.mainTexture =  GameManager.SharedObject ().opponentTeamHDTexture;
//		Team1HDMaterial.mainTexture =  GameManager.SharedObject ().opponentTeamHDTexture;


	}
}
