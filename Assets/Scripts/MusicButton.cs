using UnityEngine;
using System.Collections;

public class MusicButton : MonoBehaviour
{
	public Texture onTexture, offTexture;
	
	void Update ()
	{
		if(AudioManager.isMusicOn)
			GetComponent<GUITexture>().texture = onTexture;
		else
			GetComponent<GUITexture>().texture = offTexture;
	}

	void OnMouseDown()
	{
		AudioManager.isMusicOn = !AudioManager.isMusicOn;
		AudioManager.Save ();

		if(AudioManager.isMusicOn)
			AudioManager.PlayBackgroundMusic();
		else
			AudioManager.StopBackgroundMusic();
	}
}
