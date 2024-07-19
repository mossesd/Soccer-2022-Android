using UnityEngine;
using System.Collections;

public class ScreenSizeManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Rect insets = GetComponent<GUITexture>().pixelInset;
        insets.width *= Screen.width / 480f;
        insets.height *= Screen.width / 480f;

        GetComponent<GUITexture>().pixelInset = insets;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
