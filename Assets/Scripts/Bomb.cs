using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

    public float boomForce;

    private Transform myTransform;
    private Rigidbody2D playerRb;

    void Start()
    {
        myTransform = transform;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Vector3 direction = (coll.transform.position - myTransform.position).normalized;
            playerRb = coll.gameObject.GetComponent<Rigidbody2D>();
            playerRb.AddForce(direction * boomForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
  

}
