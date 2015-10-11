using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

    public GameObject[] mapSegments;
    public GameObject[] backgroundSegments;
    public float offset;

    private Vector3 camPos;
    private float[] mapExtents;
    private float[] bgExtents;
    private float camExtent;

	void Start () {
        offset = Mathf.Abs(offset);
        mapExtents = new float[mapSegments.Length];
        bgExtents = new float[backgroundSegments.Length];
        Camera _camera = Camera.main;
        camExtent = _camera.orthographicSize * _camera.aspect;
        for (int i = 0; i < mapSegments.Length; i++)
        {
            mapExtents[i] = mapSegments[i].GetComponent<Renderer>().bounds.extents.x;
        }
        for (int i = 0; i < backgroundSegments.Length; i++)
        {
            bgExtents[i] = backgroundSegments[i].GetComponent<Renderer>().bounds.extents.x;
            Debug.Log(bgExtents[i]);
        }
    }
	
	void Update () {
        ManageMap();
        ManageBackground();
	}

    void ManageMap()
    {
        camPos = Camera.main.transform.position;
        for (int i = 1; i < mapExtents.Length; i++)
        {
            if (camPos.x + camExtent + offset >= mapSegments[i - 1].transform.position.x + mapExtents[i])
            {
                if (!mapSegments[i].activeInHierarchy)
                {
                    mapSegments[i].SetActive(true);
                }
            }
            if (camPos.x - camExtent - offset >= mapSegments[i].transform.position.x - mapExtents[i])
            {
                if (mapSegments[i - 1].activeInHierarchy)
                {
                    mapSegments[i - 1].SetActive(false);
                }
            }
        }
    }

    void ManageBackground()
    {
        /*
        camPos = Camera.main.transform.position;
        for (int i = 1; i < backgroundPrefab.Length; i++)
        {
            if (camPos.x + camExtent + offset >= mapSegments[i - 1].transform.position.x + mapExtents[i])
            {
                if (!mapSegments[i].activeInHierarchy)
                {
                    mapSegments[i].SetActive(true);
                }
            }
            if (camPos.x - camExtent - offset >= mapSegments[i].transform.position.x - mapExtents[i])
            {
                if (mapSegments[i - 1].activeInHierarchy)
                {
                    mapSegments[i - 1].SetActive(false);
                }
            }
        }
        */
    }
}
