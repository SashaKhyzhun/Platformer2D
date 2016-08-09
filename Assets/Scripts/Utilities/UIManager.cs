using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

//ads
//using GoogleMobileAds.Api;
//ads

public class UIManager : MonoBehaviour
{
    public GameObject UI;
    public GameObject FadePlane;
    public GameObject Background;
    public GameObject MenuLayout;
    public GameObject GameLayout;
    public GameObject PauseLayout;
    public GameObject SeasonsMenuLayout;
    public GameObject[] LevelsMenuLayout;
    public GameObject LoadingScreen;
    public GameObject CongratulationsLayout;
    public Animator splashScreen;
    public float fadeTime;
    public float splashTime;
    public float levelEndWaitTime;
    public float timeScaleOnPause;
    //public bool allowAds = true;

    private ProgressManager pm;
    private PlayerController pc;
    private ShareInfo shI;
    private Animator anim;
    private WaitForSeconds halfFadeTimeWFS;
    private WaitForSeconds fadeTimeWFS;
    private WaitForEndOfFrame wfeof;
    private WaitForSeconds levelEndWaitTimeWFS;
    private bool completedLevel = false;
    private bool completedSeason = false;
    private bool completedGame = false;
    private bool backToMenu = false;
    private bool restart = false;

    //	private InterstitialAd interstitial;

    void Start()
    {
        shI = GetComponent<ShareInfo>();
        shI.text = "Look, there's an amazing game! You can play it too!\n https://play.google.com/store/apps/details?id=" + Application.bundleIdentifier;
        halfFadeTimeWFS = new WaitForSeconds(fadeTime / 2);
        fadeTimeWFS = new WaitForSeconds(fadeTime);
        wfeof = new WaitForEndOfFrame();
        levelEndWaitTimeWFS = new WaitForSeconds(levelEndWaitTime);
        pm = gameObject.GetComponent<ProgressManager>();
        anim = FadePlane.GetComponent<Animator>();
        anim.SetFloat("speedMultiplier", 1 / fadeTime);
        StartCoroutine(WaitForSplash());
        if (GameObject.FindGameObjectsWithTag(UI.tag).Length > 1)
        {
            Destroy(GameObject.FindGameObjectsWithTag(UI.tag)[1]);
        }
        if (GameObject.FindGameObjectsWithTag("es").Length > 1)
        {
            Destroy(GameObject.FindGameObjectsWithTag("es")[1]);
        }
        if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (pc != null)
        {
            if (pc.finished)
            {
                if (pc.canLoad)
                {
                    pc.time = Time.time - pc.startTime;
                    StartCoroutine(WaitForConfirm(levelEndWaitTime));                    
                    completedLevel = true;
                    //LoadLevel(Application.loadedLevel + 1);
                    pc.canLoad = false;
                }
            }
            if (pc.startFade)
            {
                anim.SetBool("Fade", true);
            }
            if (pc.startReturn)
            {
                anim.SetBool("Fade", false);
            }
        }
    }

    public void LoadStats(Transform levelsLayout)
    {
        pm.LoadStats(levelsLayout);
    }

    public void LoadLevelEndStats(Transform layout)
    {
        int level = Application.loadedLevel;
        int currSeason = (level - 1) / pm.levelCount;
        int currLevel = (level - 1) - (currSeason * 12);
        TimeSpan time = Level.FromFloatToTimeSpan(pc.time);
        Transform tile = layout.FindChild("LevelEndTile");
        Transform stats = tile.FindChild("Stats");
        tile.FindChild("LevelEndName").GetComponent<Text>().text = string.Format("SEASON {0} | LEVEL {1}", currSeason + 1, currLevel + 1);
        string timeString = string.Format("{0:0}:{1:00}.{2:00}", time.Minutes, time.Seconds, time.Milliseconds / 10);
        stats.FindChild("Time").GetComponent<Text>().text = timeString;
        stats.FindChild("Deaths").GetComponent<Text>().text = pc.deaths + "";

        string deathsString = "";
        switch (pc.deaths)
        {
            case 0:
                deathsString = "out any deaths at all";
                break;
            case 1:
                deathsString = " only one death";
                break;
            default:
                deathsString = " " + pc.deaths + " deaths";
                break;
        }

        shI.text = string.Format("Look, I've finished level {0}.{1} in {2}m. with{3}!\nYou can try it too! {4}", currSeason + 1, currLevel + 1, timeString, deathsString, "https://play.google.com/store/apps/details?id=" + Application.bundleIdentifier);
    }

