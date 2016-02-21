using UnityEngine;

public class Culling : MonoBehaviour {

    private Transform myTransform;
    private Transform mainCamTransform;
    private Camera mainCam;
    private Parallax parallax;
    private Renderer rend;
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;
    private RotateObject rotObj;
    private bool active = true;
    private bool isKinematic;
    private float camDiagonal;
    void Start()
    {
        myTransform = transform;
        mainCam = Camera.main;
        mainCamTransform = mainCam.transform;
        camDiagonal = (new Vector2(mainCam.orthographicSize * mainCam.aspect, 0) + new Vector2(0, mainCam.orthographicSize)).magnitude; // diagonal is a mgnitude of a sum of width and height vectors
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rotObj = GetComponent<RotateObject>();
        if (rb != null) { isKinematic = rb.isKinematic; }
        TurnOff();
    }

    void Update()
    {
        if (active)
        {
            if (rend != null)
            {
                if (!rend.IsVisibleFrom(mainCam)) { TurnOff(); }
            }
            else
            {
                if ((mainCamTransform.position.x - myTransform.position.x) > camDiagonal) { TurnOff(); }
            }
        }
        if (!active)
        {
            if (rend != null)
            {
                if (rend.IsVisibleFrom(mainCam)) { TurnOn(); }
            }
            else
            {
                if ((mainCamTransform.position.x - myTransform.position.x) < camDiagonal) { TurnOn(); }
            }
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
        }        
    }
}
