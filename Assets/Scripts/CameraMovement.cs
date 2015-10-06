using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    public float maxSpeed = 1;
    public float accelerationTime = 1;
  
    private InputController iController;
    private float speed = 0;

    void Awake ()
    {
        iController = player.GetComponent<InputController>();
    }

    void FixedUpdate()
    {
        if (iController.start)
        {
            if (speed <= maxSpeed)
            {
                speed = Mathf.Lerp(0, maxSpeed, Time.time * accelerationTime);
            }
            Vector2 _direction = Vector2.right * speed;
            gameObject.transform.Translate(_direction);
        }
    }
}
