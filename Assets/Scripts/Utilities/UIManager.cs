﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject UI;
    public GameObject FadePlane;
    public GameObject MenuLayout;
    public GameObject GameLayout;
    public GameObject PauseLayout;
    public GameObject SeasonsMenuLayout;
    public GameObject[] LevelsMenuLayout;
    public GameObject LoadingScreen;
    public float fadeTime;
    public float levelEndWaitTime;
    public float timeScaleOnPause;

    private ProgressManager pm;
    private PlayerController pc;
    private Animator anim;
    private WaitForSeconds halfFadeTimeWFS;
    private WaitForSeconds fadeTimeWFS;
    private WaitForEndOfFrame wfeof;
    private WaitForSeconds levelEndWaitTimeWFS;
    private bool completedLevel = false;
    private bool backToMenu = false;

    void Start()
    {
        halfFadeTimeWFS = new WaitForSeconds(fadeTime / 2);
        fadeTimeWFS = new WaitForSeconds(fadeTime);
        wfeof = new WaitForEndOfFrame();
        levelEndWaitTimeWFS = new WaitForSeconds(levelEndWaitTime);
        pm = gameObject.GetComponent<ProgressManager>();
        anim = FadePlane.GetComponent<Animator>();
        anim.SetFloat("speedMultiplier", 1 / fadeTime);
        if (GameObject.FindGameObjectsWithTag(UI.tag).Length > 1) { Destroy(GameObject.FindGameObjectsWithTag(UI.tag)[1]); }
        if (GameObject.FindGameObjectsWithTag("es").Length > 1) { Destroy(GameObject.FindGameObjectsWithTag("es")[1]); }
        if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1) { Destroy(gameObject); }
    }

    void Update()
    {
        if(pc != null)
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
        stats.FindChild("Time").GetComponent<Text>().text = string.Format("{0:0}:{1:00}.{2:00}", time.Minutes, time.Seconds, time.Milliseconds / 10);
        stats.FindChild("Deaths").GetComponent<Text>().text = pc.deaths + "";
    }

    public void Restart()
    {
        StartCoroutine(WaitForLoad(Application.loadedLevel));
    }

    public void NextLevel()
    {
        LoadLevel(Application.loadedLevel + 1);
    }

    public void LoadLevel(int index)
    {
        if (index > 0)
        {
            int currSeason = (index - 1) / pm.levelCount;
            int nextLevel = (index - 1) - (currSeason * 12); //level that you want to load
            if (pc != null)
            {
                if (currSeason > 0 && nextLevel == 0) { pm.SaveStats(currSeason - 1, pm.levelCount - 1, pc.time, pc.deaths); }
                else { pm.SaveStats(currSeason, nextLevel - 1, pc.time, pc.deaths); }
            }

            if (nextLevel < Season.levelCount)
            {
                if (Game.current.seasons[currSeason].available && Game.current.seasons[currSeason].levels[nextLevel].available)
                {
                    if (!backToMenu)
                    {
                        StartCoroutine(WaitForLoad(index));
                    }
                }
            }
            else if (currSeason + 1 < Game.seasonCount)
            {
                if (Game.current.seasons[currSeason + 1].available && Game.current.seasons[currSeason].levels[nextLevel].available)
                {
                    StartCoroutine(WaitForLoad(index));
                }
            }
            else { Debug.Log("scene index is over the limit"); }
        }
        else { StartCoroutine(WaitForLoad(index)); }
    }

    public void Pause()
    {
        int level = Application.loadedLevel;
        int currSeason = (level - 1) / pm.levelCount;
        int currLevel = (level - 1) - (currSeason * 12);
        PauseLayout.transform.FindChild("PauseLevelName").GetComponent<Text>().text = string.Format("SEASON {0}\nLEVEL {1}", currSeason + 1, currLevel + 1);
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
        if (!layout.activeInHierarchy) { layout.SetActive(true); }
    }

    public void TurnLayoutOff(GameObject layout)
    {
        if (layout.activeInHierarchy) { layout.SetActive(false); }
    }

    public void BackToMenu()
    {
        if (completedLevel)
        {
            backToMenu = true;
            LoadLevel(Application.loadedLevel  + 1);
            backToMenu = false;
            completedLevel = false;
        }
        LoadLevel(0);
    }

    public void ExitGame()
    {
        StartCoroutine(WaitForExit());
    }

    IEnumerator WaitForExit()
    {
        Time.timeScale = 1;
        anim.SetBool("Fade", true);
        yield return halfFadeTimeWFS;
        Application.Quit();
    }

    IEnumerator WaitForConfirm(float seconds)
    {

        Transform LevelEndLayout = UI.transform.FindChild("LevelEndLayout");
        yield return levelEndWaitTimeWFS;
        TurnLayoutOn(LevelEndLayout.gameObject);
        LoadLevelEndStats(LevelEndLayout);
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
        while (async.progress < 0.9f) { yield return wfeof; }

        anim.SetBool("Fade", true);
        yield return halfFadeTimeWFS;
        async.allowSceneActivation = true;
        GC.Collect();
        Resources.UnloadUnusedAssets();
        anim.SetBool("Fade", false);
    }

    void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                TurnLayoutOff(GameLayout);
                TurnLayoutOff(LoadingScreen);
                TurnLayoutOn(MenuLayout);
                TurnLayoutOff(PauseLayout);
                TurnLayoutOff(SeasonsMenuLayout);
                break;
            default:
                TurnLayoutOn(GameLayout);
                TurnLayoutOff(MenuLayout);
                TurnLayoutOff(LoadingScreen);
                TurnLayoutOff(PauseLayout);
                TurnLayoutOff(SeasonsMenuLayout);        
                pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                break;
        }
    }
}