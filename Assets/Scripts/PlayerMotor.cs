using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMotor : MonoBehaviour {

    public float xForce;
    public float yForce;
    public float xMaxSpeed;
    public float yMaxSpeed;
    public float zRotationToHold;
    public float rotationHoldForce;
    public float rotationTreshold;

    private Rigidbody2D rb;
    private Transform myTransform;
    private float zCurrRotation;
    private float torque;
    private bool hold = false;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
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

    public void HoldRotation()
    {
        if (hold)
        {
            zCurrRotation = myTransform.rotation.eulerAngles.z;

            if (zCurrRotation > 180) { zCurrRotation -= 360; }
            if (zCurrRotation < -180) { zCurrRotation += 360; }

            

            if (zCurrRotation <= zRotationToHold - rotationTreshold || zCurrRotation >= zRotationToHold + rotationTreshold)
            {
                torque = (zRotationToHold - zCurrRotation) * rotationHoldForce * Time.deltaTime;
            }
            else if (zCurrRotation > zRotationToHold - rotationTreshold && zCurrRotation < zRotationToHold + rotationTreshold)
            {
                if (torque != 0) { torque = 0; }
                //torque = -rb.angularVelocity * 0.0005f;
            }

            rb.AddTorque(torque, ForceMode2D.Force);
        }
    }

    void OnCollisionEnter2D()
    {
        hold = false;
    }

    void OnCollisionExit2D()
    {
        hold = true;
    }
}
