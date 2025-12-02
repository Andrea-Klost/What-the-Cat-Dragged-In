using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }
    
    [SerializeField] private RotationAxis axis = RotationAxis.Z;
    [SerializeField] private float rotationSpeed = 100f;

    void Update()
    {
        Vector3 rotationVector = axis switch
        {
            RotationAxis.X => new Vector3(rotationSpeed, 0f, 0f),
            RotationAxis.Y => new Vector3(0f, rotationSpeed, 0f),
            _               => new Vector3(0f, 0f, rotationSpeed)
        };

        transform.Rotate(rotationVector * Time.deltaTime);
    }
}
