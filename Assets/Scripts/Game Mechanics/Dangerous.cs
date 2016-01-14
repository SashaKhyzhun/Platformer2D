using UnityEngine;

public class Dangerous : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Kill(coll.gameObject);
        }
    }

    void Kill(GameObject player)
    {
        player.GetComponent<PlayerController>().alive = false;
    }
}
