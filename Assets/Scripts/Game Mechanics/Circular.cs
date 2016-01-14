using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Circular : Dangerous {

    private Transform myTransform;
    private HingeJoint2D hinge;

	void Start () {
        hinge = GetComponent<HingeJoint2D>();
        myTransform = transform;
        hinge.connectedAnchor = myTransform.position;
    }
}
