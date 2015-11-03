using UnityEngine;

public class OptionsManager : MonoBehaviour {

    public float screenHeight;
    public bool doCap;
    public int frameRate;

    private float screenWidth;
    
	void Start () {
        screenWidth = screenHeight * ((float)Screen.width / Screen.height);
        Screen.SetResolution((int)screenWidth, (int)screenHeight, true, 60);
        if (doCap) { Application.targetFrameRate = frameRate; }
	}
}
