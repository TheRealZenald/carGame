using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Transform centerOfMass;
    public WheelCollider[] wheelColliders;
    public Transform[] wheelVisuals;
    public float maxMotorTorque = 4000f;
    public float maxBrakeTorque = 1000f;
    public float maxSteeringAngle = 30f;
    public float handbrakeTorque = 5000f;
    public float driftStiffness = 0.2f;
    public ParticleSystem tireSmoke;

    private Rigidbody rb;
    private float currentSpeed;
    private float driftFactor;
    private bool isDrifting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        currentSpeed = rb.velocity.magnitude * 3.6f; // Convert m/s to km/h

        CheckDrifting();
    }

    private void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        if (isDrifting)
        {
            ApplyDriftStiffness();
        }

        ApplyMotorTorque(motor);
        ApplySteering(steering);
        ApplyBrakes();

        UpdateWheelVisuals();
    }

    private void CheckDrifting()
    {
        if (Input.GetButton("Fire1") && currentSpeed > 30f) // For a handbrake effect
        {
            isDrifting = true;
            if (tireSmoke != null)
            {
                tireSmoke.Play();
            }
        }
        else
        {
            isDrifting = false;
            if (tireSmoke != null)
            {
                tireSmoke.Stop();
            }
        }
    }

    private void ApplyMotorTorque(float motor)
    {
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.motorTorque = motor;
        }
    }

    private void ApplySteering(float steering)
    {
        foreach (WheelCollider wheel in wheelColliders)
        {
            wheel.steerAngle = steering;
        }
    }

    private void ApplyBrakes()
    {
        foreach (WheelCollider wheel in wheelColliders)
        {
            if (isDrifting)
            {
                wheel.brakeTorque = handbrakeTorque;
            }
            else
            {
                wheel.brakeTorque = maxBrakeTorque;
            }
        }
    }

    private void ApplyDriftStiffness()
    {
        foreach (WheelCollider wheel in wheelColliders)
        {
            WheelFrictionCurve curve = wheel.sidewaysFriction;
            curve.stiffness = driftStiffness;
            wheel.sidewaysFriction = curve;
        }
    }

    private void UpdateWheelVisuals()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            UpdateSingleWheelVisual(wheelColliders[i], wheelVisuals[i]);
        }
    }

    private void UpdateSingleWheelVisual(WheelCollider wheelCollider, Transform wheelVisual)
    {
        // Match the visual wheel's position and rotation with the WheelCollider.
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelVisual.position = position;
        wheelVisual.rotation = rotation;
    }
}
