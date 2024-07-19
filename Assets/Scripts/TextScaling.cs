using UnityEngine;
using System.Collections;

public class TextScaling : MonoBehaviour
{
	public int fontSize = 10;

	void FixedUpdate ()
	{
		GetComponent<GUIText>().fontSize = fontSize * Screen.height / 800;
	}
}