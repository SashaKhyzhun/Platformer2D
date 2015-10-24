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
    }

    public void MoveUp()
    {

        Vector2 _velocity = rb.velocity;

        if (_velocity.y <= yMaxSpeed)
        {
            rb.AddForce(Vector2.up * yForce * Time.deltaTime);
        }

        if (_velocity.x <= xMaxSpeed)
        {
            rb.AddForce(Vector2.right * xForce * Time.deltaTime);
        }
    }
}
