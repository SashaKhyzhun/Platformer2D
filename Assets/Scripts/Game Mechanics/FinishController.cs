using UnityEngine;

public class FinishController : MonoBehaviour {

    PlayerController pc;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            if (pc == null) { pc = coll.GetComponent<PlayerController>(); }
            pc.alive = false;
            pc.finished = true;
        }
    }
}
