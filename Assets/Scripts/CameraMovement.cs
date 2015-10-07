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
    private float _targetSpeed;
    private float speed = 0;
    private bool canChange = true;

    void Awake ()
    {
        inputController = player.GetComponent<InputController>();
        //rb = player.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _targetSpeed = targetSpeed;
    }

    void FixedUpdate()
    {
        if (inputController.start)
        {
            /*if (speed != targetSpeed)
            {
                speed = Mathf.Lerp(0, targetSpeed, Time.time * accelerationTime);
            }*/

            speed = ChangeSpeed(speed, _targetSpeed);

            xCurrentPosition = Camera.main.WorldToScreenPoint(player.transform.position).x;
            xPlayerDeltaPosition = xCurrentPosition - xLastPosition;

            //Debug.Log("Delta: " + xPlayerDeltaPosition);
            //Debug.Log("Speed: " + speed);
            //Debug.Log("TargetSpeed: " + targetSpeed);
            //if ()
            direction = Vector2.right * speed;
            
            if (xCurrentPosition >= Screen.width / 2)
            {
                //direction += new Vector2(xCurrentPosition - (Screen.width / 2), direction.y);
                _targetSpeed += 0.01f;
                canChange = true;
            }
            else
            {
                if (canChange)
                {
                    if (_targetSpeed > targetSpeed)
                    {
                        _targetSpeed = targetSpeed;
                        canChange = false;
                    }
                }
            }
            
            gameObject.transform.Translate(direction);

            xLastPosition = xCurrentPosition;
        }  
    }

    private float ChangeSpeed(float currenSpeed, float targetSpeed)
    {
        if (currenSpeed != targetSpeed)
        {
            currenSpeed = Mathf.Lerp(0, targetSpeed, Time.time * accelerationTime);
        }
        return currenSpeed;
    }
}
