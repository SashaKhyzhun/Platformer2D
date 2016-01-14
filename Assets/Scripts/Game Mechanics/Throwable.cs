﻿using UnityEngine;

public class Throwable : MonoBehaviour, IRevertable
{
    public Transform body;
    public Transform localGM;
    public bool freeze = true;
    public bool addTorque = false;
    public float throwForce;
    public float torque;
    public int ownIndex;

    private Rigidbody2D bodyRb;
    private CheckpointManager chManager;
    private bool used = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool initialUsed;
    private bool initialKinematic;

    void Start()
    {
        bodyRb = body.GetComponent<Rigidbody2D>();
        chManager = localGM.GetComponent<CheckpointManager>();
        SaveParams();        
    }

    void Update()
    {
        if (chManager != null)
        {
            if (chManager.revert) {
                if (chManager.currentIndex <= ownIndex) { LoadParams(); }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!used)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                Launch(body.up);
            }
        }
    }

    void  OnCollisionEnter2D(Collision2D coll)
    {
        if (freeze)
        {
            if (!bodyRb.isKinematic)
            {
                if (coll.gameObject.CompareTag("Map")) { bodyRb.isKinematic = true; }
            }
        }
    }

    public void Launch(Vector3 direction)
    {
        used = true;
        bodyRb.isKinematic = false;
        bodyRb.AddForce(direction.normalized * throwForce, ForceMode2D.Impulse);
        if (addTorque) { bodyRb.AddTorque(torque); }
    }

    public void SaveParams()
    {
        initialPosition = body.position;
        initialRotation = body.rotation;
        initialUsed = used;
        initialKinematic = bodyRb.isKinematic;
    }

    public void LoadParams()
    {
        body.position = initialPosition;
        body.rotation = initialRotation;
        used = initialUsed;
        bodyRb.isKinematic = initialKinematic;
    }
}