    public void Restart()
    {
        //StartCoroutine(WaitForLoad(Application.loadedLevel));
        if (completedLevel)
        {
            backToMenu = true;
            LoadLevel(Application.loadedLevel + 1);
            backToMenu = false;
            completedLevel = false;
        }
        restart = true;
        LoadLevel(Application.loadedLevel + 1);
        restart = false;
    }

    public void NextLevel()
    {
        if ((Application.loadedLevel) % pm.levelCount == 0)
        {
            BackToMenu();
            completedSeason = true;
        }
        else
        {
            LoadLevel(Application.loadedLevel + 1);
        }
    }

    public void LoadLevel(int index) // needs next level index;
    {
        if (index > 0)
        {
            int currSeason = (index - 1) / pm.levelCount;
            int nextLevel = (index - 1) - (currSeason * 12); //level that you want to load
            if (currSeason >= pm.seasonCount)
            {
                completedGame = true;
                if (backToMenu)
                {
                    pm.SaveStats(currSeason - 1, pm.levelCount - 1, pc.time, pc.deaths);
                }
                else if (restart)
                {
                    pm.SaveStats(currSeason - 1, pm.levelCount - 1, pc.time, pc.deaths);
                    StartCoroutine(WaitForLoad(index - 1));
                }
                return;
            }
            if (pc != null)
            {
                if (!restart)
                {
                    if (currSeason > 0 && nextLevel == 0)
                    {
                        pm.SaveStats(currSeason - 1, pm.levelCount - 1, pc.time, pc.deaths);
                    }
                    else
                    {
                        pm.SaveStats(currSeason, nextLevel - 1, pc.time, pc.deaths);
                    }
                }
            }

            if (nextLevel < Season.levelCount)
            {

                if (!restart)
                {
                    if (Game.current.seasons[currSeason].available && Game.current.seasons[currSeason].levels[nextLevel].available)
                    {
                        if (!backToMenu)
                        {
                            StartCoroutine(WaitForLoad(index));
                        }
                    }
                }
                else
                {
                    StartCoroutine(WaitForLoad(index - 1));
                }
            }
            else if (currSeason + 1 < Game.seasonCount)
            {
                if (Game.current.seasons[currSeason + 1].available && Game.current.seasons[currSeason].levels[nextLevel].available)
                {
                    //StartCoroutine(WaitForLoad(index));
                    if (!backToMenu)
                    {
                        if (!restart)
                        {
                            StartCoroutine(WaitForLoad(index));
                        }
                        else
                        {
                            StartCoroutine(WaitForLoad(index - 1));
                        }
                    }
                }
            }
            else
            {
                Debug.Log("scene index is over the limit");
            }
        }
        else
        {
            StartCoroutine(WaitForLoad(index));
        }
        completedLevel = false;
    }

    public void Pause()
    {
        int level = Application.loadedLevel;
        int currSeason = (level - 1) / pm.levelCount;
        int currLevel = (level - 1) - (currSeason * 12);
        PauseLayout.transform.FindChild("PauseTile").FindChild("PauseLevelName").GetComponent<Text>().text = string.Format("SEASON {0} | LEVEL {1}", currSeason + 1, currLevel + 1);
        TurnLayoutOn(PauseLayout);
        TurnLayoutOff(GameLayout);
        Time.timeScale = timeScaleOnPause;
    }

    public void Unpause()
    {
        TurnLayoutOn(GameLayout);
        TurnLayoutOff(PauseLayout);
        Time.timeScale = 1;
    }

    public void TurnLayoutOn(GameObject layout)
    {
        if (!layout.activeInHierarchy)
        {
            layout.SetActive(true);
        }
    }

