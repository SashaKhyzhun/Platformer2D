using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject FadePlane;
    public GameObject MenuLayout;
    public GameObject GameLayout;
    public float fadeTime;

    private Animator anim;

    void Start()
    {
        anim = FadePlane.GetComponent<Animator>();
        anim.SetFloat("speedMultiplier", 1 / fadeTime);
    } 

    public void Restart()
    {
        StartCoroutine(WaitForRestart(Application.loadedLevel));
    }

    public void StartGame()
    {
        StartCoroutine(WaitForRestart(Application.loadedLevel + 1));
    }

    IEnumerator WaitForRestart(int level)
    {
        anim.SetBool("Fade", true);
        yield return new WaitForSeconds(fadeTime / 2);
        Application.LoadLevelAsync(level);
        anim.SetBool("Fade", false);
    }

    void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                if (GameLayout.activeInHierarchy) { GameLayout.SetActive(false); }
                if (!MenuLayout.activeInHierarchy) { MenuLayout.SetActive(true); }
                break;
            case 1:
                if (!GameLayout.activeInHierarchy) { GameLayout.SetActive(true); }
                if (MenuLayout.activeInHierarchy) { MenuLayout.SetActive(false); }
                break;
        }
    }
}
