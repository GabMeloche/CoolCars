using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float m_forwardTorque = 1000f;
    public float m_reverseTorque = 500f;
    public float m_steeringSpeed = 0.05f;
    public float m_maxSteerAngle = 60f;

    private List<WheelCollider> m_motorWheels = new List<WheelCollider>();
    private List<WheelCollider> m_steeringWheels = new List<WheelCollider>();
    private Rigidbody m_rigidbody;
    private bool m_reversing = false;

    void Start()
    {
        Wheel[] AllWheels;
        AllWheels = GetComponentsInChildren<Wheel>();

        m_rigidbody = GetComponent<Rigidbody>();

        foreach (Wheel wheel in AllWheels)
        {
            if (wheel.m_isMotor)
                m_motorWheels.Add(wheel.m_wheel);
            else if (wheel.m_IsSteering)
                m_steeringWheels.Add(wheel.m_wheel);
        }
    }

    void FixedUpdate()
    {
        

        Accelerate(Input.GetKey(KeyCode.W));
        SteerLeft(Input.GetKey(KeyCode.A));
        SteerRight(Input.GetKey(KeyCode.D));

        if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
            StopSteering();
    }

    private void Accelerate(bool p_shouldAccelerate)
    {
        if (p_shouldAccelerate)
        {
            foreach (WheelCollider wheel in m_motorWheels)
                wheel.motorTorque = m_forwardTorque;
        }
        else
        {
            foreach (WheelCollider wheel in m_motorWheels)
                wheel.motorTorque = 0f;
        }
    }

    private void SteerRight(bool p_shouldTurn)
    {
        if (p_shouldTurn)
        {
            foreach (WheelCollider wheel in m_steeringWheels)
            {
                wheel.steerAngle = Mathf.Lerp(m_maxSteerAngle, 0f, Time.deltaTime * m_steeringSpeed);
            }
        }
    }

    private void SteerLeft(bool p_shouldTurn)
    {    
        if (p_shouldTurn)
        {
            foreach (WheelCollider wheel in m_steeringWheels)
            {
                wheel.steerAngle = Mathf.Lerp(-m_maxSteerAngle, 0f, Time.deltaTime * m_steeringSpeed);
            }
        }
    }

    private void StopSteering()
    {
        foreach (WheelCollider wheel in m_steeringWheels)
        {
            wheel.steerAngle = Mathf.Lerp(0f, wheel.steerAngle, Time.deltaTime * m_steeringSpeed);
        }
    }

    /*private void BrakeOrReverse(bool p_keyPressed)
    {
        if (Mathf.Approximately(m_rigidbody.velocity.magnitude, 0f) && Input.GetKeyDown(KeyCode.))
        {
            m_reversing = !m_reversing;
        }
    }*/
}
