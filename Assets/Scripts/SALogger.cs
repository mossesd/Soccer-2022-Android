using UnityEngine;
using System.Collections;

public class SALogger : MonoBehaviour
{
	private static int Requests = 0;
	private Rect windowRect = new Rect(0,0,0,0);
	private static string str = "None";
	private static SALogger salogger = null;

	float GUIV(float x)
	{
		return x * (Screen.width<Screen.height?Screen.width:Screen.height) / 340f;
	}
		
	void Start()
	{
		if(salogger != null)
		{
			Destroy(gameObject);
			return;
		}

		salogger = gameObject.GetComponent<SALogger>();

		float height = GUIV (180);
		if(AdsManager.SharedObject().HeyzapPublisherID != null && AdsManager.SharedObject().HeyzapPublisherID.Length >5)
			height += GUIV (27);
		if(AdsManager.SharedObject().admobBannerID != null && AdsManager.SharedObject().admobBannerID.Length > 5)
			height += GUIV (27);
		if(AdsManager.SharedObject().admobIntertitialID != null && AdsManager.SharedObject().admobIntertitialID.Length>5)
			height += GUIV (27);
		if(AdsManager.SharedObject().ChartboostAppID != null && AdsManager.SharedObject().ChartboostAppID.Length>5)
			height += GUIV (27);
		if(AdsManager.SharedObject().LeadboltApiKey != null && AdsManager.SharedObject().LeadboltApiKey.Length>5)
			height += GUIV (27);
		if(AdsManager.SharedObject().StartAppID != null && AdsManager.SharedObject().StartAppID.Length>5)
			height += GUIV (27);
		if(AdsManager.SharedObject().UnityAdsId != null && AdsManager.SharedObject().UnityAdsId.Length>5)
			height += GUIV (27);
		
		height += GUIV (90);
		
		windowRect = new Rect(0,0,GUIV(200),height);
	}
	
	void OnGUI()
	{
		windowRect = GUI.Window(0, windowRect, DoMyWindow, "");
	}
	
	void DoMyWindow(int windowID)
	{
		float y = GUIV (34);
		GUI.DragWindow(new Rect(0, 0, GUIV(200), GUIV(50)));
		GUI.color = Color.white;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.fontSize = (int)GUIV (22f);
		GUI.Label (new Rect(0,0,GUIV(200),GUIV(35)),"AdsManager");

		GUI.skin.label.fontSize = (int)GUIV (20f);
		if(AdsManager.SharedObject().HeyzapPublisherID != null && AdsManager.SharedObject().HeyzapPublisherID.Length >5)
		{
			GUI.color = Color.green;
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Heyzap Interstitial");
		}
		else
		{
			GUI.color = Color.red;
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Heyzap Ints. OFF");
		}
		
		if(AdsManager.SharedObject().admobBannerID != null && AdsManager.SharedObject().admobBannerID.Length > 5)
		{
			GUI.color = Color.green;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Admob Banner");
		}
		else
		{
			GUI.color = Color.red;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Admob Banner OFF");
		}
		
		if(AdsManager.SharedObject().admobIntertitialID != null && AdsManager.SharedObject().admobIntertitialID.Length>5)
		{
			GUI.color = Color.green;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Admob Interstitial");
		}
		else
		{
			GUI.color = Color.red;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Admob Ints. OFF");
		}
		
		if(AdsManager.SharedObject().ChartboostAppID != null && AdsManager.SharedObject().ChartboostAppID.Length>5)
		{
			GUI.color = Color.green;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Chartboost Interstitial");
		}
		else
		{
			GUI.color = Color.red;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Chartboost Ints. OFF");
		}
		
		if(AdsManager.SharedObject().LeadboltApiKey != null && AdsManager.SharedObject().LeadboltApiKey.Length>5)
		{
			GUI.color = Color.green;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Leadbolt Interstitial");
		}
		else
		{
			GUI.color = Color.red;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"Leadbolt Ints. OFF");
		}
	

		if(AdsManager.SharedObject().StartAppID != null && AdsManager.SharedObject().StartAppID.Length>5)
		{
			GUI.color = Color.green;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"StartApp Interstitial");
		}
		else
		{
			GUI.color = Color.red;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"StartApp Ints. OFF");
		}
		if(AdsManager.SharedObject().UnityAdsId != null && AdsManager.SharedObject().UnityAdsId.Length>5)
		{
			GUI.color = Color.green;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"unityADS Interstitial");
		}
		else
		{
			GUI.color = Color.red;
			y += GUIV (27);
			GUI.Label (new Rect(0,y,GUIV(200),GUIV(27)),"unityADS Ints. OFF");
		}
		
		GUI.color = Color.white;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.label.fontSize = (int)GUIV (16f);
		
		y += GUIV (27);
		GUI.Label (new Rect(GUIV(10),y,GUIV(180),GUIV(50)),"RL:"+str);
	}

	public static void CreateLogger()
	{
		GameObject logger = new GameObject("SALogger");
		logger.AddComponent<SALogger>();
		UnityEngine.Object.DontDestroyOnLoad(logger);
	}

	public static void Log(string lstr)
	{
		Requests += 1;
		str = lstr + " " + Requests;
	}
}
