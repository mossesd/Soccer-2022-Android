using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource footballKicked;
    public AudioSource pauseWhistle;
    public AudioSource resumeWhistle;
    public AudioSource restartWhistle;
    public AudioSource onGoal;
    public AudioSource onMiss;
    public AudioSource onMatchStart;

    public static AudioManager Instance { get; private set; }

    public static bool isMusicOn = false;
    public static bool isSFXOn = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayerPrefs.DeleteAll();
        AdsManager manager = AdsManager.SharedObject();
        manager.DisableTestMode();
        manager.DisableLogger();
        manager.showBannerAtTop = true;

        manager.admobAccountID = "ca-app-pub-2330175674430713~6303947410";
        manager.admobBannerID = "ca-app-pub-2330175674430713/1051620731";
        manager.admobIntertitialID = "ca-app-pub-2330175674430713/7425457395";

        manager.ChartboostAppID = "";
        manager.ChartboostAppSignature = "";
        manager.StartAppDeveloperID = "";
        manager.StartAppID = "";
        manager.LeadboltApiKey = "";
        manager.HeyzapPublisherID = "";
        manager.UnityAdsId = "";
        manager.Initialize();
        manager.ShowBanner();

        if (PlayerPrefs.GetInt("jtsounds") == 1)
        {
            isMusicOn = PlayerPrefs.GetInt("isMusicOn") == 1;
            isSFXOn = PlayerPrefs.GetInt("isSFXOn") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("jtsounds", 1);
            PlayerPrefs.SetInt("isMusicOn", 1);
            PlayerPrefs.SetInt("isSFXOn", 1);
            PlayerPrefs.Save();

            isMusicOn = true;
            isSFXOn = true;
        }

        Invoke("LoadMainMenu", 0.5f);
    }

    void LoadMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("isMusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.SetInt("isSFXOn", isSFXOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void SetMusicVolume(float volume)
    {
        if (Instance != null)
        {
            Instance.backgroundMusic.volume = volume;
        }
    }

    public static void SetSFXVolume(float volume)
    {
        if (Instance != null)
        {
            Instance.footballKicked.volume = volume;
            Instance.pauseWhistle.volume = volume;
            Instance.resumeWhistle.volume = volume;
            Instance.restartWhistle.volume = volume;
            Instance.onGoal.volume = volume;
            Instance.onMiss.volume = volume;
            Instance.onMatchStart.volume = volume;
        }
    }

    public static void PlayKickSound() { if (Instance != null && isSFXOn) Instance.footballKicked.Play(); }
    public static void PlayResumeWhistle() { if (Instance != null && isSFXOn) Instance.resumeWhistle.Play(); }
    public static void PlayRestartWhistle() { if (Instance != null && isSFXOn) Instance.restartWhistle.Play(); }
    public static void PlayPauseWhistle() { if (Instance != null && isSFXOn) Instance.pauseWhistle.Play(); }
    public static void PlayOnGoalRoar() { if (Instance != null && isSFXOn) Instance.onGoal.Play(); }
    public static void PlayAudienceSound() { if (Instance != null && isSFXOn) Instance.onMatchStart.Play(); }
    public static void StopAudienceSound() { if (Instance != null) Instance.onMatchStart.Stop(); }
    public static void PlayOnMissRoar() { if (Instance != null && isSFXOn) Instance.onMiss.Play(); }
    public static void PlayBackgroundMusic() { if (Instance != null && isMusicOn && !Instance.backgroundMusic.isPlaying) Instance.backgroundMusic.Play(); }
    public static void StopBackgroundMusic() { if (Instance != null) Instance.backgroundMusic.Stop(); }
}
