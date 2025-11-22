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
        if (Input.GetKeyDown("e"))
            Grab();
        if (Input.GetKeyDown("return"))
            OrderSystem.START_LEVEL();
        if (Input.GetKeyDown("tab"))
            RecipeMenu.TOGGLE_MENU();
    }

    void Jump() {
        // Check if the player is grounded before jumping by checking for colliders under them
        if (IsGrounded()) {
            Vector3 velocity = rbody.velocity;
            velocity.y += jumpImpulse;
            rbody.velocity = velocity;
        }
    }

    bool IsGrounded() {
        return Physics.CheckCapsule(frontPosition.position, backPosition.position, colliderRadius, 
            1 << LayerMask.NameToLayer("Ground"));
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
            if (IsGrounded()) {
                grabbedObject.transform.SetParent(null); // Set parent to world
                grabbedObject = null;
            }
        }
        else { // No grabbed object, grab current grab target if exists
            if (grabTarget == null) return;
            
            Grabbable grabScript = grabTarget.GetComponent<Grabbable>();

            // Reparent to move with Cat
            grabTarget.transform.SetParent(grabPoint, false);
            
            /* Set up transform of grabbed object */
            // Rotate so that the item is facing the same direction as the Cat
            // (ensures item is held the same way when grabbed from any angle)
            grabTarget.transform.rotation = Quaternion.LookRotation(this.transform.forward);
            // Set position to Cat's grabPoint
            grabTarget.transform.position = grabPoint.transform.position;
            // Offset position by grabbed object's grabPoint
            grabTarget.transform.position += grabTarget.transform.position - grabScript.grabPoint.position;
            
            // If grabbed object has an ItemSpawner managing it, alert it that its item has been grabbed
            if (grabScript.Manager != null) {
                grabScript.Manager.ItemGrabbed(grabTarget);
            }
            
            // Move grabTarget to be the grabbedObject
            grabbedObject = grabTarget;
            grabTarget = null;
        }
    }
}
