using UnityEngine;
using UnityEngine.UI;

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
            GetComponent<UIManager>().UI.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        }
        if (doCap) { Application.targetFrameRate = frameRate; }
	}

    
}
