using UnityEngine;
using System.Collections;

public class Throwable : Dangerous {

    public Transform body;
    public float throwForce;

    private Rigidbody2D bodyRb;
    private bool used = false;

    void Awake()
    {
        bodyRb = body.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!used)
        {
            if (coll.gameObject.tag == "Player")
            {
                used = true;
                bodyRb.isKinematic = false;
                bodyRb.AddForce(transform.up * throwForce * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
    }

    void  OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Map")
        {
            bodyRb.isKinematic = true;
        }
    }
        

}
