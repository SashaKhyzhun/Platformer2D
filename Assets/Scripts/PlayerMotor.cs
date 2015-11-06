using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMotor : MonoBehaviour {

    public float constForce;
    public float xForce;
    public float yForce;
    public float constSpeed;
    public float xMaxSpeed;
    public float yMaxSpeed;
    public float zRotationToHold;
    public float rotationHoldForce;
    public float rotationTreshold;
    public float rotationBreakForce;

    private Rigidbody2D rb;
    private Transform myTransform;
    private Vector2 vector2Up = Vector2.up;
    private Vector2 vector2Right = Vector2.right;
    private float zCurrRotation;
    private float torque;
    private bool hold = false;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = transform;
    }

    public void MoveRight()
    {
        if (hold)
        {
            if (rb.velocity.x <= constSpeed)
            {
                rb.AddForce(vector2Right * constForce * Time.deltaTime);
            }
        }
    }

    public void MoveUp()
    {
        Vector3 velocity = rb.velocity;
        if (velocity.y <= yMaxSpeed)
        {
            rb.AddForce(vector2Up * yForce * Time.deltaTime);
        }

        if (velocity.x <= xMaxSpeed)
        {
            rb.AddForce(vector2Right * xForce * Time.deltaTime);
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
                //if (torque != 0) { torque = 0; }
                torque = -rb.angularVelocity * rotationBreakForce * Time.deltaTime;
            }

            if (torque > 0.001 || torque < -0.001)
            {
                rb.AddTorque(torque, ForceMode2D.Force);
            }
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
