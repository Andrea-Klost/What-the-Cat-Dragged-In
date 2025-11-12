using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This component allows a GameObject to be grabbed by the player. For intended behavior the GameObject should be
/// on the Grabbable layer and have a trigger collider.
/// </summary>
public class Grabbable : MonoBehaviour {
    [Tooltip("Defines point that the Cat will grab the object from. If not set it will default to the object's transform")]
    public Transform grabPoint;

    void Awake() {
        if (grabPoint == null) { // If not set default to object's transform
            grabPoint = gameObject.transform;
        }
    }
}
