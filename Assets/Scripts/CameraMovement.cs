using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    public float targetSpeed = 1;
    public float accelerationTime = 1;
  
    private InputController inputController;
    //private Rigidbody2D rb;
    private Vector2 direction;
    private float xPlayerDeltaPosition;
    private float xLastPosition;
    private float xCurrentPosition;
    private float speed;
    private float halfWidth;
    private float currentSpeed;
    private bool canChange = true;

    void Awake ()
    {
        inputController = player.GetComponent<InputController>();
    }

    void Start()
    {
        halfWidth = Screen.width / 2;
    }

    void FixedUpdate()
    {
        if (inputController.start)
        {
            //speed = ChangeSpeed(speed, _targetSpeed);

            xCurrentPosition = Camera.main.WorldToScreenPoint(player.transform.position).x;
            float xDeltaPosition = (xCurrentPosition - halfWidth) / halfWidth;
            Debug.Log(xDeltaPosition);
            direction = Vector2.right * speed;
            

            if (xCurrentPosition >= halfWidth)
            {
                speed += accelerationTime * xDeltaPosition;
            }
            else
            {
                if (speed > targetSpeed)
                {
                    speed -= accelerationTime * Mathf.Abs(xDeltaPosition);
                    canChange = false;
                }
                else if (speed < targetSpeed)
                {
                    speed += accelerationTime; //* xDeltaPosition;
                }
                else
                {
                    speed = targetSpeed;
                }
            }
            
            gameObject.transform.Translate(direction);
        }  
    }
    /*
    private float ChangeSpeed(float speed, float targetSpeed)
    {
        if (speed != targetSpeed)
        {
            speed = Mathf.Lerp(currentSpeed, targetSpeed, Time.time * accelerationTime);
        }
        return speed;
    }
    */
}
