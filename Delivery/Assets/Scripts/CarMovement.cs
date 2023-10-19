using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f;
    public float driftFactor = 0.9f;  // Adjust the drift intensity.
    public float maxSteerAngle = 45f;

    private float currentSpeed;
    private float horizontalInput;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Handle input for steering.
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        // Calculate current speed.
        currentSpeed = transform.InverseTransformDirection(rb.velocity).z;

        // Apply acceleration and steering.
        Accelerate();
        Steer();

        // Apply drift factor.
        ApplyDrift();
    }

    private void Accelerate()
    {
        rb.AddRelativeForce(Vector3.forward * speed);
    }

    private void Steer()
    {
        float steerAngle = maxSteerAngle * horizontalInput;
        transform.Rotate(0, steerAngle * Time.fixedDeltaTime, 0);
    }

    private void ApplyDrift()
    {
        Vector3 driftForce = -transform.right * (currentSpeed * (1 - driftFactor));
        rb.AddForce(driftForce);
    }
}
