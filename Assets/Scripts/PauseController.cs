using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{

	public static bool isPaused = false;
	OtherDialoguesActive oda;
	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 1f;
		isPaused = false;
		oda = GameObject.Find ("Main Camera").GetComponent<OtherDialoguesActive> ();
	}
	public void unityAds()
	{
		AdsManager.SharedObject ().ShowUnityADS ();
	}
	// Update is called once per frame
	void Update ()
	{
		if(!oda.isOtherDialogueActive)
		{
		if(Input.GetKeyDown(KeyCode.Escape))
			{
			isPaused = !isPaused;

			if(isPaused)
			{
				AdsManager.SharedObject().ShowBanner();
                    AdsManager.SharedObject().ShowAdmobInterstitial();
                    //					AdsManager.SharedObject().ShowChartboostInterstitial();
                    //					AdsManager.SharedObject().ShowUnityADS();
                    Invoke("unityAds",0.5f);
			}
			else
				{
				AdsManager.SharedObject().HideBanner();
                    AdsManager.SharedObject().HideAdmobInterstitial();
                    //				AdsManager.SharedObject().HideChartboostInterstitial();

                }
			}
		}
		if (isPaused)
		{
			AudioListener.volume=0;
//			Time.timeScale = 0f;
			Invoke("timeScaleZero",0.8f);
			transform.position = new Vector3(0.5f,0.48f,30);
		}
		else
		{
			AudioListener.volume=1;
			Time.timeScale = 1f;
			transform.position = new Vector3(3,3,3);
		}
	
	}

	public static void pause()
	{
		isPaused = !isPaused;
	}
	void timeScaleZero()
	{
		Time.timeScale = 0f;
	}
//	void OnApplicationFocus(bool focusStatus) {
//		isPaused = focusStatus;
//	}
}
