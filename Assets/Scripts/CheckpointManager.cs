using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour {

    public Transform checkpointObject;

    private Transform[] checkpoints;
    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        checkpoints = new Transform[checkpointObject.childCount];
        int i = 0;
        foreach (Transform child in checkpointObject)
        {
            checkpoints[i] = child;
            i++;
        }
    }

	
	// Update is called once per frame
	void Update () {
        float camPositionX = Camera.main.transform.position.x;

        if (!playerController.alive)
        {
            for (int i = 1; i < checkpoints.Length; i++)
            {
                if (camPositionX >= checkpoints[i - 1].position.x && camPositionX <= checkpoints[i].position.x)
                {
                    playerController.Respawn(checkpoints[i - 1].position);
                }

            }
        }
	}



}
