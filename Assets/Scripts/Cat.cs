using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cat : MonoBehaviour {
    [Header("Movement")] 
    public float movementSpeed = 10f;
    public float jumpImpulse = 10f;

    [Header("Grabbing")]
    [Tooltip("Point that defines position of object when grabbed")]
    public Transform grabPoint;
    [Tooltip("An object that is currently able to be grabbed")]
    [SerializeField] private GameObject grabTarget;
    [Tooltip("The object that is currently grabbed")]
    [SerializeField] private GameObject grabbedObject;
    
    
    [Header("Ground Checking")]
    [Tooltip("Radius of capsule used for checking if the cat can jump (i.e. if the player is grounded)")]
    public float colliderRadius = 1f;
    [Tooltip("Front position of capsule used for checking if the cat can jump (i.e. if the player is grounded)")]
    public Transform frontPosition;
    [Tooltip("Back position of capsule used for checking if the cat can jump (i.e. if the player is grounded)")]
    public Transform backPosition;
    
    [Header("Rotation")]
    [Tooltip("How fast the cat rotates to match the player's input")]
    public float rotateSpeed = 1f;
    
    private Rigidbody rbody;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
        grabTarget = grabbedObject = null;
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
        if (Input.GetKeyDown("o"))
            Grab();
    }

    void Jump() {
        // Check if the player is grounded before jumping by checking for colliders under them
        if (Physics.CheckCapsule(frontPosition.position, backPosition.position, colliderRadius, 
                1 << LayerMask.NameToLayer("Ground"))) {
            Vector3 velocity = rbody.velocity;
            velocity.y += jumpImpulse;
            rbody.velocity = velocity;
        }
    }

    void OnTriggerEnter(Collider other) {
        Grabbable grabScript = other.GetComponent<Grabbable>();
        if (grabScript != null) {
            grabTarget = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject == grabTarget) {
            grabTarget = null;
        }
    }
    
    void Grab() {
        if (grabbedObject != null) { // Already have an object, attempt to drop
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
        else { // No grabbed object, grab current grab target if exists
            if (grabTarget == null) return;
            grabTarget.transform.SetParent(grabPoint, false);
            grabTarget.transform.position = grabPoint.transform.position;
            grabbedObject = grabTarget;
            grabTarget = null;
        }
    }
}
