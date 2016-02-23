using UnityEngine;
using System.Collections;

public class MachineShooting : MonoBehaviour {
    
    public Transform target;
    public Transform muzzle;
    public Transform bulletPool;
    public float spotDistance = 25;
    public float shootForce;
    public float shootRate = 0.50f;

    public bool withinReach { get; set; }

    private Transform myTransform;
    private Transform currBullet;
    private Rigidbody2D currBulletRB;
    private AudioSource audio;
    private BulletCleaner currBulletBC;
    private WaitForSeconds shootRateWFS;
    private bool canShoot = true;
    private int currBulletIndex = 0;
    private int bulletLayer;
    
	void Start() {
        myTransform = transform;
        audio = GetComponent<AudioSource>();
        bulletLayer = gameObject.layer;
        shootRateWFS = new WaitForSeconds(shootRate);
        canShoot = true;
}

	void Update () {
        withinReach = (target.position - myTransform.position).magnitude < spotDistance;
        if (withinReach)
        {
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
	}

    IEnumerator Shoot()
    {
        canShoot = false;

        audio.Play();
        currBullet = bulletPool.GetChild(currBulletIndex);

        currBulletRB = currBullet.GetComponent<Rigidbody2D>();
        currBulletBC = currBullet.GetComponent<BulletCleaner>();

        //currBullet.gameObject.layer = bulletLayer;
        currBullet.gameObject.SetActive(true);

        currBulletBC.origin = gameObject;
        currBullet.position = muzzle.position;
        currBullet.rotation = muzzle.rotation;
        currBulletRB.velocity = Vector2.zero;
        currBulletRB.angularVelocity = 0;
        currBulletRB.AddForce(muzzle.up * shootForce, ForceMode2D.Impulse);

        yield return shootRateWFS;
        canShoot = true;
        if (currBulletIndex < bulletPool.childCount - 1)
        {
            currBulletIndex++;
        }
        else
        {
            currBulletIndex = 0;
        }
    }

    void OnEnable()
    {
        canShoot = true;
    }
}
