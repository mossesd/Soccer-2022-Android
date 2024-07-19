using UnityEngine;
using GoogleMobileAds.Api;
using ChartboostSDK;
using System;
using StartApp;
using UnityEngine.Advertisements;

using SATestAds;

public class AdsManager : StartAppWrapper.AdEventListener
{
	private bool testMode = false;
	private bool loggerEnabled = true;
	private float delay = 0f;

    public string admobAccountID = null;

    public string StartAppID = null;
	public string StartAppDeveloperID = null;

	public string ChartboostAppID = null;
	public string ChartboostAppSignature = null;

	public string admobBannerID = null;
   
    public bool showBannerAtTop = false;
	public string admobIntertitialID = null;
   
    public string HeyzapPublisherID = null;
	public string LeadboltApiKey = null;

	public string UnityAdsId = null;
	private bool showStartAppInsterstitial = false;
	private bool showAdmobInsterstitial = false;
	private bool showHeyzapInsterstitial = false;
	private bool showChartboostInsterstitial = false;
	private bool showLeadboltInsterstitial = false;

	static InterstitialAd admobInterstitial = null;

	static int adNumber = 0;

	static AdsManager instance = null;
	
	static BannerView bannerView = null;

	static bool isInitialized = false;

	private static bool allowConstructor = false;

	public static AdsManager SharedObject()
	{
		if (instance == null)
		{
			AdsManager.allowConstructor = true;
			instance = new AdsManager();
			AdsManager.allowConstructor = false;
		}
		return instance;
	}

	AdsManager()
	{
		if (AdsManager.allowConstructor == false)
		{
			Debug.LogError("AdsManager: Use 'AdsManager.SharedObject()' instead of new 'AdsManager()'");
			Application.Quit();
		}
	}

	public void DisableTestMode()
	{
		testMode = false;
	}

	public void DisableLogger()
	{
		loggerEnabled = false;
	}

	public void SetTestAdDelay(float d)
	{
		delay = d;
	}

	void TestAdFetched(string adName)
	{
		SALogger.Log(adName+" Fetched.");
	}
	
	void TestAdShown(string adName)
	{
		SALogger.Log(adName+" Shown.");
	}
	
	void TestAdDidHide(string adName)
	{
		SALogger.Log(adName+" Hide.");
	}

	public void Initialize()
    { 
         // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(admobAccountID);

        if (testMode)
		{
			TestAds.SharedObject().adDelay = delay; // SECONDS
			TestAds.SharedObject().showBannerAtTop = showBannerAtTop;
			TestAds.SharedObject().onTestAdShown += TestAdShown;
			TestAds.SharedObject().onTestAdFetched += TestAdFetched;
			TestAds.SharedObject().onTestAdHide += TestAdDidHide;
		}

        if (loggerEnabled) {
            SALogger.CreateLogger();
        }

               
#if UNITY_EDITOR
        isInitialized = true;
        return;
#endif

        if (isInitialized == false)
		{
			isInitialized = true;

			if(ChartboostAppID != null)	ChartboostAppID = ChartboostAppID.Trim();
			if(ChartboostAppSignature != null)	ChartboostAppSignature = ChartboostAppSignature.Trim();
			
			if(admobBannerID != null)	admobBannerID = admobBannerID.Trim();
			if(admobIntertitialID != null)	admobIntertitialID = admobIntertitialID.Trim();
			
			if(HeyzapPublisherID != null)	HeyzapPublisherID = HeyzapPublisherID.Trim();
			if(LeadboltApiKey != null)	LeadboltApiKey = LeadboltApiKey.Trim();
	
			if(UnityAdsId != null)	UnityAdsId = UnityAdsId.Trim();

            Init();
        }


    }

  
    private void Init()
	{
		SALogger.Log("AdsManager Initialized");

		if(HeyzapPublisherID != null && HeyzapPublisherID.Length >5)
		{
			#if UNITY_ANDROID
			if(!testMode)
			{
				HeyzapAds.start(HeyzapPublisherID, HeyzapAds.FLAG_DISABLE_AUTOMATIC_FETCHING);
				HZInterstitialAd.AdDisplayListener listener = delegate(string adState, string adTag)
				{
					if(adState.Equals("failed"))
					{
						showHeyzapInsterstitial = false;
						SALogger.Log("Heyzap Failed");
					}
					if(adState.Equals("available"))
					{
						SALogger.Log("Heyzap Fetched");
						if(showHeyzapInsterstitial)
						{
							SALogger.Log("Heyzap Shown");
							HZInterstitialAd.show();
							showHeyzapInsterstitial = false;
						}
					}
					if(adState.Equals("fetch_failed"))
					{
						SALogger.Log("Heyzap Fetch Failed");
						showHeyzapInsterstitial = false;
					}
				};
				HZInterstitialAd.setDisplayListener(listener);
			}
			#endif
		}

		if(admobBannerID != null && admobBannerID.Length > 5)
		{
			#if UNITY_ANDROID
			bannerView = new BannerView(admobBannerID, AdSize.Banner, showBannerAtTop?AdPosition.Top:AdPosition.Bottom);


            // Register for ad events.
            bannerView.OnAdLoaded += this.HandleAdLoaded;
            bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            bannerView.OnAdOpening += this.HandleAdOpened;
            bannerView.OnAdClosed += this.HandleAdClosed;
            bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;


            AdRequest request = new AdRequest.Builder().Build();
            bannerView.LoadAd(request);
       		bannerView.Hide();
			#endif
			SALogger.Log("Admob Banner Request");
		}

		if(admobIntertitialID != null && admobIntertitialID.Length>5)
		{
			SALogger.Log("Admob Interstitial Created");
			#if UNITY_ANDROID
			admobInterstitial = new InterstitialAd(admobIntertitialID);
			admobInterstitial.OnAdLoaded += delegate(object sender, EventArgs args)
            {
                if (showAdmobInsterstitial)
                {
                    SALogger.Log("Admob Interstitial Shown");
                    showAdmobInsterstitial = false;

                    if (admobInterstitial.IsLoaded())
                    {
                        admobInterstitial.Show();
                    }
                }
            };

            // Register for ad events.
            //admobInterstitial.OnAdLoaded += this.HandleInterstitialLoaded;
            admobInterstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
            admobInterstitial.OnAdOpening += this.HandleInterstitialOpened;
            admobInterstitial.OnAdClosed += this.HandleInterstitialClosed;
            admobInterstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;


			#endif
		}

		if(ChartboostAppID != null && ChartboostAppID.Length>5)
		{
			#if UNITY_ANDROID
			GameObject cb = new GameObject("Chartboost");
			cb.AddComponent<Chartboost>();
			UnityEngine.Object.DontDestroyOnLoad(cb);
			CBExternal.init();
			Chartboost.didCacheInterstitial += didCacheInterstitial;
			Chartboost.didFailToLoadInterstitial += didFailToCache;
			#endif
			SALogger.Log("Chartboost Interstitial Created");
		}

		if(LeadboltApiKey != null && LeadboltApiKey.Length>5)
		{

		}
	
	
		if(UnityAdsId != null && UnityAdsId.Length>=5)
		{
			#if UNITY_ANDROID
			if (Advertisement.isSupported) {
				Advertisement.Initialize (UnityAdsId);
			} else {
				SALogger.Log("Platform not supported");
			}
			#endif
			SALogger.Log("Unity ADS Created");
		}
	}


