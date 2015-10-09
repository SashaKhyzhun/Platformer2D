using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMotor : MonoBehaviour {

    public float xForce;
    public float yForce;
    public float xMaxSpeed;
    public float yMaxSpeed;

    private Rigidbody2D rb;
    private InputController ic;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        ic = GetComponent<InputController>();
        //rb.freezeRotation = true;
    }

    void Update()
    {
        if (ic.start)
        {
            //rb.freezeRotation = false;
        }
    }

    public void MoveUp()
    {
        Vector2 _force = new Vector2(xForce, yForce);

        Vector2 _velocity = rb.velocity;

        if (_velocity.y <= yMaxSpeed)
        {
            rb.AddForce(Vector2.up * yForce, ForceMode2D.Force);
        }

        if (_velocity.x <= xMaxSpeed)
        {
            rb.AddForce(Vector2.right * xForce, ForceMode2D.Force);
        }

        /*
        if (_velocity.x < xMaxSpeed && _velocity. y < yMaxSpeed)
        {
            _force = new Vector2(xForce, yForce);
        }
        else if (_velocity.x > xMaxSpeed)
        {
            _force = new Vector2(0, yForce);
        }
        else if (_velocity.y > yMaxSpeed)
        {
            _force = new Vector2(xForce, 0);
        }
        else
        {
            _force = new Vector2(0, 0);
        }
        */

    }
}
