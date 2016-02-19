using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsManager : MonoBehaviour {

    public float screenHeight;
    public bool doCap;
    public bool doChangeResolution;
    public int frameRate;

    private CanvasScaler cs;
    private float screenWidth;
    
	void Start () {
        cs = GetComponent<UIManager>().UI.GetComponent<CanvasScaler>();
        if (doChangeResolution)
        {            
			StartCoroutine (ChangeREsolution ());
        }
        if (doCap) { Application.targetFrameRate = frameRate; }
    }

	IEnumerator ChangeREsolution()
	{
		Vector2 prevRes = new Vector2 (Screen.width, Screen.height);
		screenWidth = screenHeight * ((float)Screen.width / Screen.height);
		Screen.SetResolution((int)screenWidth, (int)screenHeight, true, 0);
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		Vector2 resDelta = new Vector2(Screen.width - prevRes.x, Screen.height - prevRes.y);
		cs.referenceResolution -= resDelta;
	}
}