	void DidShowAd() {
//		Debug.Log ("Unity: DidShowAd");
	}
	
	void DidClick() {
//		Debug.Log ("Unity: DidClick");
	}
	
	void DidHideAd() {
//		Debug.Log ("Unity: DidHideAd");
	}

	void DidFailToShowAd(string adError)
	{
	
	}
	
	void DidCacheAd()
	{
        showAdmobInsterstitial = false;
    }



	void didCacheInterstitial(CBLocation location)
	{
		if(showChartboostInsterstitial)
		{
			SALogger.Log("Chartboost Interstitial Fetched");
			showChartboostInsterstitial = false;
			Chartboost.showInterstitial(CBLocation.Default);
			SALogger.Log("Chartboost Interstitial Shown");
		}
	}

	void didFailToCache(CBLocation location, CBImpressionError error)
	{
		if(showChartboostInsterstitial)
			SALogger.Log("Chartboost Fetch Failed");
	
		showChartboostInsterstitial = false;
	}

	public void ShowChartboostInterstitial()
	{
		if(ChartboostAppID != null && ChartboostAppID.Length>5)
		{
			SALogger.Log("Chartboost Interstitial Requested");

			if(testMode)
			{
				TestAds.SharedObject().ShowChartboostTestAd();
				return;
			}
			#if UNITY_ANDROID
			showChartboostInsterstitial = true;
			Chartboost.cacheInterstitial (CBLocation.Default);
			#endif
		}
	}

	public void HideChartboostInterstitial()
	{
		SALogger.Log("Chartboost Interstitial Hide");
		if(testMode)
		{
			TestAds.SharedObject().HideChartboostTestAd();
			return;
		}

		showChartboostInsterstitial = false;
	}

	public void ShowHeyzapInterstitial()
	{
		if(HeyzapPublisherID != null && HeyzapPublisherID.Length >5)
		{
			SALogger.Log("Heyzap Interstitial Requested");
			if (testMode)
			{
				TestAds.SharedObject ().ShowHeyzapTestAd ();
				return;
			}

			#if UNITY_ANDROID
			showHeyzapInsterstitial = true;
			HZInterstitialAd.fetch();
			#endif
		}
	}

	public void HideHeyzapInterstitial()
	{
		SALogger.Log("Heyzap Interstitial Hide");
		showHeyzapInsterstitial = false;

		if(testMode)
			TestAds.SharedObject().HideHeyzapTestAd();
	}

	public void ShowLeadboltInterstitial()
	{
		if(LeadboltApiKey != null && LeadboltApiKey.Length>5)
		{
			SALogger.Log("Leadbolt Interstitial Requested");

			if(testMode)
			{
				TestAds.SharedObject().ShowLeadboltTestAd();
				return;
			}

			#if UNITY_ANDROID
			showLeadboltInsterstitial = true;
			#endif
		}
	}



