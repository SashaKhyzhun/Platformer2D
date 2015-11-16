using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
      
    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation + speed * Time.deltaTime);
    }

}