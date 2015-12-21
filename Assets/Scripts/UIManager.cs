using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{    
    public GameObject FadePlane;
    public GameObject MenuLayout;
    public GameObject GameLayout;
    public GameObject PauseLayout;
    public GameObject LoadingScreen;
    public float fadeTime;
    public float timeScaleOnPause;

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
        anim = FadePlane.GetComponent<Animator>();
        anim.SetFloat("speedMultiplier", 1 / fadeTime);
    }

    void Update()
    {
        if(pc != null)
        {
            if (pc.finished) { if (pc.canLoad) { StartGame(); pc.canLoad = false; } }
            if (pc.startFade) { anim.SetBool("Fade", true); }
            if (pc.startReturn) { anim.SetBool("Fade", false); }
        }
    }

    public void Restart()
    {
        StartCoroutine(WaitForLoad(Application.loadedLevel));
    }

    public void StartGame()
    {
        StartCoroutine(WaitForLoad(Application.loadedLevel + 1));
    }

    public void Pause()
    {
        if (GameLayout.activeInHierarchy) { GameLayout.SetActive(false); }
        if (!PauseLayout.activeInHierarchy) { PauseLayout.SetActive(true); }
        Time.timeScale = timeScaleOnPause;
    }

    public void Unpause()
    {
        if (!GameLayout.activeInHierarchy) { GameLayout.SetActive(true); }
        if (PauseLayout.activeInHierarchy) { PauseLayout.SetActive(false); }
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        StartCoroutine(WaitForLoad(0));

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
                if (GameLayout.activeInHierarchy) { GameLayout.SetActive(false); }
                if (LoadingScreen.activeInHierarchy) { LoadingScreen.SetActive(false); }
                if (!MenuLayout.activeInHierarchy) { MenuLayout.SetActive(true); }
                if (PauseLayout.activeInHierarchy) { PauseLayout.SetActive(false); }
                break;
            default:
                if (!GameLayout.activeInHierarchy) { GameLayout.SetActive(true); }
                if (MenuLayout.activeInHierarchy) { MenuLayout.SetActive(false); }
                if (LoadingScreen.activeInHierarchy) { LoadingScreen.SetActive(false); }
                if (PauseLayout.activeInHierarchy) { PauseLayout.SetActive(false); }
                pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                break;
        }
    }
}
