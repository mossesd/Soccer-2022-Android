using UnityEngine;
using System.Collections;

/*
 * This Class can be used with GUITexture Component to simulate a standard button.
 * Basic button functionality can be controlled from the Editor window so you dont
 * need to code at all.
*/

[RequireComponent (typeof (GUITexture))]
public class ButtonController : MonoBehaviour
{
	public Texture hoverTexture;

	[HideInInspector]
	public Texture normalTexture;
	
	void OnMouseEnter()
	{
//		if(hoverTexture != null)
//			guiTexture.texture = hoverTexture;
	}

	void OnMouseExit() 
	{
		if(hoverTexture != null)
			GetComponent<GUITexture>().texture = normalTexture;
	}

	void OnMouseDown()
	{
		if(hoverTexture != null)
			GetComponent<GUITexture>().texture = gameObject.GetComponent<ButtonController>().hoverTexture;
	}
	
	void OnMouseUp()
	{
		if(hoverTexture != null)
			GetComponent<GUITexture>().texture = gameObject.GetComponent<ButtonController>().normalTexture;
	}

	// Update is called once per frame
	void Start ()
	{
		normalTexture = GetComponent<GUITexture>().texture;
//		Rect rect = guiTexture.pixelInset;
//		rect.x = -GetValue (rect.width)/2;
//		rect.y = -GetValue (rect.height) / 2;
//		rect.width = GetValue (rect.width);
//		rect.height = GetValue (rect.height);
//		guiTexture.pixelInset = rect;

		//transform.position = new Vector3 (transform.position.x * Screen.width / 960f,transform.position.y,transform.position.z);
	}

//	float GetValue(float value)
//	{
//		return value * Screen.width / 960f;
//	}
}
