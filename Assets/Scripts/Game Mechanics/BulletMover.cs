using UnityEngine;
using System.Collections;

public class BulletMover : MonoBehaviour {

    public GameObject bullet;

    public float bulletSpeed = 5f;
	
	void Update () {

        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, bulletSpeed * Time.deltaTime, 0);
        pos += transform.rotation * velocity;
        transform.position = pos;

        if (Input.GetKey(KeyCode.Space)) {
            bullet.transform.Translate(new Vector3(0, Time.deltaTime * bulletSpeed, 0));
        }
    }

}
