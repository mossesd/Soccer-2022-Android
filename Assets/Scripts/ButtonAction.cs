using UnityEngine;
using System.Collections;

public class ButtonAction : MonoBehaviour
{
    OtherDialoguesActive oda;

    void Start()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            oda = mainCamera.GetComponent<OtherDialoguesActive>();
            if (oda == null)
            {
                Debug.LogError("OtherDialoguesActive component not found on Main Camera.");
            }
        }
        else
        {
            Debug.LogError("Main Camera not found in the scene.");
        }
    }

    public enum Buttons
    {
        Play,
        MoreGames,
        RateUs,
        QuickMatch,
        InternationalCup,
        Back,
        Next,
        PrevTeam,
        NextTeam,
        KickOff,
        MainMenu,
        PlaySecondHalf,
        Replay,
        YES_QUIT,
        NO_QUIT,
        Resume,
        Pause,
        OwnTeam,
        None
    };

    public Buttons buttonType = Buttons.None;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backPressed();
        }
    }

    void OnMouseDown()
    {
        GetComponent<GUITexture>().texture = gameObject.GetComponent<ButtonController>().hoverTexture;
    }

    public void unityAds()
    {
        AdsManager.SharedObject().ShowUnityADS();
    }

    void OnMouseUpAsButton()
    {
        GetComponent<GUITexture>().texture = gameObject.GetComponent<ButtonController>().normalTexture;

        switch (buttonType)
        {
            case Buttons.Pause:
                if (oda != null && !oda.isOtherDialogueActive)
                {
                    AdsManager.SharedObject().ShowAdmobInterstitial();
                    AdsManager.SharedObject().ShowBanner();
                    PauseController.isPaused = true;
                }
                break;

            case Buttons.Play:
                AdsManager.SharedObject().HideBanner();
                Application.LoadLevel("GameSelectionScene");
                break;

            case Buttons.MoreGames:
                Application.OpenURL("https://play.google.com/store/apps/dev?id=9040880520724218258");
                break;

            case Buttons.RateUs:
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.vestapublicis.soccergame");
                break;

            case Buttons.QuickMatch:
                GameManager.SharedObject().isQuickMatch = true;
                GameManager.SharedObject().IsFirstHalf = true;
                PlayerPosition.PlayerTurn = true;
                Application.LoadLevel("1stTeamSelection");
                break;

            case Buttons.InternationalCup:
                GameManager.SharedObject().isQuickMatch = false;
                GameManager.SharedObject().IsFirstHalf = true;
                PlayerPosition.PlayerTurn = true;

                if (MatchesSceneController.HasPendingCup())
                    Application.LoadLevel("MatchesScene");
                else
                    Application.LoadLevel("1stTeamSelection");
                break;

            case Buttons.Back:
                backPressed();
                break;

            case Buttons.Next:
                if (Application.loadedLevelName == "1stTeamSelection")
                    Application.LoadLevel("GroupsScene");
                else if (Application.loadedLevelName == "GroupsScene" && !GameManager.SharedObject().isQuickMatch)
                    Application.LoadLevel("MatchesScene");
                else if (Application.loadedLevelName == "MatchesScene" && !GameManager.SharedObject().isQuickMatch)
                    Application.LoadLevel("KickOffScene");
                else if (Application.loadedLevelName == "2ndTeamSelection")
                    Application.LoadLevel("KickOffScene");
                break;

            case Buttons.OwnTeam:
                Application.LoadLevel("KickOffScene");
                break;

            case Buttons.PrevTeam:
                if (Application.loadedLevelName == "1stTeamSelection")
                    TeamSelectionController.teamIndex -= 1;
                else if (Application.loadedLevelName == "2ndTeamSelection")
                    TeamSelectionController2.teamIndex -= 1;
                break;

            case Buttons.NextTeam:
                if (Application.loadedLevelName == "1stTeamSelection")
                    TeamSelectionController.teamIndex += 1;
                else if (Application.loadedLevelName == "2ndTeamSelection")
                    TeamSelectionController2.teamIndex += 1;
                break;

            case Buttons.KickOff:
                PlayerPrefs.Save();
                Application.LoadLevel("MatchScene");
                break;

            case Buttons.MainMenu:
                AdsManager.SharedObject().ShowBanner();
                InitGame.matchcomplete = false;
                InitGame.halfComplete = false;
                if (AudioManager.isSFXOn)
                    AudioListener.volume = 1;
                PauseController.isPaused = false;
                Time.timeScale = 1f;
                PlayerPosition.PlayerTurn = !PlayerPosition.PlayerTurn;
                if (Application.loadedLevelName == "FinalCeleberation")
                {
                    AdsManager.SharedObject().ShowBanner();
                    Application.LoadLevel("MainMenu");
                }
                else if (GameManager.SharedObject().isQuickMatch == false && PlayerPrefs.GetInt("matchNumber") > 7)
                {
                    PlayerPrefs.SetInt("HasPendingCup", 0);
                    Application.LoadLevel("FinalCeleberation");
                }
                else if (GameManager.SharedObject().isQuickMatch == false && PlayerPrefs.GetInt("matchNumber") < 7)
                {
                    AdsManager.SharedObject().HideBanner();
                    Application.LoadLevel("MatchesScene");
                }
                else
                {
                    AdsManager.SharedObject().ShowBanner();
                    Application.LoadLevel("MainMenu");
                }
                break;

            case Buttons.PlaySecondHalf:
                InitGame.halfComplete = false;
                AdsManager.SharedObject().HideBanner();
                PlayerPosition.PlayerTurn = false;
                GameManager.SharedObject().GameTime = 0;
                GameManager.SharedObject().IsFirstHalf = false;
                if (GameManager.SharedObject().isQuickMatch)
                    Application.LoadLevel("MatchScene");
                else
                    Application.LoadLevel("KickOffScene");
                break;

            case Buttons.Replay:
                GameManager.SharedObject().IsFirstHalf = true;
                GameManager.SharedObject().IsGameReady = true;
                GameManager.SharedObject().ShowHalfTimeDialog = false;
                GameManager.SharedObject().ShowMatchEndDialog = false;
                GameManager.SharedObject().playerTeamGoals = 0;
                GameManager.SharedObject().opponentTeamGoals = 0;
                Application.LoadLevel("MatchScene");
                if (AudioManager.isSFXOn)
                    AudioListener.volume = 1;
                break;

            case Buttons.YES_QUIT:
                Application.Quit();
                break;

            case Buttons.NO_QUIT:
                if (AudioManager.isMusicOn)
                    AudioListener.volume = 1;
                GameObject.Find("QuitDialog").SetActive(false);
                break;

            case Buttons.Resume:
                AdsManager.SharedObject().HideBanner();
                PauseController.isPaused = false;
                break;
        }
    }

    void backPressed()
    {
        if (Application.loadedLevelName == "GameSelectionScene")
        {
            Application.LoadLevel("MainMenu");
            AdsManager.SharedObject().ShowBanner();
        }
        else if (Application.loadedLevelName == "1stTeamSelection")
            Application.LoadLevel("GameSelectionScene");
        else if (Application.loadedLevelName == "2ndTeamSelection")
            Application.LoadLevel("1stTeamSelection");
        else if (Application.loadedLevelName == "MatchesScene" && MatchesSceneController.HasPendingCup())
            Application.LoadLevel("GameSelectionScene");
        else if (Application.loadedLevelName == "KickOffScene" && !GameManager.SharedObject().isQuickMatch && MatchesSceneController.HasPendingCup())
            Application.LoadLevel("MatchesScene");
        else if (Application.loadedLevelName == "KickOffScene" && !GameManager.SharedObject().isQuickMatch && !MatchesSceneController.HasPendingCup())
            Application.LoadLevel("1stTeamSelection");
        else if (Application.loadedLevelName == "KickOffScene" && GameManager.SharedObject().isQuickMatch)
            Application.LoadLevel("2ndTeamSelection");
        else if (Application.loadedLevelName == "GroupsScene")
            Application.LoadLevel("CreateTeam");
        else if (Application.loadedLevelName == "CreateTeam")
            Application.LoadLevel("MainMenu");
    }

}
