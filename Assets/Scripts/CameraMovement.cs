using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    
    public float targetSpeed = 1;
    public float accelerationTime = 1;
    public float offsetPerc;
    
    private PlayerController playerController;
    private Animator fadeAnimator;
    private Transform camTransform;
    private Vector2 direction;
    private Vector3 endPosition;
    private bool begin = true;
    private float xCurrentPosition;
    private float speed;
    private float camExtent;
    private float camExtentY;
    private float camLowerEdge;
    private float playerSpriteExtent;

    void Start()
    {
        playerController = target.gameObject.GetComponent<PlayerController>();
        fadeAnimator = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        camExtentY = Camera.main.orthographicSize;
        camExtent = camExtentY * Camera.main.aspect;
        camTransform = transform;
        camLowerEdge = camTransform.position.y - camExtentY - 1;
        playerSpriteExtent = target.GetComponent<Renderer>().bounds.extents.x;
        //fadeAnimator.SetFloat("speedMultiplier", 1 / playerController.cameraBackToPositionTime);
    }

    void Update()
    {
        float camPosX = camTransform.position.x;

        if (playerController.start)
        {
            xCurrentPosition = target.position.x;

            if (xCurrentPosition >= camPosX + ((offsetPerc / 100) * camExtent))
            {
                speed = xCurrentPosition - (camPosX + ((offsetPerc / 100) * camExtent));
                if (speed < targetSpeed) { speed = targetSpeed; }
            }
            else
            {
                if (speed >= targetSpeed) { speed -= accelerationTime * speed; }
                else { speed = targetSpeed; }
            }

            //Check if player within camera sight
            if (target.position.x + playerSpriteExtent < camPosX - camExtent
                || target.position.x - playerSpriteExtent > camPosX + camExtent
                || target.position.y  < camLowerEdge)
            {
                playerController.alive = false;
            }
            //Start breaking
            if (!playerController.alive)
            {
                if (speed > 0) { speed -= accelerationTime * speed; }
            }
            if (playerController.startFade) { speed = 0; }

            direction = Vector2.right * speed;
            //transform.Translate(direction * Time.deltaTime);
        }

        if (playerController.startFade)
        {
            speed = 0;

            fadeAnimator.SetBool("Fade", true);

            if (begin)
            {
                endPosition = new Vector3(playerController.checkpointPosition.x - ((offsetPerc / 100) * camExtent), camTransform.position.y, camTransform.position.z);
                begin = false;
            }
        }
        else
        {
            if (!begin)
            {
                begin = true;
            }
        }
        if (playerController.startReturn)
        {
            fadeAnimator.SetBool("Fade", false);
            transform.position = endPosition;
        }
    }

    void LateUpdate()
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
