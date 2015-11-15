using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Culling : MonoBehaviour {

    private Parallax parallax;
    private Renderer rend;
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;
    private Joint2D joint;
    //private Transform myTransform;
    private float leftCamBorder;
    private float rightCamBorder;
    private float spriteCenterX;
    private float selfExtent;

    void Start()
    {
        //myTransform = transform;
        parallax = Camera.main.GetComponent<Parallax>();
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<Joint2D>();
        selfExtent = rend.bounds.extents.x;
        TurnOff();
    }

    void Update()
    {
        spriteCenterX = rend.bounds.center.x;
        if (parallax != null)
        {
            leftCamBorder = parallax.leftCameraBorder;
            rightCamBorder = parallax.rightCameraBorder;
        }
        else { Debug.Log("There is no Parallax script on the camera"); }

        if (leftCamBorder >= spriteCenterX + selfExtent)
        {
            TurnOff();
        }
        if (leftCamBorder <= spriteCenterX + selfExtent && rightCamBorder >= spriteCenterX - selfExtent)
        {
            TurnOn();
        }
    }

    private void TurnOn()
    {
        if (!rend.enabled) { rend.enabled = true; }
        if (anim != null) { if (!anim.enabled) { anim.enabled = true; } }
        if (coll != null) { if (!coll.enabled) { coll.enabled = true; } }
        if (joint != null) { if (!joint.enabled) { joint.enabled = true; } }
        if (rb != null) { if (rb.IsSleeping()) { rb.WakeUp(); } }
    }

    private void TurnOff()
    {
        if (rend.enabled) { rend.enabled = false; }
        if (anim != null) { if (anim.enabled) { anim.enabled = false; } }
        if (coll != null) { if (coll.enabled) { coll.enabled = false; } }
        if (joint != null) { if (joint.enabled) { joint.enabled = false; } }
        if (rb != null) { if (rb.IsAwake()) { rb.Sleep(); } }
    }
}
