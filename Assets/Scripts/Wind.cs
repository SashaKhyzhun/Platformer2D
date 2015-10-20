using UnityEngine;
    
public class Wind : MonoBehaviour {

    public float windForce; 

    private Rigidbody2D rb;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        rb = collider.gameObject.GetComponent<Rigidbody2D>();
    }


    void OnTriggerStay2D (Collider2D collider)
    {
        if (rb != null)
        {
            rb.AddForce(transform.right * windForce * Time.deltaTime);
        }    

        //Debug.Log(collider.name);

    }


    

    


}