    public void TurnLayoutOff(GameObject layout)
    {
        if (layout.activeInHierarchy)
        {
            layout.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        if (completedLevel)
        {
            backToMenu = true;
            LoadLevel(Application.loadedLevel + 1);
            backToMenu = false;
            completedLevel = false;
        }
        LoadLevel(0);
    }

    public void ExitGame()
    {
        StartCoroutine(WaitForExit());
    }

    IEnumerator WaitForSplash()
    {
        splashScreen.SetTrigger("Play");
        yield return new WaitForSeconds(splashTime);
        anim.SetBool("Fade", false);
    }

    IEnumerator WaitForExit()
    {
        Time.timeScale = 1;
        anim.SetBool("Fade", true);
        yield return halfFadeTimeWFS;
        Application.Quit();
    }

    //	private InterstitialAd RequestInterstitial()
    //	{
    //		#if UNITY_ANDROID
    //		string adUnitId = "ca-app-pub-8810835231774698/6782405565";
    //		#elif UNITY_IPHONE
    //		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
    //		#else
    //		string adUnitId = "unexpected_platform";
    //		#endif
    //
    //		// Initialize an InterstitialAd.
    //    	InterstitialAd interstitial = new InterstitialAd(adUnitId);
    //        // Create an empty ad request.
    //
    //        //for release
    //        //AdRequest request = new AdRequest.Builder().Build();
    //
    //        AdRequest request = new AdRequest.Builder()
    //            //.AddTestDevice(AdRequest.TestDeviceSimulator)
    //            //.AddTestDevice("94B6F3B031BFB085513365B02FBBB6DE")
    //            //.AddTestDevice("09970ED4E5B9A61393ED38E4E163783C")
    //            .Build();
    //
    //        // Load the interstitial with the request.
    //        interstitial.LoadAd (request);
    //		return interstitial;
    //	}

    IEnumerator WaitForConfirm(float seconds)
    {
        Transform LevelEndLayout = UI.transform.FindChild("LevelEndLayout");
        yield return levelEndWaitTimeWFS;
//        if (allowAds) { if (interstitial.IsLoaded()) { interstitial.Show(); } }
        TurnLayoutOn(LevelEndLayout.gameObject);
        LoadLevelEndStats(LevelEndLayout);
        GetComponent<AudioManager>().AudioPause();
    }

    IEnumerator WaitForLoad(int level)
    {
        Time.timeScale = 1;        
        anim.SetBool("Fade", true);
        yield return halfFadeTimeWFS;
        TurnLayoutOn(LoadingScreen);
        anim.SetBool("Fade", false);
        yield return fadeTimeWFS;

        AsyncOperation async = Application.LoadLevelAsync(level);
        async.allowSceneActivation = false;
        while (async.progress < 0.9f)
        {
            yield return wfeof;
        }

        anim.SetBool("Fade", true);
        yield return halfFadeTimeWFS;
        async.allowSceneActivation = true;
        Resources.UnloadUnusedAssets();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Resources.UnloadUnusedAssets();
        anim.SetBool("Fade", false);
    }

    void OnLevelWasLoaded(int level)
    {
        //if (allowAds) { interstitial = RequestInterstitial(); }
        if (shI != null)
        {
            shI.text = "Look, there's an amazing game called Flying Adventures! You can play it too!\n https://play.google.com/store/apps/details?id=" + Application.bundleIdentifier;
        }
        switch (level)
        {
            case 0:
                TurnLayoutOff(GameLayout);
                TurnLayoutOff(LoadingScreen);
                if (completedSeason)
                {
                    TurnLayoutOff(MenuLayout);
                    if (completedGame)
                    {
                        if (shI != null)
                        {
                            shI.text = "Look, I've completed the whole game! You should try too\n https://play.google.com/store/apps/details?id=" + Application.bundleIdentifier;
                        }
                        TurnLayoutOn(CongratulationsLayout);
                    }
                    else
                    {
                        TurnLayoutOn(SeasonsMenuLayout);
                        LoadStats(SeasonsMenuLayout.transform);
                    }
                    completedSeason = false;
                }
                else
                {
                    TurnLayoutOn(MenuLayout);
                    TurnLayoutOff(SeasonsMenuLayout);
                }
                TurnLayoutOn(Background);
                TurnLayoutOff(PauseLayout);
                break;
            default:
                TurnLayoutOn(GameLayout);
                TurnLayoutOff(MenuLayout);
                TurnLayoutOff(Background);
                TurnLayoutOff(LoadingScreen);
                TurnLayoutOff(PauseLayout);
                TurnLayoutOff(SeasonsMenuLayout);
                pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                break;
        }
    }
}
