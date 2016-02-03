using UnityEngine;
using System.Collections;

public class BulletCleaner : MonoBehaviour {

    public Transform pool;

    public GameObject origin { get; set; }
    public bool newMS = true;

    private Transform myTransform;
    private Rigidbody2D rb;
    private MachineShooting ms;
    private GameObject lastOrigin;

    void Start()
    {
        myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
    }
	
	void Update ()
    {

        if (origin != null)
        {
            if (origin != lastOrigin)
            {
                ms = origin.GetComponent<MachineShooting>();
            }
            if (!ms.withinReach)
            {
                myTransform.position = pool.position;
                myTransform.rotation = pool.rotation;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                gameObject.SetActive(false);
            }
        }
	}
}
