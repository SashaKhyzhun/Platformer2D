using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    public Transform checkpointObject;

    private Transform[] checkpoints;
    private Transform playerTransform;
    private PlayerController playerController;

    public  int currentIndex { get; set; }
    public bool revert { get; set; }

	void Start () {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        checkpoints = new Transform[checkpointObject.childCount];
        currentIndex = 0;
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

        if (playerController.alive)
        {
            if (playerPositionX >= checkpoints[currentIndex].position.x)
            {
                if (currentIndex < checkpoints.Length - 1)
                {
                    playerController.checkpointNumber = currentIndex;
                    currentIndex++;
                }
            }
        }
        if (revert != playerController.startReturn) { revert = playerController.startReturn; }
    }
}
