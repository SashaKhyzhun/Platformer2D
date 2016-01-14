using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour, IRevertable {

    public Transform localGM;
    public float boomForce;
    public int ownIndex;

    private Transform myTransform;
    private Rigidbody2D playerRb;
    private CheckpointManager chManager;
    private Renderer spriteRenderer;
    private Collider2D circleCollider;
    private bool initialActive;

    void Start()
    {
        myTransform = transform;
        chManager = localGM.GetComponent<CheckpointManager>();
        spriteRenderer = gameObject.GetComponent<Renderer>();
        circleCollider = gameObject.GetComponent<Collider2D>();
        SaveParams();
    }

    void Update()
    {
        if (chManager != null)
        {
            if (chManager.revert)
            {
                if (chManager.currentIndex <= ownIndex) { LoadParams(); }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //if (coll.gameObject.CompareTag("Player"))
        //{
            Vector3 direction = (coll.transform.position - myTransform.position).normalized;
            playerRb = coll.gameObject.GetComponent<Rigidbody2D>();
            playerRb.AddForce(direction * boomForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            spriteRenderer.enabled = false;
            circleCollider.enabled = false;
        //}
    }
  
    public void SaveParams()
    {
        initialActive = true;
    }

    public void LoadParams()
    {
        spriteRenderer.enabled = initialActive;
        circleCollider.enabled = initialActive;
    }
}
