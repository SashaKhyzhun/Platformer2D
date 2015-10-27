using UnityEngine;
using System.Collections;

[RequireComponent(typeof (PlayerMotor))]

public class PlayerController: MonoBehaviour {

    public float cameraStayTime;
    public float cameraBackToPositionTime;

    public bool alive { get; set; }
    public bool start { get; set; }
    public bool wait { get; set; }
    public bool startReturn { get; set; }
    public Transform[] checkpoints { get; set; }
    public Vector3 checkpointPosition { get; set; }
    public float cameraTime { get; set; }
    public int checkpointNumber { get; set; }

    private Rigidbody2D rb;  
    private PlayerMotor motor;
    private bool died = false;
    

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        motor = GetComponent<PlayerMotor>();
        wait = false;
        alive = true;
        cameraTime = cameraBackToPositionTime;
    }

	void Update () {
        if (!wait)
        {
            if (!died)
            {
                if (Input.GetButton("Fire1"))
                {
                    motor.MoveUp();
                    start = true;

                }
                else if (Input.touchCount > 0)
                {
                    motor.MoveUp();
                    start = true;

                }
                motor.HoldRotation();
            }
            if (Input.GetButtonDown("Fire1"))
            {
                died = false;
            }
            else if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    died = false;
                }
            }
            if (!alive)
            {
                checkpointPosition = checkpoints[checkpointNumber].position;
                StartCoroutine(Respawn(checkpointPosition));
            }
        }
    }

        IEnumerator Respawn(Vector3 position)
    {
        wait = true;
        transform.position = position;
        transform.rotation = new Quaternion();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
        yield return new WaitForSeconds(cameraStayTime);
        startReturn = true;
        yield return new WaitForSeconds(cameraBackToPositionTime);
        rb.isKinematic = false;
        start = false;
        startReturn = false;
        alive = true;
        died = true;
        wait = false;
    }
   
}