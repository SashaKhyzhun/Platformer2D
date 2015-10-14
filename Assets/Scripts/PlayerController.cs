using UnityEngine;

[RequireComponent(typeof (PlayerMotor))]

public class PlayerController: MonoBehaviour {

    public bool alive { get; set; }
    public bool start { get; set; }

    public Vector3 respawnPosition { get; set; }

    private PlayerMotor motor;

    void Awake ()
    {
        motor = GetComponent<PlayerMotor>();
    } 

    void Start()
    {
        alive = true;
    }

	void Update () {
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
        
        /**if (!alive)
        {
            Respawn(respawnPosition);
        } **/

	}

    public void Respawn(Vector3 position)
    {
        transform.position = position;
        start = false;
        alive = true;
    }
   
}