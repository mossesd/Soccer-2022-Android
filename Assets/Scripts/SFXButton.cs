using UnityEngine;
using System.Collections;

public class SFXButton : MonoBehaviour
{
	public Texture onTexture, offTexture;

	void Update ()
	{
		if(AudioManager.isSFXOn)
			GetComponent<GUITexture>().texture = onTexture;
		else
			GetComponent<GUITexture>().texture = offTexture;
	}
	
	void OnMouseDown()
	{
		AudioManager.isSFXOn = !AudioManager.isSFXOn;
		AudioManager.Save ();
	}
}
