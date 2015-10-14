using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform player;
    public float targetSpeed = 1;
    public float accelerationTime = 1;
    public float offsetPerc;
  
    private PlayerController playerController;
    private Vector2 direction;
    private float xCurrentPosition;
    private float speed;
    private float camExtent;

    void Awake ()
    {
        playerController = player.gameObject.GetComponent<PlayerController>();
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
            if (!playerController.wait)
            {
                xCurrentPosition = player.position.x;//Camera.main.WorldToScreenPoint(player.transform.position).x;

                direction = Vector2.right * speed;

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
                        speed -= accelerationTime;
                    }
                    else if (speed < targetSpeed)
                    {
                        speed += accelerationTime; ;
                    }
                    else
                    {
                        speed = targetSpeed;
                    }
                }
                gameObject.transform.Translate(direction * Time.deltaTime);
            }
        }
        else
        {
            Vector3 respawnPosition = new Vector3(player.transform.position.x - ((offsetPerc / 100) * camExtent),
                                                                              transform.position.y, transform.position.z);
            if (transform.position != respawnPosition && speed != 0) {
                transform.position = respawnPosition;
                speed = 0;
            }
        }

        if (player.position.x < camPosX - camExtent || player.position.x > camPosX + camExtent)
        {
            playerController.alive = false;
        }
    }
}
