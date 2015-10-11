using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

    public GameObject[] mapSegments;
    public GameObject[] backgroundPrefab;
    public float offset;

    private Vector3 camPos;
    private float[] extents;
    private float camExtent;

	// Use this for initialization
	void Start () {
        offset = Mathf.Abs(offset);
        extents = new float[mapSegments.Length];
        Camera _camera = Camera.main;
        camExtent = _camera.orthographicSize * _camera.aspect;
        for (int i = 0; i < mapSegments.Length; i++)
        {
            extents[i] = mapSegments[i].GetComponent<Renderer>().bounds.extents.x;
            //Debug.Log(extents[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        camPos = Camera.main.transform.position;
        for (int i = 1; i < extents.Length; i++) {
            if (camPos.x + camExtent + offset >= mapSegments[i - 1].transform.position.x + extents[i])
            {
                if (!mapSegments[i].activeInHierarchy)
                {
                    mapSegments[i].SetActive(true);
                }
            }
            if (camPos.x - camExtent - offset >= mapSegments[i].transform.position.x - extents[i])
            {
                if (mapSegments[i - 1].activeInHierarchy)
                {
                    mapSegments[i - 1].SetActive(false);
                }
            }
        }

	}
}
