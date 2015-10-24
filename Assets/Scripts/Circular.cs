using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Circular : Dangerous {

    private Transform myTransform;
    private HingeJoint2D hinge;

	void Awake () {
        hinge = GetComponent<HingeJoint2D>();
        myTransform = transform;
	}
	
	void Update () {
        hinge.connectedAnchor = myTransform.position;
	}

}
