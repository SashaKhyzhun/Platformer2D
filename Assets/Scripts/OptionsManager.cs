using UnityEngine;
using System.Collections;

public class OptionsManager : MonoBehaviour {

    public float screenHeight;
    public bool doCap;
    public bool doChangeResolution;
    public int frameRate;

    private float screenWidth;
    
	void Start () {
        if (doChangeResolution)
        {
            screenWidth = screenHeight * ((float)Screen.width / Screen.height);
            Screen.SetResolution((int)screenWidth, (int)screenHeight, true, 60);
        }
        if (doCap) { Application.targetFrameRate = frameRate; }
	}

    
}
