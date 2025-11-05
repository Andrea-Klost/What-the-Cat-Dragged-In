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
    public float rotateSpeed = 1f;
    
    private Rigidbody rbody;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
    }

    void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 velocity = rbody.velocity;
        velocity.x = hAxis * movementSpeed;
        velocity.z = vAxis * movementSpeed;
        rbody.velocity = velocity;
        
        // Rotate towards movement only when input is given
        if (hAxis != 0 || vAxis != 0) {
            transform.rotation = Quaternion.LookRotation(new Vector3(hAxis, 0, vAxis));
        }

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
