using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public Transform checkpointObject;

    private Transform[] checkpoints;
    private Transform playerTransform;
    private PlayerController playerController;

	void Start () {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        checkpoints = new Transform[checkpointObject.childCount];
        int i = 0;
        foreach (Transform child in checkpointObject)
        {
            checkpoints[i] = child;
            i++;
        }
        playerController.checkpoints = checkpoints;
        playerTransform = playerController.gameObject.transform;
    }
    
	void Update () {
        float playerPositionX = playerTransform.position.x;
        for (int i = 0; i < checkpoints.Length - 1; i++)
        {
            if (playerPositionX >= checkpoints[i].position.x)
            {
                if (playerController.checkpointNumber < i)
                {
                    playerController.checkpointNumber = i;
                }                    
            }
        }
    }



}
