using UnityEngine;
using System.Collections;

[RequireComponent(typeof (PlayerMotor))]

public class PlayerController: MonoBehaviour {

    public float respawnTime = 2;

    public bool alive { get; set; }
    public bool start { get; set; }
    public bool wait { get; set; }
    public Vector3 respawnPosition { get; set; }
    
    private PlayerMotor motor;
    private bool died = false;
    

    void Awake ()
    {
        motor = GetComponent<PlayerMotor>();
    } 

    void Start()
    {
        wait = false;
        alive = true;
    }

	void Update () {
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
            if (!wait)
            {
                StartCoroutine(Respawn(respawnPosition));
            }
        }
    }

        IEnumerator Respawn(Vector3 position)
    {
        wait = true;
        transform.position = new Vector3(position.x, -10, position.z);
        yield return new WaitForSeconds(respawnTime);
        transform.position = position;
        transform.rotation = new Quaternion();
        start = false;
        alive = true;
        died = true;
        wait = false;
    }
   
}