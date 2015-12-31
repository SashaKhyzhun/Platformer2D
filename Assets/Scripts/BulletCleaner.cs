using UnityEngine;
using System.Collections;

public class BulletCleaner : MonoBehaviour {

    public float timer = 3f;
	
	void Update () {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Destroy(gameObject);
        }
	}
}
