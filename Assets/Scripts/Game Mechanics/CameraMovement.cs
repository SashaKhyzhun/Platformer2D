using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    public Transform finish;
    
    public float targetSpeed = 1;
    public float accelerationTime = 1;
    public float followTemp = 1;
    public float offsetPerc;
    
    private PlayerController playerController;
    private Transform camTransform;
    private Vector2 direction;
    private Vector3 endPosition;
    private bool returnToPos = true;
    private float xCurrentPosition;
    private float speed;
    private float camExtent;
    private float camExtentY;
    private float camLowerEdge;
    private float camUpperEdge;
    private float playerSpriteExtent;

    void Start()
    {
        playerController = target.gameObject.GetComponent<PlayerController>();
        camExtentY = Camera.main.orthographicSize;
        camExtent = camExtentY * Camera.main.aspect;
        camTransform = transform;
        camLowerEdge = camTransform.position.y - camExtentY - 1;
        camUpperEdge = camTransform.position.y + camExtentY + 1;
        playerSpriteExtent = target.GetComponent<Renderer>().bounds.extents.x;
    }

    void Update()
    {
        float camPosX = camTransform.position.x;

        if (playerController.start)
        {
            xCurrentPosition = target.position.x;

            if (playerController.alive)
            {
                if (xCurrentPosition >= camPosX + ((offsetPerc / 100) * camExtent))
                {
                    speed = (xCurrentPosition - (camPosX + ((offsetPerc / 100) * camExtent))) * followTemp;
                    if (speed < targetSpeed) { speed = targetSpeed; }
                }
                else
                {
                    if (speed >= targetSpeed) { speed -= accelerationTime * Time.deltaTime * speed; }
                    else { speed = targetSpeed; }
                }
            }
            //Check if player within camera sight
            if (target.position.x + playerSpriteExtent < camPosX - camExtent
                || target.position.x - playerSpriteExtent > camPosX + camExtent
                || target.position.y < camLowerEdge
                || target.position.y > camUpperEdge)
            {
                playerController.alive = false;
            }
            //Start breaking
            if (!playerController.alive) { if (speed > 0) { speed -= accelerationTime * Time.deltaTime * speed; } }
            if (playerController.startFade) { speed = 0; }
        }

        if (playerController.startFade)
        {
            speed = 0;

            if (returnToPos)
            {
                endPosition = new Vector3(playerController.checkpointPosition.x - ((offsetPerc / 100) * camExtent), camTransform.position.y, camTransform.position.z);
                returnToPos = false;
            }
        }
        else
        {
            if (!returnToPos)
            {
                returnToPos = true;
            }
        }
        if (playerController.startReturn)
        {
            transform.position = endPosition;
        }

        direction = Vector2.right * speed;
    }

    void LateUpdate()
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
