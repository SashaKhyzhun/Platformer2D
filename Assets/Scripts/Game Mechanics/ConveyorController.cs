using UnityEngine;
using System.Collections;

public class ConveyorController : MonoBehaviour {

    private SurfaceEffector2D surfaceEffector;
    private Animator anim;
    private float speed;

    
	void Start ()
    {
        surfaceEffector = GetComponent<SurfaceEffector2D>();
        anim = GetComponent<Animator>();
        SetSpeed();
	}

    void SetSpeed()
    {
        speed = surfaceEffector.speed;
        anim.SetFloat("speed", speed);
    }
}
