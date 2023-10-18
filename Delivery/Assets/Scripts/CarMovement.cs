using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Transform centerOfMass;
    public float maxSpeed = 100f;
    public float maxSteerAngle = 30f;
    public float acceleration = 10f;
    public float brakePower = 20f;
    
    private Rigidbody rb;
    private float currentSpeed;
    private float steeringAngle;
    private float verticalInput; // Declare verticalInput

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.centerOfMass = centerOfMass.position;
    }

    private void FixedUpdate()
    {
        HandleInput();
        ApplySteering();
        ApplyThrottle();
        ApplyBrakes();
        UpdateWheelVisuals();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // A and D or Left and Right arrow keys
        verticalInput = Input.GetAxis("Vertical"); // W and S or Up and Down arrow keys

        currentSpeed = transform.InverseTransformDirection(rb.velocity).z;
        steeringAngle = maxSteerAngle * horizontalInput;
    }

    private void ApplySteering()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelCollider wheel = transform.GetChild(i).GetComponent<WheelCollider>();
            wheel.steerAngle = steeringAngle;
        }
    }

    private void ApplyThrottle()
    {
        if (currentSpeed < maxSpeed)
        {
            for (int i = 0; i < 4; i++)
            {
                WheelCollider wheel = transform.GetChild(i).GetComponent<WheelCollider>();
                wheel.motorTorque = acceleration * Time.fixedDeltaTime * verticalInput;
            }
        }
    }

    private void ApplyBrakes()
    {
        if (verticalInput < 0)
        {
            for (int i = 0; i < 4; i++)
            {
                WheelCollider wheel = transform.GetChild(i).GetComponent<WheelCollider>();
                wheel.brakeTorque = brakePower;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                WheelCollider wheel = transform.GetChild(i).GetComponent<WheelCollider>();
                wheel.brakeTorque = 0;
            }
        }
    }

    private void UpdateWheelVisuals()
    {
        // You can add code to rotate and move the wheel mesh to match the WheelCollider's rotation and position.
    }
}
