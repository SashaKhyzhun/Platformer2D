using UnityEngine;
using System.Collections;

public class ChainVolumeController : MonoBehaviour {

    public float maxVelVol;
    public float minVel;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private WaitForSeconds audioCooldown;
    private float currVel;
    private bool hasRb = false; //for not loading update with pointless checks 
    private bool canStartCoroutine = true;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (rb != null) { hasRb = true; }
        audioCooldown = new WaitForSeconds(Random.Range(0.2f, 0.5f));
	}

    void Update ()
    {
	    if (hasRb)
        {
            currVel = rb.velocity.magnitude;
            if (currVel > minVel)
            {
                audioSource.volume = currVel / maxVelVol;
                if (canStartCoroutine) { StartCoroutine(PlayAudio()); }
            }
        }
	}
    

    private IEnumerator PlayAudio()
    {
        canStartCoroutine = false;
        audioSource.Play();
        yield return audioCooldown;
        canStartCoroutine = true;
    }
}
