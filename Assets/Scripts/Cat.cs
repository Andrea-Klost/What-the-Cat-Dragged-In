using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {
    [Header("Inscribed")] 
    public float movementSpeed = 10f;
    
    void Update() {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * movementSpeed * Time.deltaTime;
        pos.z += vAxis * movementSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
