using UnityEngine;

public class OptionsManager : MonoBehaviour {

    public float screenHeight;

    private float screenWidth;
    
	void Start () {
        screenWidth = screenHeight * (Screen.width / Screen.height);
        Screen.SetResolution((int)screenHeight, (int)screenWidth, true, 60);
	}
}
