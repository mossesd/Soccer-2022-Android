using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
public class adsShow : MonoBehaviour {

	// Use this for initialization
	void Awake()
	{
		AdsManager manager = AdsManager.SharedObject();

        manager.admobIntertitialID = "ca-app-pub-2330175674430713/7425457395";

        manager.UnityAdsId= "";
		manager.Initialize();
	}
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void callUnityAds()
	{
		AdsManager.SharedObject ().ShowUnityADS ();

	}
	public void AdmobInterstital()
	{
		AdsManager.SharedObject ().ShowAdmobInterstitial();
	}

}
