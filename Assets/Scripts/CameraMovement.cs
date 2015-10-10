using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    public float targetSpeed = 1;
    public float accelerationTime = 1;
    public float offsetPerc;
  
    private PlayerController playerController;
    private Vector2 direction;
    private float xCurrentPosition;
    private float speed;
    private float halfWidth;

    void Awake ()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        halfWidth = Screen.width / 2;
    }

    void FixedUpdate()
    {
        if (playerController.start)
        {
            xCurrentPosition = Camera.main.WorldToScreenPoint(player.transform.position).x;

            direction = Vector2.right * speed;
            
            if (xCurrentPosition >= halfWidth + ((offsetPerc/100) * halfWidth))
            {                
                speed = -(halfWidth + ((offsetPerc / 100) * halfWidth) - xCurrentPosition)  / 10;
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
                    speed += accelerationTime;;
                }
                else
                {
                    speed = targetSpeed;
                }
            }            
            gameObject.transform.Translate(direction * Time.deltaTime);
        }  
    }
}
