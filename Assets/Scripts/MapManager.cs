﻿using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

    public Transform mapObject;
    public Transform backgroundObject;
    public float offset;

    private Transform[] mapSegments;
    private Transform[] backgroundSegments;
    private float[] mapExtents;
    private float[] bgExtents;
    private float camPosX;
    private float camExtent;
    private float rightCameraBorder;
    private float leftCameraBorder;
    private float rightSpriteBorder;
    private float leftSpriteBorder;
    //private int bgCount;

    void Start () {
        offset = Mathf.Abs(offset);
        mapSegments = new Transform[mapObject.childCount];
        backgroundSegments = new Transform[backgroundObject.childCount];
        //bgCount = backgroundSegments.Length / 2;
        mapExtents = new float[mapSegments.Length];
        bgExtents = new float[backgroundSegments.Length];
        Camera _camera = Camera.main;
        camExtent = _camera.orthographicSize * _camera.aspect;

        int i = 0;
        foreach (Transform child in mapObject)
        {
            mapSegments[i] = child;
            mapExtents[i] = mapSegments[i].GetComponent<Renderer>().bounds.extents.x;
            mapSegments[i].gameObject.SetActive(false);
            i++;
        }

        i = 0;

        foreach(Transform child in backgroundObject)
        {
            backgroundSegments[i] = child;
            bgExtents[i] = backgroundSegments[i].GetComponent<Renderer>().bounds.extents.x;
            i++;
        }
    }

    void Update () {
        camPosX = Camera.main.transform.position.x;
        rightCameraBorder = camPosX + camExtent + offset;
        leftCameraBorder = camPosX - camExtent - offset;

        ManageMap();
        ManageBackground();
	}

    void ManageMap()
    {
        for (int i = 0; i < mapExtents.Length; i++)
        {
            rightSpriteBorder = mapSegments[i].position.x + mapExtents[i];
            leftSpriteBorder = mapSegments[i].position.x - mapExtents[i];

            if (leftCameraBorder >= leftSpriteBorder && rightCameraBorder <= rightSpriteBorder)
            {
                if (!mapSegments[i].gameObject.activeInHierarchy)
                {
                    mapSegments[i].gameObject.SetActive(true);
                }
            }
            if (leftCameraBorder >= rightSpriteBorder)
            {
                if (mapSegments[i].gameObject.activeInHierarchy)
                {
                    mapSegments[i].gameObject.SetActive(false);
                }
            }
            else if (rightCameraBorder >= leftSpriteBorder)
            {
                if (!mapSegments[i].gameObject.activeInHierarchy)
                {
                    mapSegments[i].gameObject.SetActive(true);
                }
            }
        }
    }

    void ManageBackground()
    {
        for (int i = 0; i < backgroundSegments.Length; i++)
        {
            rightSpriteBorder = backgroundSegments[i].position.x + bgExtents[i];

            if (leftCameraBorder >= rightSpriteBorder)
            {
                backgroundSegments[i].position = backgroundSegments[i].position + new Vector3(4 * bgExtents[i], 0);
            }
        }
    }
}