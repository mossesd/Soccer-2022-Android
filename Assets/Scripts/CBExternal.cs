using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ChartboostSDK {
	public class CBExternal {
		private static string _logTag = "ChartboostSDK";
		
		public static void Log (string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		private static bool checkInitialized() {
			if (Chartboost.isInitialized) {
				return true;
			} else {
				Debug.LogError("The Chartboost SDK needs to be initialized before we can show any ads");
				return false;
			}
		}

#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// Initializes the Chartboost plugin.
		/// This must be called before using any other Chartboost features.
		public static void init() {
			Log("Unity : init");
			
			// Will verify all the id and signatures against example ones.
//			CBSettings.getIOSAppId ();
//			CBSettings.getIOSAppSecret ();
//			CBSettings.getAndroidAppId ();
//			CBSettings.getAndroidAppSecret ();
//			CBSettings.getAmazonAppId ();
//			CBSettings.getAmazonAppSecret ();

				
		}
		
		/// Caches an interstitial. Location is optional.
		public static void cacheInterstitial(CBLocation location) {
			Log("Unity : cacheInterstitial at location = " + location.ToString());
		}
		
		/// Checks for a cached an interstitial. Location is optional.
		public static bool hasInterstitial(CBLocation location) {
			Log("Unity : hasInterstitial at location = " + location.ToString());
			return false;
		}
		
		/// Loads an interstitial. Location is optional.
		public static void showInterstitial(CBLocation location) {
			Log("Unity : showInterstitial at location = " + location.ToString());
		}
		
		/// Caches the more apps screen. Location is optional.
		public static void cacheMoreApps(CBLocation location) {
			Log("Unity : cacheMoreApps at location = " + location.ToString());
		}
		
		/// Checks to see if the more apps screen is cached. Location is optional.
		public static bool hasMoreApps(CBLocation location) {
			Log("Unity : hasMoreApps at location = " + location.ToString());
			return false;
		}
		
		/// Shows the more apps screen. Location is optional.
		public static void showMoreApps(CBLocation location) {
			Log("Unity : showMoreApps at location = " + location.ToString());
		}
		
		public static void cacheInPlay(CBLocation location) {
			Log("Unity : cacheInPlay at location = " + location.ToString());
		}
		
		public static bool hasInPlay(CBLocation location) {
			Log("Unity : hasInPlay at location = " + location.ToString());
			return false;
		}
		
		public static CBInPlay getInPlay(CBLocation location) {
			Log("Unity : getInPlay at location = " + location.ToString());
			return null;
		}
		
		/// Caches a rewarded video. Location is optional.
		public static void cacheRewardedVideo(CBLocation location) {
			Log("Unity : cacheRewardedVideo at location = " + location.ToString());
		}
		
		
		/// Checks for a cached a rewarded video. Location is optional.
		public static bool hasRewardedVideo(CBLocation location) {
			Log("Unity : hasRewardedVideo at location = " + location.ToString());
			return false;
		}
		
		/// Loads a rewarded video. Location is optional.
		public static void showRewardedVideo(CBLocation location) {
			Log("Unity : showRewardedVideo at location = " + location.ToString());
		}
		
		// Sends back the reponse by the delegate call for ShouldDisplayInterstitial
		public static void chartBoostShouldDisplayInterstitialCallbackResult(bool result) {
			Log("Unity : chartBoostShouldDisplayInterstitialCallbackResult");
		}
		
		// Sends back the reponse by the delegate call for ShouldDisplayRewardedVideo
		public static void chartBoostShouldDisplayRewardedVideoCallbackResult(bool result) {
			Log("Unity : chartBoostShouldDisplayRewardedVideoCallbackResult");
		}
		
		// Sends back the reponse by the delegate call for ShouldDisplayMoreApps
		public static void chartBoostShouldDisplayMoreAppsCallbackResult(bool result) {
			Log("Unity : chartBoostShouldDisplayMoreAppsCallbackResult");
		}
		
		/// Sets the name of the game object to be used by the Chartboost iOS SDK
		public static void setGameObjectName(string name) {
			Log("Unity : Set Game object name for callbacks to = " + name);
		}
		
		/// Set the custom id used for rewarded video call
		public static void setCustomId(string id) {
			Log("Unity : setCustomId to = " + id);
		}
		
		/// Get the custom id used for rewarded video call
		public static string getCustomId() {
			Log("Unity : getCustomId");
			return "";
		}
		
		/// Confirm if an age gate passed or failed. When specified
		/// Chartboost will wait for this call before showing the ios app store
		public static void didPassAgeGate(bool pass) {
			Log("Unity : didPassAgeGate with value = " + pass);
		}
		
		/// Open a URL using a Chartboost Custom Scheme
		public static void handleOpenURL(string url, string sourceApp) {
			Log("Unity : handleOpenURL at url = " + url + " for app = " + sourceApp);
		}
		
		/// Set to true if you would like to implement confirmation for ad clicks, such as an age gate.
		/// If using this feature, you should call CBBinding.didPassAgeGate() in your didClickInterstitial.
		public static void setShouldPauseClickForConfirmation(bool pause) {
			Log("Unity : setShouldPauseClickForConfirmation with value = " + pause);
		}
		
		/// Set to false if you want interstitials to be disabled in the first user session
		public static void setShouldRequestInterstitialsInFirstSession(bool request) {
			Log("Unity : setShouldRequestInterstitialsInFirstSession with value = " + request);
		}
		
		public static bool getAutoCacheAds() {
			Log("Unity : getAutoCacheAds");
			return false;
		}
		
		public static void setAutoCacheAds(bool autoCacheAds) {
			Log("Unity : setAutoCacheAds with value = " + autoCacheAds);
		}
		
		public static void setShouldDisplayLoadingViewForMoreApps(bool shouldDisplay) {
			Log("Unity : setShouldDisplayLoadingViewForMoreApps with value = " + shouldDisplay);
		}
		
		public static void setShouldPrefetchVideoContent(bool shouldPrefetch) {
			Log("Unity : setShouldPrefetchVideoContent with value = " + shouldPrefetch);
		}
		
		public static void pause(bool paused) {
			Log("Unity : pause");
		}
		
		/// Shuts down the Chartboost plugin
		public static void destroy() {
			Log("Unity : destroy");
		}
		
		/// Used to notify Chartboost that the Android back button has been pressed
		/// Returns true to indicate that Chartboost has handled the event and it should not be further processed
		public static bool onBackPressed() {
			Log("Unity : onBackPressed");
			return true;
		}

		public static void trackInAppGooglePlayPurchaseEvent(string title, string description, string price, string currency, string productID, string purchaseData, string purchaseSignature) {
			Log("Unity: trackInAppGooglePlayPurchaseEvent");
		}
		
		public static void trackInAppAmazonStorePurchaseEvent(string title, string description, string price, string currency, string productID, string userID, string purchaseToken) {
			Log("Unity: trackInAppAmazonStorePurchaseEvent");
		}
		
		public static void trackInAppAppleStorePurchaseEvent(string receipt, string productTitle, string productDescription, string productPrice, string productCurrency, string productIdentifier) {
			Log("Unity : trackInAppAppleStorePurchaseEvent");
		}

		public static bool isAnyViewVisible() {
			Log("Unity : isAnyViewVisible");
			return false;
		}
		
#elif UNITY_ANDROID
		private static AndroidJavaObject _plugin;
		
		/// Initialize the android sdk
		public static void init() {
			// get the AppID and AppSecret from CBSettings

			string appID = "";
			string appSecret = "";
			
			if(AdsManager.SharedObject() != null)
			{
				appID = AdsManager.SharedObject().ChartboostAppID;
				appSecret = AdsManager.SharedObject().ChartboostAppSignature;
				
				if(appID.Length > 5 && appSecret.Length > 5)
				{
//					CBExternal.setAutoCacheAds(false);

					Chartboost.isInitialized = true;

					using (var pluginClass = new AndroidJavaClass("com.chartboost.sdk.unity.CBPlugin"))
						_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
					_plugin.Call("init", appID, appSecret);

//					Chartboost.cacheInterstitial (CBLocation.Default);
				}
			}
		}

		/// Check to see if any chartboost ad or view is visible
		public static bool isAnyViewVisible() {
			bool handled = false;
			if (!checkInitialized())
				return handled;

			handled = _plugin.Call<bool>("isAnyViewVisible");
			Log("Android : isAnyViewVisible = " + handled );

			return handled;
		}

		/// Caches an interstitial. Location is optional.
		public static void cacheInterstitial(CBLocation location) {
			if (!checkInitialized())
				return;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return;
			}
			_plugin.Call("cacheInterstitial", location.ToString());
			Log("Android : cacheInterstitial at location = " + location.ToString());
		}
		
		/// Checks for a cached an interstitial. Location is optional.
		public static bool hasInterstitial(CBLocation location) {
			if (!checkInitialized())
				return false;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return false;
			}
			Log("Android : hasInterstitial at location = " + location.ToString());
			return _plugin.Call<bool>("hasInterstitial", location.ToString());
		}
		
		/// Loads an interstitial. Location is optional.
		public static void showInterstitial(CBLocation location) {
			if (!checkInitialized())
				return;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return;
			}
			_plugin.Call("showInterstitial", location.ToString());
			Log("Android : showInterstitial at location = " + location.ToString());
		}
		
		/// Caches the more apps screen. Location is optional.
		public static void cacheMoreApps(CBLocation location) {
			if (!checkInitialized())
				return;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return;
			};
			_plugin.Call("cacheMoreApps", location.ToString());
			Log("Android : cacheMoreApps at location = " + location.ToString());
		}
		
		/// Checks to see if the more apps screen is cached. Location is optional.
		public static bool hasMoreApps(CBLocation location) {
			if (!checkInitialized())
				return false;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return false;
			}
			Log("Android : hasMoreApps at location = " + location.ToString());
			return _plugin.Call<bool>("hasMoreApps", location.ToString());
		}
		
		/// Shows the more apps screen. Location is optional.
		public static void showMoreApps(CBLocation location) {
			if (!checkInitialized())
				return;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return;
			}
			_plugin.Call("showMoreApps", location.ToString());
			Log("Android : showMoreApps at location = " + location.ToString());
		}
		
		public static void cacheInPlay(CBLocation location) {
			if (!checkInitialized())
				return;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return;
			}
			_plugin.Call("cacheInPlay", location.ToString());
			Log("Android : cacheInPlay at location = " + location.ToString());
		}
		
		public static bool hasInPlay(CBLocation location) {
			if (!checkInitialized())
				return false;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return false;
			}
			Log("Android : hasInPlay at location = " + location.ToString());
			return _plugin.Call<bool>("hasCachedInPlay", location.ToString());
		}
		
		public static CBInPlay getInPlay(CBLocation location) {
			Log("Android : getInPlay at location = " + location.ToString());
			if (!checkInitialized())
				return null;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return null;
			}
			try 
			{
				AndroidJavaObject androidInPlayAd = _plugin.Call<AndroidJavaObject>("getInPlay", location.ToString());
				CBInPlay inPlayAd = new CBInPlay(androidInPlayAd, _plugin);
				return inPlayAd;
			}
			catch
			{
				return null;
			}
		}
		
		/// Caches a rewarded video. 
		public static void cacheRewardedVideo(CBLocation location) {
			if (!checkInitialized())
				return;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return;
			}
			_plugin.Call("cacheRewardedVideo", location.ToString());
			Log("Android : cacheRewardedVideo at location = " + location.ToString());
		}
		
		/// Checks for a cached a rewarded video. 
		public static bool hasRewardedVideo(CBLocation location) {
			if (!checkInitialized())
				return false;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return false;
			}
			Log("Android : hasRewardedVideo at location = " + location.ToString());
			return _plugin.Call<bool>("hasRewardedVideo", location.ToString());
		}
		
		/// Loads a rewarded video. 
		public static void showRewardedVideo(CBLocation location) {
			if (!checkInitialized())
				return;
			else if(location == null) {
				Debug.LogError("Chartboost SDK: location passed is null cannot perform the operation requested");
				return;
			}
			_plugin.Call("showRewardedVideo", location.ToString());
			Log("Android : showRewardedVideo at location = " + location.ToString());
		}
		
		// Sends back the reponse by the delegate call for ShouldDisplayInterstitial
		public static void chartBoostShouldDisplayInterstitialCallbackResult(bool result) {
			if (!checkInitialized())
				return;
			_plugin.Call("chartBoostShouldDisplayInterstitialCallbackResult", result);
			Log("Android : chartBoostShouldDisplayInterstitialCallbackResult");
		}
		
		// Sends back the reponse by the delegate call for ShouldDisplayRewardedVideo
		public static void chartBoostShouldDisplayRewardedVideoCallbackResult(bool result) {
			if (!checkInitialized())
				return;
			_plugin.Call("chartBoostShouldDisplayRewardedVideoCallbackResult", result);
			Log("Android : chartBoostShouldDisplayRewardedVideoCallbackResult");
		}
		
		// Sends back the reponse by the delegate call for ShouldDisplayMoreApps
		public static void chartBoostShouldDisplayMoreAppsCallbackResult(bool result) {
			if (!checkInitialized())
				return;
			_plugin.Call("chartBoostShouldDisplayMoreAppsCallbackResult", result);
			Log("Android : chartBoostShouldDisplayMoreAppsCallbackResult");
		}
		
		public static void didPassAgeGate(bool pass) {
			_plugin.Call ("didPassAgeGate",pass);
		}
		
		public static void setShouldPauseClickForConfirmation(bool shouldPause) {
			_plugin.Call ("setShouldPauseClickForConfirmation",shouldPause);
		}
		
		public static String getCustomId() {
			return _plugin.Call<String>("getCustomId");
		}
		
		public static void setCustomId(String customId) {
			_plugin.Call("setCustomId", customId);
		}
		
		public static bool getAutoCacheAds() {
			return _plugin.Call<bool>("getAutoCacheAds");
		}
		
		public static void setAutoCacheAds(bool autoCacheAds) {
			_plugin.Call ("setAutoCacheAds", autoCacheAds);
		}
		
		public static void setShouldRequestInterstitialsInFirstSession(bool shouldRequest) {
			_plugin.Call ("setShouldRequestInterstitialsInFirstSession", shouldRequest);
		}
		
		public static void setShouldDisplayLoadingViewForMoreApps(bool shouldDisplay) {
			_plugin.Call ("setShouldDisplayLoadingViewForMoreApps", shouldDisplay);
		}
		
		public static void setShouldPrefetchVideoContent(bool shouldPrefetch) {
			_plugin.Call ("setShouldPrefetchVideoContent", shouldPrefetch);
		}
		
		/// Sets the name of the game object to be used by the Chartboost Android SDK
		public static void setGameObjectName(string name) {
			_plugin.Call("setGameObjectName", name);
		}
		
		/// Informs the Chartboost SDK about the lifecycle of your app
		public static void pause(bool paused) {
			if (!checkInitialized())
				return;
			
			_plugin.Call("pause", paused);
			Log("Android : pause");
		}
		
		/// Shuts down the Chartboost plugin
		public static void destroy() {
			if (!checkInitialized())
				return;
			
			_plugin.Call("destroy");
			Chartboost.isInitialized = false;
			Log("Android : destroy");
		}
		
		/// Used to notify Chartboost that the Android back button has been pressed
		/// Returns true to indicate that Chartboost has handled the event and it should not be further processed
		public static bool onBackPressed() {
			bool handled = false;
			if (!checkInitialized())
				return false;
			
			handled = _plugin.Call<bool>("onBackPressed");
			Log("Android : onBackPressed");
			return handled;
		}

		public static void trackInAppGooglePlayPurchaseEvent(string title, string description, string price, string currency, string productID, string purchaseData, string purchaseSignature) {
			Log("Android: trackInAppGooglePlayPurchaseEvent");
			_plugin.Call("trackInAppGooglePlayPurchaseEvent", title,description,price,currency,productID,purchaseData,purchaseSignature);
		}
		
		public static void trackInAppAmazonStorePurchaseEvent(string title, string description, string price, string currency, string productID, string userID, string purchaseToken) {
			Log("Android: trackInAppAmazonStorePurchaseEvent");
			_plugin.Call("trackInAppAmazonStorePurchaseEvent", title,description,price,currency,productID,userID,purchaseToken);
		}
		
#endif
	}
}

