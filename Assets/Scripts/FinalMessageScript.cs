using UnityEngine;
using System.Collections;

public class FinalMessageScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		PlayerPrefs.SetInt ("HasPendingCup",0);
		PlayerPrefs.Save ();

		GetComponent<GUIText>().text = PlayerPrefs.GetString ("message");

		if(GetComponent<GUIText>().text.StartsWith("Sorry"))
			GetComponent<GUIText>().color = Color.red;
		else
			GetComponent<GUIText>().color = Color.green;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
