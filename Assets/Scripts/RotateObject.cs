using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
   
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().rotation = Quaternion.identity;
    }

}