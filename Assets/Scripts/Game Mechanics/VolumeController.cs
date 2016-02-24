using UnityEngine;

public class VolumeController : MonoBehaviour {

    public float maxVelVol = 16; //use this to adjust the volume
    public bool useCosine = true;
    public LayerMask ignoreLayers; // layers to ignore

    private AudioSource audioSource;
    private Rigidbody2D rb;

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (CompareMask(coll.gameObject.layer, ignoreLayers))
        {
            if (audioSource != null)
            {
                Rigidbody2D otherRb = coll.rigidbody;
                float relVel = 0;
                //absolute value of a cosine of an angle between own velocity and contact point normal (in case of player skimming along)
                float cosine = Mathf.Abs(Mathf.Cos(Vector3.Angle(-rb.velocity.normalized, coll.contacts[0].normal) * Mathf.Deg2Rad));

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

                if (!audioSource.isPlaying)
                {
                    audioSource.volume = Mathf.Clamp01(Mathf.Abs(relVel / maxVelVol)); //fraction of maximum velocity
                    if (useCosine) { audioSource.volume *= cosine; } // depends on angle to the surface
                }
                if (CompareTag("Player"))
                {
                    audioSource.volume = Mathf.Clamp01(Mathf.Abs(relVel / maxVelVol)); //fraction of maximum velocity
                    if (useCosine) { audioSource.volume *= cosine; } // depends on angle to the surface
                }
                audioSource.Play();
            }
            else { Debug.Log("There are no audio source on " + gameObject.name + ". Please add one."); }
        }
    }

    bool CompareMask(int layer, LayerMask layerMask) // works for ignore layers type layer mask
    {
        if (layerMask.value == ~(~layerMask.value | (1<<layer))) return true; // combining inverted layer mask and layermask of given layer and then inverting it
        else return false;
    }

    Vector2 TangentialVelocity(Vector2 point, Vector2 center, float angularVelocity) // angVel must be in rads
    {
        Vector2 positionVector = point - center;
        float r = positionVector.magnitude;
        return new Vector2(positionVector.y, -positionVector.x).normalized * r * -angularVelocity;// * Mathf.Deg2Rad;
    }
}
