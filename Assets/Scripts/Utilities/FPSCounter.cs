using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    private Text text;
    private float deltaTime = 0;
    private float fps;
    private string fpsText;

	// Use this for initialization
	void Start ()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        if (!Debug.isDebugBuild) { gameObject.SetActive(false); }
#endif
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        fpsText = string.Format("{0:0.} fps", fps);
        text.text = fpsText;
    }
}
