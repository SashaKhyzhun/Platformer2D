using UnityEngine;

public class MapManager : MonoBehaviour {

    public Transform mapObject;

    private Parallax parallax;
    private Transform[] segments;
    private Renderer[] renderers;
    private float[] extents;
    private float leftCamBorder;
    private float rightCamBorder;
    private float spriteCenterX;

    void Start()
    {
        int childCount = mapObject.childCount;
        segments = new Transform[childCount];
        extents = new float[childCount];
        renderers = new Renderer[childCount];

        int i = 0;
        foreach(Transform child in mapObject)
        {
            segments[i] = child;
            renderers[i] = child.GetComponent<Renderer>();
            if (renderers[i] != null) { extents[i] = renderers[i].bounds.extents.x; }
            else { extents[i] = 2; }
            i++;
        }
        parallax = Camera.main.GetComponent<Parallax>();

        foreach (Transform t in segments)
        {
            TurnOff(t);
        }
    }

    void Update()
    {
        if (parallax != null)
        {
            leftCamBorder = parallax.leftCameraBorder;
            rightCamBorder = parallax.rightCameraBorder;
        }
        else { Debug.Log("There is no Parallax script on the camera"); }

        for (int i = 0; i < segments.Length; i++)
        {
            if (renderers[i] != null) { spriteCenterX = renderers[i].bounds.center.x; }
            else { spriteCenterX = segments[i].position.x; }

            if (segments[i].gameObject.activeInHierarchy)
            {
                if (leftCamBorder >= spriteCenterX + extents[i] || rightCamBorder <= spriteCenterX - extents[i])
                {
                    TurnOff(segments[i]);
                }
            }
            else
            {
                if (leftCamBorder <= spriteCenterX + extents[i] && rightCamBorder >= spriteCenterX - extents[i])
                {
                    TurnOn(segments[i]);
                }
            }
        }
    }

    private void TurnOn(Transform obj)
    {
        if (!obj.gameObject.activeInHierarchy) { obj.gameObject.SetActive(true); }
    }

    private void TurnOff(Transform obj)
    { 
        if (obj.gameObject.activeInHierarchy) { obj.gameObject.SetActive(false); }
    }

}
