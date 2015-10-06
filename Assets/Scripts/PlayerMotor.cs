using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMotor : MonoBehaviour {

    public float xForce = 1;
    public float yForce = 1;

    private Rigidbody2D rb;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    public void Jump(float input)
    {
        Vector2 _force = new Vector2(xForce * input, yForce);
        rb.AddForce(_force, ForceMode2D.Impulse);
    }

}
