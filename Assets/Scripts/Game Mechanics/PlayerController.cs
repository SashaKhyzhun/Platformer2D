using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof (PlayerMotor))]

public class PlayerController: MonoBehaviour {

    public float cameraStayTime;
    public float maxVelVol = 4;
    public float blinkTime;
    public float lookUpTime;

    public bool alive { get; set; }
    public bool finished { get; set; }
    public bool canLoad { get; set; }
    public bool start { get; set; }
    public bool wait { get; set; }
    public bool startFade { get; set; }
    public bool startReturn { get; set; }
    public bool canTouch { get; set; }
    public Transform[] checkpoints { get; set; }
    public Vector3 checkpointPosition { get; set; }
    public float cameraBackToPositionTime { get; set; }
    public int checkpointNumber { get; set; }
    public int deaths { get; set; }
    public float time { get; set; }
    public float startTime { get; set; }

    private EventSystem es;
    private WaitForSeconds halfBackTime;
    private WaitForSeconds fullBackTime;
    private WaitForSeconds blinkTimeWFS;
    private WaitForSeconds lookUpTimeWFS;
    private Rigidbody2D rb;
    private PlayerMotor motor;
    private Animator anim;
    private Throwable throwable;
    private AudioSource audioSource;
    
    private bool canControl = false;
    private bool died = false;
    private bool first = true;
    private bool canBlink = true;
    private bool canLookUp = true;
    

    void Start()
    {
        es = EventSystem.current;
        rb = GetComponent<Rigidbody2D>();
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<Animator>();
        throwable = GetComponent<Throwable>();
        audioSource = GetComponent<AudioSource>();
        GameObject globalGM = GameObject.FindGameObjectWithTag("globalGM");
        if (globalGM != null) { cameraBackToPositionTime = globalGM.GetComponent<UIManager>().fadeTime; }
        halfBackTime = new WaitForSeconds(cameraBackToPositionTime / 2);
        fullBackTime = new WaitForSeconds(cameraBackToPositionTime);
        blinkTimeWFS = new WaitForSeconds(blinkTime);
        lookUpTimeWFS = new WaitForSeconds(lookUpTime);
        wait = false;
        alive = true;
        canLoad = true;
        canTouch = true;
        StartCoroutine(WaitAtStart());
        time = 0;
    }
    
	void Update () {
        if (canControl)
        {
            if (!wait)
            {
                if (!died)
                {
                    // animations coroutines start
                    if(canBlink) { StartCoroutine(Blink()); }
                    if (canLookUp) { StartCoroutine(LookUp()); }

                    if (Input.touchCount > 0 || Input.GetButton("Fire1"))
                    {
                        if (es != null)
                        {
                            if (!es.IsPointerOverGameObject())
                            {
                                motor.moveUp = true;
                                if (!start)
                                {
                                    start = true;
                                    if (first)
                                    {
                                        first = false;
                                        startTime = Time.time;
                                    }
                                }
                            }
                        }
                        else
                        {
                            motor.moveUp = true;
                            if (!start)
                            {
                                start = true;
                                if (first)
                                {
                                    first = false;
                                    startTime = Time.time;
                                }
                            }
                        }                        
                    }

                    motor.holdRotation = true;
                    motor.moveRight = true;
                }
                else
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        died = false;
                    }
                    else if (Input.touchCount > 0)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            died = false;
                        }
                    }
                }
                if (!alive)
                {
                    checkpointPosition = checkpoints[checkpointNumber].position;
                    if (!finished) { StartCoroutine(Respawn(checkpointPosition)); }
                    else
                    {
                        if (canLoad)
                        { time = Time.time - startTime; }
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (audioSource != null) {
            Rigidbody2D otherRb = coll.rigidbody;
            float relVel = 0;

            if (otherRb != null)
            {
                if (otherRb.velocity.sqrMagnitude < 0.0001f) // if not moving
                {
                    // relVel = difference between own velocity and tangent velocity of collision point
                    relVel = (rb.velocity - TangentialVelocity(coll.contacts[0].point, coll.transform.position, otherRb.angularVelocity)).magnitude;
                }
                else
                {
                    // else equals to a difference between velocities
                    relVel = (rb.velocity - otherRb.velocity).magnitude;
                }
            }
            //if has no rb relVel equals to own velocity
            else { relVel = rb.velocity.magnitude; }

            audioSource.volume = relVel / maxVelVol; //fraction of maximum velocity
            audioSource.Play();
        }
        else { Debug.Log("There are no audio source on " + gameObject.name + ". Please add one." ); }
    }

    Vector2 TangentialVelocity(Vector2 point, Vector2 center, float angularVelocity) // angVel must be in rads
    {
        Vector2 positionVector = point - center;
        float r = positionVector.magnitude;
        return new Vector2(positionVector.y, -positionVector.x).normalized * r * -angularVelocity;// * Mathf.Deg2Rad;
    } 

    IEnumerator Blink()
    {
        canBlink = false;
        yield return blinkTimeWFS;
        anim.SetTrigger("blink");
        canBlink = true;
    }

    IEnumerator LookUp()
    {
        canLookUp = false;
        yield return lookUpTimeWFS;
        anim.SetTrigger("lookUp");
        canLookUp = true;
    }

    IEnumerator Respawn(Vector3 position)
    {
        deaths++;
        wait = true;
        anim.SetTrigger("death");
        gameObject.layer = 13; // Player_dead layer
        
        yield return fullBackTime;

        //reset transfor
        transform.position = new Vector3(0, -10, 0);
        transform.rotation = new Quaternion();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        //
        rb.isKinematic = true;
        gameObject.layer = 11; // Player layer

        startFade = true;

        yield return halfBackTime;

        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        startReturn = true;

        // back to checkpoint
        transform.position = position;

        yield return halfBackTime;

        rb.isKinematic = false;
        start = false;
        startFade = false;
        startReturn = false;
        alive = true;
        died = true;
        wait = false;
    }



    IEnumerator WaitAtStart()
    {
        yield return halfBackTime;
        if (throwable != null)
        {
            throwable.Launch(motor.throwDirection);
            throwable.enabled = false;
        }
        yield return fullBackTime;
        yield return fullBackTime;
        canControl = true;
    }
}