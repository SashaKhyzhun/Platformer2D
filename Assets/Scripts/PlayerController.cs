using UnityEngine;

[RequireComponent(typeof (PlayerMotor))]

public class PlayerController: MonoBehaviour {

    public bool alive { get; set; } 
    public bool start { get; set; }

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
        Debug.Log("Alive: " + alive);
	}
}