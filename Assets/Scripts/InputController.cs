using UnityEngine;

[RequireComponent(typeof (PlayerMotor))]

public class InputController : MonoBehaviour {

    public bool start { get; set; }

    private PlayerMotor motor;
    private float input;

    void Awake ()
    {
        motor = GetComponent<PlayerMotor>();
    }

	void Update () {
        float _xPlayerPos = Camera.main.WorldToScreenPoint(gameObject.transform.position).x;
        if (Input.GetButtonDown("Fire1"))
        {
            float _xMousePos = Input.mousePosition.x - _xPlayerPos;
            input = (2 * _xMousePos) / Screen.width;
            motor.Jump(input);
            start = true;
        }
        else if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                float _xTouchPos = Input.GetTouch(0).position.x - _xPlayerPos;
                input = (2 * _xTouchPos) / Screen.width;
                motor.Jump(input);
                start = true;
            }
        }
	}
}
