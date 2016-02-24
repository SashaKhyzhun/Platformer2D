using UnityEngine;
using System.Collections;

public class Press : MonoBehaviour {

    public float restTime;

    private Transform myTransform;
    private Transform pressBody;
    private SliderJoint2D sj;
    private AudioSource audioSource;
    private bool canStartCoroutine = true;
    private bool canPlaySound = true;

	void Start ()
    {
        myTransform = transform;
        audioSource = GetComponent<AudioSource>();
        foreach (Transform ch in myTransform)
        {
            if (ch.CompareTag("Press")) { pressBody = ch; }
        }
        if(pressBody != null) { sj = pressBody.GetComponent<SliderJoint2D>(); }
        else { Debug.LogError(this.name + " does not have a child with tag 'Press'", this); }
	}

	void Update ()
    {
        if (sj != null)
        {
            if (sj.limitState == JointLimitState2D.LowerLimit)
            {
                sj.useMotor = false;
                canPlaySound = true;
            }
            else if (sj.limitState == JointLimitState2D.UpperLimit)
            {
                if (canStartCoroutine) { StartCoroutine(Rest()); };
            }
        }
	}

    private IEnumerator Rest()
    {
        canStartCoroutine = false;
        if (canPlaySound) { canPlaySound = false; audioSource.Play(); }
        yield return new WaitForSeconds(restTime);
        sj.useMotor = true;
        canStartCoroutine = true;
    }
}
