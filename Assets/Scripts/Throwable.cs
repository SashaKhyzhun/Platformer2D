using UnityEngine;
using System.Collections;

public class Throwable : Dangerous {

    public Transform body;
    public float throwForce;

    private Rigidbody2D bodyRb;
    private Collider2D bodyColl;


    void Awake()
    {
        bodyColl = body.GetComponent<Collider2D>();
        bodyRb = body.GetComponent<Rigidbody2D>();
        bodyRb.centerOfMass = transform.localPosition;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player") {
            //bodyColl.enabled = false;
            bodyRb.isKinematic = false;
            bodyRb.AddForce(transform.up * throwForce * Time.deltaTime, ForceMode2D.Impulse);
            //bodyColl.enabled = true;
        }
        
    }


        

}
