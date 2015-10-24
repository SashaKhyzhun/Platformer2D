using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform player;
    public float targetSpeed = 1;
    public float accelerationTime = 1;
    public float offsetPerc;

    private PlayerController playerController;
    private Transform camTransform;
    private Vector2 direction;
    private Vector3 endPosition;
    private bool begin = true;
    private float xCurrentPosition;
    private float speed;
    private float camExtent;

    void Awake ()
    {
        playerController = player.gameObject.GetComponent<PlayerController>();
        camExtent = Camera.main.orthographicSize * Camera.main.aspect;
        camTransform = transform;
    }

    void FixedUpdate()
    {
        float camPosX = camTransform.position.x;

        if (playerController.start)
        {
            xCurrentPosition = player.position.x;

            if (xCurrentPosition >= camPosX + ((offsetPerc / 100) * camExtent))
            {
                speed = xCurrentPosition - (camPosX + ((offsetPerc / 100) * camExtent));
                if (speed <= targetSpeed)
                {
                    speed = targetSpeed;
                }
            }
            else
            {
                if (speed > targetSpeed)
                {
                    speed -= accelerationTime * speed;
                }
                //else if (speed < targetSpeed)
                //{
                //    //speed += accelerationTime; ;
                //}
                else
                {
                    speed = targetSpeed;
                }
            }
            
            if (player.position.x < camPosX - camExtent || player.position.x > camPosX + camExtent)
            {
                playerController.alive = false;
            }
            if (!playerController.alive)
            {
                if (speed >= 0)
                {
                    speed -= accelerationTime * speed;
                }
            }
            if (playerController.startReturn)
            {
                speed = 0;

                if (begin)
                {
                    endPosition = new Vector3(playerController.checkpointPosition.x - ((offsetPerc / 100) * camExtent), camTransform.position.y, camTransform.position.z);
                    begin = false;
                }
                transform.position = Vector3.Lerp(camTransform.position, endPosition, Time.deltaTime * playerController.cameraTime);
            }
            else
            {
                if (!begin) { begin = true; }
            }

            direction = Vector2.right * speed;
            transform.Translate(direction * Time.fixedDeltaTime);
        }
    }
}
