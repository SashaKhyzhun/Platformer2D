using UnityEngine;
using System.Collections;

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
    public float timeScaleOnPause;

    private ProgressManager pm;
    private PlayerController pc;
    private Animator anim;
    private WaitForSeconds halfOfTime;
    private WaitForSeconds time;
    private WaitForEndOfFrame wfeof;

    void Start()
    {
        halfOfTime = new WaitForSeconds(fadeTime / 2);
        time = new WaitForSeconds(fadeTime);
        wfeof = new WaitForEndOfFrame();
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
                    LoadLevel(Application.loadedLevel + 1);
                    pc.canLoad = false;
                }
            }
            if (pc.startFade) { anim.SetBool("Fade", true); }
            if (pc.startReturn) { anim.SetBool("Fade", false); }
        }
    }

    public void LoadStats(Transform levelsLayout)
    {
        pm.LoadStats(levelsLayout);
    }

    public void Restart()
    {
        StartCoroutine(WaitForLoad(Application.loadedLevel));
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(WaitForLoad(index));
    }

    public void Pause()
    {
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
        yield return halfOfTime;
        Application.Quit();
    }

    IEnumerator WaitForLoad(int level)
    {
        Time.timeScale = 1;
        if (level > 1)
        {
            int currSeason = (level - 2) / pm.levelCount;
            int currLevel = (level - 2) - (currSeason * 12);
            pm.SaveStats(currSeason, currLevel, pc.time, 1);
        }
        anim.SetBool("Fade", true);
        yield return halfOfTime;
        if (!LoadingScreen.activeInHierarchy) { LoadingScreen.SetActive(true); }
        anim.SetBool("Fade", false);
        yield return time;

        AsyncOperation async = Application.LoadLevelAsync(level);
        async.allowSceneActivation = false;
        while (async.progress < 0.9f) { yield return wfeof; }

        anim.SetBool("Fade", true);
        yield return halfOfTime;
        async.allowSceneActivation = true;
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
