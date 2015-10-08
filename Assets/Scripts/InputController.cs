using UnityEngine;

[RequireComponent(typeof (PlayerMotor))]

public class InputController : MonoBehaviour {

    public bool start { get; set; }

    private PlayerMotor motor;

    void Awake ()
    {
        motor = GetComponent<PlayerMotor>();
    }

	void Update () {
        float _xPlayerPos = Camera.main.WorldToScreenPoint(gameObject.transform.position).x;
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
}
