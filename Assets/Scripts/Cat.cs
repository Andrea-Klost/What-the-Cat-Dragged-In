using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cat : MonoBehaviour {
    [Header("Inscribed")] 
    public float movementSpeed = 10f;
    public float jumpImpulse = 10f;
    [Tooltip("Radius of sphere used for checking if the cat can jump (i.e. if the player is grounded)")]
    public float colliderRadius = 1f; 
    
    private Rigidbody rbody;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
    }

    void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * movementSpeed * Time.deltaTime;
        pos.z += vAxis * movementSpeed * Time.deltaTime;
        transform.position = pos;
        
        if (Input.GetKeyDown("space"))
            Jump();
    }

    void Jump() {
        // Check if the player is grounded before jumping by checking for colliders under them
        if (Physics.CheckSphere(transform.position + Vector3.up * (colliderRadius * 0.9f), colliderRadius)) {
            Vector3 velocity = rbody.velocity;
            velocity.y += jumpImpulse;
            rbody.velocity = velocity;
        }
    }
}
