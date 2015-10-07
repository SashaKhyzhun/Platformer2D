using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMotor : MonoBehaviour {

    public float xForce;
    public float yForce;
    public float xMaxSpeed;
    public float yMaxSpeed;

    private Rigidbody2D rb;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.freezeRotation = true;
    }

    public void Jump()
    {
        Vector2 _force = new Vector2();

        Vector2 _velocity = rb.velocity;

        if (_velocity.x < xMaxSpeed || _velocity. y < yMaxSpeed)
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
        
        rb.AddForce(_force, ForceMode2D.Force);
    }

}
