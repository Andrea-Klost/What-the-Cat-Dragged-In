using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cat : MonoBehaviour {
    [Header("Inscribed")] 
    public float movementSpeed = 10f;
    public float jumpImpulse = 10f;
    [Tooltip("Radius of capsule used for checking if the cat can jump (i.e. if the player is grounded)")]
    public float colliderRadius = 1f;
    [Tooltip("Front position of capsule used for checking if the cat can jump (i.e. if the player is grounded)")]
    public Transform frontPosition;
    [Tooltip("Back position of capsule used for checking if the cat can jump (i.e. if the player is grounded)")]
    public Transform backPosition;
    [Tooltip("How fast the cat rotates to match the player's input")]
    public float rotateSpeed = 1f;
    
    private Rigidbody rbody;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
    }

    void Update() {
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        Vector3 velocity = rbody.velocity;
        velocity.x = inputVector.normalized.x * movementSpeed;
        velocity.z = inputVector.normalized.z * movementSpeed;
        rbody.velocity = velocity;

        // Rotate player towards movement
        // Round so that rotation always lands on full input
        Vector3 targetDirection = new Vector3(Mathf.Round(inputVector.x), 0, Mathf.Round(inputVector.z));
        float singleStep = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection,
            singleStep, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (Input.GetKeyDown("space"))
            Jump();
    }

    void Jump() {
        // Check if the player is grounded before jumping by checking for colliders under them
        if (Physics.CheckCapsule(frontPosition.position, backPosition.position, colliderRadius)) {
            Vector3 velocity = rbody.velocity;
            velocity.y += jumpImpulse;
            rbody.velocity = velocity;
        }
    }
}
