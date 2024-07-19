using UnityEngine;
using System.Collections;

public class GameSelectionAds : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // TODO changes done by me
        //AdsManager.SharedObject ().ShowHeyzapInterstitial ();

        AdsManager.SharedObject().ShowAdmobInterstitial();

    }

    // Update is called once per frame
    void Update () {
	
	}
}
