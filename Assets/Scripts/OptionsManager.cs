using UnityEngine;
using System.Collections;

public class OptionsManager : MonoBehaviour {

    public float screenHeight;
    public bool doCap;
    public bool doChangeResolution;
    public int frameRate;
    public GameObject FadePlane;

    private float screenWidth;
    private Animator anim;
    
	void Start () {
        anim = FadePlane.GetComponent<Animator>();
        if (doChangeResolution)
        {
            screenWidth = screenHeight * ((float)Screen.width / Screen.height);
            Screen.SetResolution((int)screenWidth, (int)screenHeight, true, 60);
        }
        if (doCap) { Application.targetFrameRate = frameRate; }
	}

    public void RestartButton()
    {
        StartCoroutine(WaitForRestart());        
    }

    IEnumerator WaitForRestart()
    {
        anim.SetBool("Fade", true);
        yield return new WaitForSeconds(1);
        Application.LoadLevel(Application.loadedLevel);
    }
}
