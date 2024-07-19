using UnityEngine;
using System.Collections;

public class PluginTest : MonoBehaviour
{
	// Use this for initialization
	void Awake()
	{
		AdsManager manager = AdsManager.SharedObject();

		manager.SetTestAdDelay(2f);
		manager.DisableTestMode ();
		manager.DisableLogger ();
		manager.showBannerAtTop = false;

		manager.admobBannerID = "";
		manager.admobIntertitialID = "";
		manager.ChartboostAppID = "";
		manager.ChartboostAppSignature = "";
		manager.LeadboltApiKey = "";
		manager.HeyzapPublisherID = "";
		manager.StartAppID = "";
		manager.StartAppDeveloperID = "";

		manager.Initialize();

		manager.ShowBanner ();

		Time.timeScale = 0;
	}

	private bool showingBanner = false;
	void OnGUI ()
	{
		if(GUI.Button(new Rect(0,60+Screen.height*0.01f+Screen.height*0.1f*0,Screen.width,Screen.height*0.1f),"Show Admob Interstitial"))
			AdsManager.SharedObject().ShowAdmobInterstitial();

		if(GUI.Button(new Rect(0,60+Screen.height*0.02f+Screen.height*0.1f*1,Screen.width,Screen.height*0.1f),"Show Chartboost Interstitial"))
			AdsManager.SharedObject().ShowChartboostInterstitial();
	
		if(GUI.Button(new Rect(0,60+Screen.height*0.03f+Screen.height*0.1f*2,Screen.width,Screen.height*0.1f),"Show Heyzap Interstitial"))
			AdsManager.SharedObject().ShowHeyzapInterstitial();
	
		if(GUI.Button(new Rect(0,60+Screen.height*0.04f+Screen.height*0.1f*3,Screen.width,Screen.height*0.1f),"Show Leadbolt Interstitial"))
			AdsManager.SharedObject().ShowLeadboltInterstitial();
	
		if(GUI.Button(new Rect(0,60+Screen.height*0.05f+Screen.height*0.1f*4,Screen.width,Screen.height*0.1f),"Show StartApp Interstitial"))
			AdsManager.SharedObject().ShowStartAppInterstitial();

		if(showingBanner)
		{
			if(GUI.Button(new Rect(0,60+Screen.height*0.07f+Screen.height*0.1f*6,Screen.width,Screen.height*0.1f),"Hide Admob Banner"))
			{
				showingBanner = false;
				AdsManager.SharedObject().HideBanner();
			}
		}
		else
		{
			if(GUI.Button(new Rect(0,60+Screen.height*0.07f+Screen.height*0.1f*6,Screen.width,Screen.height*0.1f),"Show Admob Banner"))
			{
				showingBanner = true;
				AdsManager.SharedObject().ShowBanner();
			}
		}
	}
}