	void onModuleClosedEvent(string placement){	}

	void onModuleFailedEvent(string placement,string error, bool cached)
	{
		SALogger.Log("LB Failed: "+error);
		showLeadboltInsterstitial = false;
	}

	void onModuleLoadedEvent(string placement){	}

	void onModuleCachedEvent(string placement)
	{
		#if UNITY_EDITOR
		return;
		#endif
		if(showLeadboltInsterstitial)
		{
			showLeadboltInsterstitial = false;
			SALogger.Log("Leadbolt Interstitial Fetched. "+placement);
			SALogger.Log("Leadbolt Interstitial Shown.");
		}
	}

	public void ShowAdmobInterstitial()
	{
		if(admobIntertitialID != null && admobIntertitialID.Length>5)
		{
			SALogger.Log("Admob Interstitial Requested");

			//if(testMode)
			//{
			//	TestAds.SharedObject().ShowAdmobTestAd();
			//	return;
			//}

            #if UNITY_ANDROID
            showAdmobInsterstitial = true;
			AdRequest request = new AdRequest.Builder().Build();
            if (admobInterstitial == null) {
                admobInterstitial = new InterstitialAd(admobIntertitialID);
             }
            admobInterstitial.LoadAd(request);

            if (admobInterstitial.IsLoaded())
            {
                admobInterstitial.Show();
            }
         
            #endif
        }
	}

  
   
    public void ShowAutoAd()
	{
		switch(adNumber)
		{
		case 0:
			ShowAdmobInterstitial();
			break;

		case 1:
                //ShowChartboostInterstitial();
                ShowAdmobInterstitial();

                break;

		case 2:
                //ShowHeyzapInterstitial();
                ShowAdmobInterstitial();

                break;

		case 3:
			//ShowLeadboltInterstitial();
                ShowAdmobInterstitial();

                break;

		case 4:
               
                ShowAdmobInterstitial();

                break;

		case 5:
                //ShowStartAppInterstitial();
                ShowAdmobInterstitial();

                break;

		default:
			ShowAdmobInterstitial();
			break;
		}

		adNumber += 1;
		adNumber = adNumber > 5 ? 0 : adNumber;
	}

	public void ShowBanner()
	{
		if(admobBannerID != null && admobBannerID.Length > 5)
		{
			SALogger.Log("Admob Banner Show ");

			if(testMode)
			{
				TestAds.SharedObject().ShowBanner();
				return;
			}

           #if UNITY_ANDROID
            // bannerView.Show();
			#endif
		}
	}

	public void HideBanner()
	{
		SALogger.Log("Admob Banner Hide");
		if(testMode)
		{
			TestAds.SharedObject().HideBanner();
			return;
		}

#if UNITY_ANDROID
		if (bannerView != null) {
			bannerView.Hide();
		}
    
#endif
    }

	public void HideAdmobInterstitial()
	{
		SALogger.Log("Admob Interstitial Hide");
		if(testMode)
			TestAds.SharedObject().HideAdmobTestAd();

		showAdmobInsterstitial = false;
	}

	public void HideLeadboltInterstitial()
	{
		SALogger.Log("Leadbolt Interstitial Hide");
		showLeadboltInsterstitial = false;
	}

	public void ShowStartAppInterstitial()
	{
		if(StartAppID != null && StartAppID.Length >5)
		{
			SALogger.Log("StartApp Interstitial Requested");

			if(testMode)
			{
				TestAds.SharedObject().ShowStartAppTestAd();
				return;
			}

			#if UNITY_ANDROID
			showStartAppInsterstitial = true;
			StartAppWrapper.loadAd(this);
			#endif
		}
	}

	public void HideStartAppInterstitial()
	{
		if(testMode)
			TestAds.SharedObject().HideStartAppTestAd();

		SALogger.Log("StartApp Interstitial Hide");
		showStartAppInsterstitial = false;
	}

	public void onReceiveAd()
	{
		if(showStartAppInsterstitial)
		{
			SALogger.Log("StartApp Interstitial Fetched");
			//StartAppWrapper.showAd();
			showStartAppInsterstitial = false;
			SALogger.Log("StartApp Interstitial Shown");
		}
	}

	public void onFailedToReceiveAd()
	{
		if(showStartAppInsterstitial)
			SALogger.Log("StartApp Interstitial Failed to Fetch");
	
		showStartAppInsterstitial = false;
	}
	public void ShowUnityADS()
	{
		if(Advertisement.IsReady())
		{ 
			Advertisement.Show(null, new ShowOptions {
				resultCallback = result => {
					SALogger.Log(result.ToString());
				}
			});
		
		}
		else
			SALogger.Log("UnityAD Failed to Fetch");
	}


    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        //Debug.Log("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Debug.Log(
            //"HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        //Debug.Log("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        //Debug.Log("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        //Debug.Log("HandleInterstitialLeftApplication event received");
    }

    #endregion


    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        //Debug.Log("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        //Debug.Log("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        //Debug.Log("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        //Debug.Log("HandleAdLeftApplication event received");
    }

    #endregion
}
