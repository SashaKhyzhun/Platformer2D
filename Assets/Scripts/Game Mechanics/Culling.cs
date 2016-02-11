using UnityEngine;

//[RequireComponent(typeof(Renderer))]
public class Culling : MonoBehaviour {

    private Transform myTransform;
    private Parallax parallax;
    private Renderer rend;
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;
    private RotateObject rotObj;
    private bool active = true;

    private bool isKinematic;

    private float leftCamBorder;
    private float rightCamBorder;
    private float spriteCenterX;
    private float selfExtent;

    void Start()
    {
        myTransform = transform;
        parallax = Camera.main.GetComponent<Parallax>();
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rotObj = GetComponent<RotateObject>();        
        if (rend != null) { selfExtent = rend.bounds.extents.x; }
        else { selfExtent = 2; }
        if (rb != null) { isKinematic = rb.isKinematic; }

        TurnOff();
    }

    void Update()
    {
        //spriteCenterX = rend.bounds.center.x;

        if (rend != null) { spriteCenterX = rend.bounds.center.x; }
        else { spriteCenterX = myTransform.position.x; }

        if (parallax != null)
        {
            leftCamBorder = parallax.leftCameraBorder;
            rightCamBorder = parallax.rightCameraBorder;
        }
        else { Debug.Log("There is no Parallax script on the camera"); }

        if (active)
        {
            if (leftCamBorder >= spriteCenterX + selfExtent || rightCamBorder <= spriteCenterX - selfExtent) { TurnOff(); }
        }
        if (!active)
        {
            if (leftCamBorder <= spriteCenterX + selfExtent && rightCamBorder >= spriteCenterX - selfExtent) { TurnOn(); }
        }
    }

    public void TurnOn()
    {
        active = true;
        if (rend != null) { if (!rend.enabled) { rend.enabled = true; } }
        if (anim != null) { if (!anim.enabled) { anim.enabled = true; } }
        if (coll != null) { if (!coll.enabled) { coll.enabled = true; } }
        if (rotObj != null) { if (!rotObj.enabled) { rotObj.enabled = true; } }
        if (rb != null)
        {
            if (rb.isKinematic != isKinematic) { rb.isKinematic = isKinematic; }
            if (rb.IsSleeping()) { rb.WakeUp(); }
            //rb.simulated = true;
        }
    }

    private void TurnOff()
    {
        active = false;
        if (rend != null) { if (rend.enabled) { rend.enabled = false; } }
        if (anim != null) { if (anim.enabled) { anim.enabled = false; } }
        if (coll != null) { if (coll.enabled) { coll.enabled = false; } }
        if (rotObj != null) { if (rotObj.enabled) { rotObj.enabled = false; } }
        if (rb != null)
        {
            if (!rb.isKinematic) { rb.isKinematic = true; }
            if (rb.IsAwake()) { rb.Sleep(); }
            //rb.simulated = false;
        }        
    }
}
