using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class RotateObject : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }
      
    void FixedUpdate()
    {
        rb.angularVelocity = speed * Time.deltaTime;
        rb.MoveRotation(rb.rotation + rb.angularVelocity);
    }

}