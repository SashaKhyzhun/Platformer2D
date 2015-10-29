using UnityEngine;

public class OptionsManager : MonoBehaviour {

    public float screenHeight;

    private float screenWidth;
    
	void Start () {
        screenWidth = screenHeight * ((float)Screen.width / Screen.height);
        Screen.SetResolution((int)screenWidth, (int)screenHeight, true, 60);
	}
}
