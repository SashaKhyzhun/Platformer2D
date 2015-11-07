using UnityEngine;
using System.Collections;

[RequireComponent(typeof (PlayerMotor))]

public class PlayerController: MonoBehaviour {

    public float cameraStayTime;
    public float cameraBackToPositionTime;

    public bool alive { get; set; }
    public bool start { get; set; }
    public bool wait { get; set; }
    public bool startFade { get; set; }
    public bool startReturn { get; set; }
    public Transform[] checkpoints { get; set; }
    public Vector3 checkpointPosition { get; set; }
    public float cameraTime { get; set; }
    public int checkpointNumber { get; set; }

    private Rigidbody2D rb;  
    private PlayerMotor motor;
    private bool canPlay = false;
    private bool died = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        motor = GetComponent<PlayerMotor>();
        wait = false;
        alive = true;
        cameraTime = cameraBackToPositionTime;
        StartCoroutine(WaitAtStart());
    }

	void Update () {
        if (canPlay)
        {
            if (!wait)
            {
                if (!died)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        motor.moveUp = true;
                        start = true;

                    }
                    else if (Input.touchCount > 0)
                    {
                        motor.moveUp = true;
                        start = true;

                    }
                    motor.holdRotation = true;
                    motor.moveRight = true;
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
    }

    IEnumerator Respawn(Vector3 position)
    {
        WaitForSeconds halfBacktime = new WaitForSeconds(cameraBackToPositionTime / 2);
        wait = true;
        transform.position = new Vector3(0, -10, 0);
        transform.rotation = new Quaternion();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
        yield return new WaitForSeconds(cameraStayTime);
        startFade = true;
        yield return halfBacktime;
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        startReturn = true;
        transform.position = position;
        yield return halfBacktime;
        rb.isKinematic = false;
        start = false;
        startFade = false;
        startReturn = false;
        alive = true;
        died = true;
        wait = false;
    }

    IEnumerator WaitAtStart()
    {
        yield return new WaitForSeconds(cameraBackToPositionTime);
        canPlay = true;
    }

}