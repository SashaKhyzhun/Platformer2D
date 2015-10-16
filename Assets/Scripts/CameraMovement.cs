using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform player;
    public float targetSpeed = 1;
    public float accelerationTime = 1;
    public float offsetPerc;

    //private Renderer playerRenderer;
    private PlayerController playerController;
    private Vector2 direction;
    //private Vector3 startPosition;
    private Vector3 endPosition;
    private bool begin = true;
    //private float startTime;
    //private float distance;
    private float xCurrentPosition;
    private float speed;
    private float camExtent;

    void Awake ()
    {
        playerController = player.gameObject.GetComponent<PlayerController>();
        //playerRenderer = player.gameObject.GetComponent<Renderer>();
    }

    void Start()
    {
        //camExtent = Screen.width / 2;
        camExtent = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void FixedUpdate()
    {
        float camPosX = Camera.main.transform.position.x;

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
                else if (speed < targetSpeed)
                {
                    //speed += accelerationTime; ;
                }
                else
                {
                    speed = targetSpeed;
                }
            }
            
            if (player.position.x < camPosX - camExtent || player.position.x > camPosX + camExtent)
            {
                //speed = 0;

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
                    //startTime = Time.time;
                    //startPosition = transform.position;
                    endPosition = new Vector3(playerController.checkpointPosition.x - ((offsetPerc / 100) * camExtent), transform.position.y, transform.position.z);
                    //distance = Vector3.Distance(startPosition, endPosition);
                    begin = false;
                }

                //transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) / playerController.cameraTime);
                transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * playerController.cameraTime); // (playerController.cameraTime / 3));
                //Debug.Log(Time.time - startTime);
            }
            else
            {
                if (!begin) { begin = true; }
            }

            direction = Vector2.right * speed;
            gameObject.transform.Translate(direction * Time.deltaTime);
        }

        //Debug.DrawRay(new Vector3(camPosX + ((offsetPerc / 100) * camExtent), -10,0), Vector3.up * 100);
    }
}
